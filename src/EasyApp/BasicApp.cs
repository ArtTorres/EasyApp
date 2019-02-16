using EasyApp.Events;
using EasyApp.Util;
using EasyApp.Widgets;
using System.Linq;
using TWidgets;
using TWidgets.Util;
using TWidgets.Widgets;

namespace EasyApp
{
    public class BasicApp : BaseApp
    {
        public Widget Header { get; set; }
        public Widget Help { get; set; }
        public Notification Notification { get; set; }
        public ProgressChar Progress { get; set; }

        protected bool _prevProgress = false;
        protected bool _prevNotification = false;

        public BasicApp()
        {
            this.Settings = new AppSettings();

            this.Header = new Header("easy_header");

            this.Help = new Help("easy_help", HelpUtils.GetHelpAttributes(this));

            this.Notification = new Notification("easy_notification");
            this.Notification.Margin.Left = 1;

            this.Progress = new ProgressChar("easy_progress");
            this.Progress.ForegroundColor = WidgetColor.White;
            this.Progress.Margin.Left = 1;
        }

        public override void ShowHeader()
        {
            WidgetPlayer.Mount(this.Header);
        }

        public override void ShowHelp()
        {
            WidgetPlayer.Mount(this.Help);
        }

        public override bool ShowInputExceptions()
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

        #region Print

        public void Print(MessageType type, Priority priority, string message, params object[] arg)
        {
            this.Print(new Events.Message
            {
                Type = type,
                Text = string.Format(message, arg),
                Priority = priority
            });
        }

        public override void Print(Events.Message message)
        {
            // skip message if lower priority
            if (this.Settings.MessagePriority < message.Priority) return;

            if (MessageType.Progress == message.Type)
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
    }
}
