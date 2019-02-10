using QApp.Events;
using TWidgets.Core;
using TWidgets.Core.Drawing;
using TWidgets.Util;

namespace QApp.Widgets
{
    public class Notification : Widget
    {
        private QMessage _message;
        public QMessage Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                this.OnStateChanged();
            }
        }

        public WidgetColor DefaultColor { get; set; } = WidgetColor.Gray;

        public Notification(string id) : base(id)
        { }

        public override void BeforeDraw()
        {
            this.SetColor(_message.MessageType);
        }

        public override void Draw(Graphics g)
        {
            g.Draw(new Text(
                FormatMessage(this.Message),
                this.Margin
            ));
        }

        private string FormatMessage(QMessage message)
        {
            switch (message.MessageType)
            {
                case MessageType.Data:
                case MessageType.Resume:
                case MessageType.Highlight:
                case MessageType.Text:
                case MessageType.Help:
                    return message.Text;
                case MessageType.Arguments:
                    return string.Format("[ARGS] {0}", message.Text);
                default:
                    return string.Format(
                        "[{0}] {1}",
                        message.MessageType.ToString().ToUpper(),
                        message.Text
                    );
            }
        }

        private void SetColor(MessageType type)
        {
            switch (type)
            {
                case MessageType.Error:
                    this.ForegroundColor = WidgetColor.Red;
                    break;
                case MessageType.Warning:
                    this.ForegroundColor = WidgetColor.Yellow;
                    break;
                case MessageType.Data:
                    this.ForegroundColor = WidgetColor.Green;
                    break;
                case MessageType.Resume:
                    this.ForegroundColor = WidgetColor.Cyan;
                    break;
                case MessageType.Help:
                case MessageType.Progress:
                    this.ForegroundColor = WidgetColor.White;
                    break;
                case MessageType.Arguments:
                case MessageType.Highlight:
                    this.ForegroundColor = WidgetColor.Magenta;
                    break;
                default:
                    this.ForegroundColor = this.DefaultColor;
                    break;
            }
        }
    }
}
