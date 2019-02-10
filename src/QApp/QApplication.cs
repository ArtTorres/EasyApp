using MagnetArgs;
using QApp.Documentation;
using QApp.Events;
using QApp.Options;
using QApp.Util;
using QApp.Widgets;
using System;
using System.Linq;
using TWidgets;
using TWidgets.Core;
using TWidgets.Util;
using TWidgets.Widgets;

namespace QApp
{
    public class MessagePriorityParser : IParser
    {
        public object Parse(string value)
        {
            switch (value.ToLowerInvariant())
            {
                case "high":
                    return MessagePriority.High;
                case "medium":
                    return MessagePriority.Medium;
                case "low":
                    return MessagePriority.Low;
                default:
                    throw new Exception(string.Format("Value {0} for MessagePriority not found.", value));
            }
        }
    }

    public class ApplicationOptions : MagnetOption
    {
        [Arg("help", Alias = "h"), IfPresent]
        [Help("Displays the application help instructions.", Example = "--help", Group = "Help", Order = 0)]
        public bool ShowHelp { get; set; }

        [Arg("log", Alias = "l"), Parser(typeof(MessagePriorityParser)), Default("low")]
        [Help("Displays the logging options.", Example = "--log", Group = "Help", Order = 1)]
        public MessagePriority MessagePriority { get; set; }
    }

    public abstract class QApplication
    {
        protected ConsoleColor DefaultColor = ConsoleColor.Gray;

        protected bool _prevProgressMessage = false;
        protected ConsoleColor _initialColor;

        public bool DisplayHeader { get; set; }
        public bool DisplayArguments { get; set; }
        public bool DisplayEnvironment { get; set; }

        public Widget Header { get; set; }
        public Widget Help { get; set; }
        //public Notification Notification { get; set; }
        //public Widget InputExceptions { get; set; }

        [OptionSet]
        public ApplicationOptions AppOptions { get; set; }

        public QApplication()
        {
            //_initialColor = Console.ForegroundColor;

            this.DisplayHeader = true;
            this.DisplayEnvironment = true;
            this.DisplayArguments = true;

            this.Header = new Header("app_header");
            //this.Notification = new Notification("app_notification");
            //this.Notification.Margin.Left = 1;
        }

        public void ShowHeader()
        {
            WidgetPlayer.Mount(this.Header);
        }

        public void ShowHelp()
        {
            this.Help = new Help("app_help", HelpUtils.GetHelpAttributes(this));

            WidgetPlayer.Mount(this.Help);
        }

        public bool ShowInputExceptions()
        {
            var errors = ExceptionUtils.GetInputExceptions(this).Select(ex => ex.Message).ToArray();
            bool found = errors.Count() > 0;

            if (found)
            {
                var widget = new BulletList("app_errors")
                {
                    Items = errors,
                    ForegroundColor = WidgetColor.Red
                };
                widget.Margin.All = 1;

                WidgetPlayer.Mount(widget);
            }

            return found;
        }

        public void ShowEnvironment()
        {
            this.Print(MessageType.Warning, MessagePriority.High, "Environment Version: {0}", Environment.Version.ToString());
        }

        public void ShowArguments(string[] args)
        {
            this.Print(MessageType.Arguments, MessagePriority.High, string.Join(" ", args));
        }

        public abstract void ExecutionProcess();

        public void Execute(string[] args)
        {
            try
            {
                if (this.DisplayHeader)
                    this.ShowHeader();

                Magnet.Magnetize(this, args);

                //WidgetPlayer.Mount(this.Notification);

                if (this.DisplayArguments)
                    this.ShowArguments(args);

                if (this.AppOptions.ShowHelp)
                {
                    this.ShowHelp();
                }
                else if (!this.ShowInputExceptions())
                {
                    if (this.DisplayEnvironment)
                        this.ShowEnvironment();

                    //Console.ForegroundColor = _initialColor;

                    this.ExecutionProcess();
                }
            }
            catch (Exception ex)
            {
                this.Print(MessageType.Error, MessagePriority.High, ex.Message);
            }
            finally
            {
                //Console.ForegroundColor = _initialColor;
            }
        }

        #region Print

        protected void Print(string message, params object[] arg)
        {
            Console.WriteLine(message, arg);
        }

        protected void Print(MessageType type, MessagePriority priority, string message, params object[] arg)
        {
            this.Print(new QMessage
            {
                MessageType = type,
                Text = string.Format(message, arg),
                Priority = priority
            });
        }

        protected void Print(QMessage message)
        {
            // skip message if lower priority
            if (this.AppOptions.MessagePriority < message.Priority) return;

            if (MessageType.Progress == message.MessageType)
            {
                this.SetConsoleColor(message.MessageType);

                if (this._prevProgressMessage && message.MessageType != MessageType.Progress)
                {
                    Console.Write("\n");
                }

                switch (message.MessageType)
                {
                    case MessageType.Data:
                    case MessageType.Resume:
                    case MessageType.Highlight:
                    case MessageType.Text:
                    case MessageType.Help:
                        Console.WriteLine("{0}", message.Text);
                        this._prevProgressMessage = false;
                        break;
                    case MessageType.Progress:
                        Console.Write("\r{0}", message.Text);
                        this._prevProgressMessage = true;
                        break;
                    case MessageType.Arguments:
                        Console.WriteLine("[ARGS] {0}", message.Text);
                        this._prevProgressMessage = false;
                        break;
                    default:
                        Console.WriteLine("[{0}] {1}", message.MessageType.ToString().ToUpper(), message.Text);
                        this._prevProgressMessage = false;
                        break;
                }
            }
            else
            {
                var widget = new Notification("app_notification")
                {
                    Message = message
                };
                widget.Margin.Left = 1;

                WidgetPlayer.Mount(widget);
                //this.Notification.Message = message;
            }
        }

        protected void SetConsoleColor(MessageType type)
        {
            switch (type)
            {
                case MessageType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    return;
                case MessageType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    return;
                case MessageType.Data:
                    Console.ForegroundColor = ConsoleColor.Green;
                    return;
                case MessageType.Resume:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    return;
                case MessageType.Help:
                case MessageType.Progress:
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                case MessageType.Arguments:
                case MessageType.Highlight:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    return;
            }
            Console.ForegroundColor = this.DefaultColor;
        }

        #endregion

        #region Events

        protected void MonitorTask(QTask task)
        {
            task.Started += this.OnTaskStarted;
            task.Completed += this.OnTaskCompleted;
            task.Failed += this.OnTaskFailed;
            task.Progress += this.OnTaskProgress;
            task.Notification += this.OnTaskNotification;
        }

        protected void ForgetTask(QTask task)
        {
            task.Started -= this.OnTaskStarted;
            task.Completed -= this.OnTaskCompleted;
            task.Failed -= this.OnTaskFailed;
            task.Progress -= this.OnTaskProgress;
            task.Notification -= this.OnTaskNotification;
        }

        protected void OnTaskProgress(object sender, MessageEventArgs e)
        {
            this.Print(e.Message);
        }

        protected void OnTaskFailed(object sender, MessageEventArgs e)
        {
            this.Print(e.Message);
        }

        protected void OnTaskCompleted(object sender, MessageEventArgs e)
        {
            this.Print(e.Message);
        }

        protected void OnTaskStarted(object sender, MessageEventArgs e)
        {
            this.Print(e.Message);
        }

        protected void OnTaskNotification(object sender, MessageEventArgs e)
        {
            this.Print(e.Message);
        }

        #endregion
    }
}
