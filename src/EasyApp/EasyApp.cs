using MagnetArgs;
using EasyApp.Events;
using EasyApp.Util;
using EasyApp.Widgets;
using System;
using System.Linq;
using TWidgets;
using TWidgets.Util;
using TWidgets.Widgets;

namespace EasyApp
{
    public abstract class EasyApp
    {
        protected ConsoleColor DefaultColor = ConsoleColor.Gray;
        protected ConsoleColor _initialColor;

        public bool DisplayHeader { get; set; }
        public bool DisplayArguments { get; set; }
        public bool DisplayEnvironment { get; set; }

        public Widget Header { get; set; }
        public Widget Help { get; set; }
        public Notification Notification { get; set; }
        public ProgressChar Progress { get; set; }

        protected bool _prevProgress = false;
        protected bool _prevNotification = false;

        public EasyAppArguments AppOptions { get; set; }

        public EasyApp()
        {
            this.DisplayHeader = true;
            this.DisplayEnvironment = true;
            this.DisplayArguments = true;

            this.Header = new Header("easy_header");
            this.Help = new Help("easy_help", HelpUtils.GetHelpAttributes(this));

            this.Notification = new Notification("easy_notification");
            this.Notification.Margin.Left = 1;

            this.Progress = new ProgressChar("easy_progress");
            this.Progress.ForegroundColor = WidgetColor.White;
            this.Progress.Margin.Left = 1;
        }

        public void ShowHeader()
        {
            WidgetPlayer.Mount(this.Header);
        }

        public void ShowHelp()
        {
            WidgetPlayer.Mount(this.Help);
        }

        public bool ShowInputExceptions()
        {
            var errors = ExceptionUtils.GetInputExceptions(this).Select(ex => ex.Message).ToArray();
            bool found = errors.Count() > 0;

            if (found)
            {
                var widget = new BulletList("easy_errors")
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
            this.Print(MessageType.Environment, Priority.High, "Environment Version: {0}", Environment.Version.ToString());
        }

        public void ShowArguments(string[] args)
        {
            this.Print(MessageType.Arguments, Priority.High, string.Join(" ", args));
        }

        public abstract void ExecutionProcess();

        public void Execute(string[] args)
        {
            try
            {
                if (this.DisplayHeader)
                    this.ShowHeader();

                Magnet.Magnetize(this, args);

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

                    this.ExecutionProcess();
                }
            }
            catch (Exception ex)
            {
                this.Print(MessageType.Error, Priority.High, ex.Message);
            }
        }

        #region Print

        protected void Print(MessageType type, Priority priority, string message, params object[] arg)
        {
            this.Print(new EasyMessage
            {
                MessageType = type,
                Text = string.Format(message, arg),
                Priority = priority
            });
        }

        protected void Print(EasyMessage message)
        {
            // skip message if lower priority
            if (this.AppOptions.MessagePriority < message.Priority) return;

            if (MessageType.Progress == message.MessageType)
            { // Display progress message
                this.Progress.Text = message.Text;

                if (!_prevProgress)
                {
                    WidgetPlayer.Mount(this.Progress);
                    _prevProgress = true;
                    _prevNotification = false;
                }
            }
            else
            { // Display notification message
                this.Notification.Message = message;

                if (!_prevNotification)
                {
                    WidgetPlayer.Mount(this.Notification);
                    _prevNotification = true;
                    _prevProgress = false;
                }
            }
        }

        #endregion

        #region Events

        protected void MonitorTask(EasyTask task)
        {
            task.Started += this.OnTaskStarted;
            task.Completed += this.OnTaskCompleted;
            task.Failed += this.OnTaskFailed;
            task.Progress += this.OnTaskProgress;
            task.Notification += this.OnTaskNotification;
        }

        protected void ForgetTask(EasyTask task)
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
