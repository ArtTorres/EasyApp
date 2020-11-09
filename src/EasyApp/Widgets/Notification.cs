using EasyApp.Events;
using TWidgets;
using TWidgets.Core.Drawing;

namespace EasyApp.Widgets
{
    public class Notification : TWidgetBase
    {
        private Events.Message _message;
        public Events.Message Message
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

        public TWidgetColor DefaultColor { get; set; } = TWidgetColor.System;

        public Notification(string id) : base(id)
        { }

        public override void BeforeDraw()
        {
            this.SetColor(_message.Type);
        }

        public override void Draw(Graphics g)
        {
            g.Draw(new Text(
                FormatMessage(this.Message),
                this.Margin
            ));
        }

        private string FormatMessage(Events.Message message)
        {
            switch (message.Type)
            {
                case MessageType.Data:
                case MessageType.Resume:
                case MessageType.Highlight:
                case MessageType.Text:
                case MessageType.Help:
                case MessageType.Environment:
                    return message.Text;
                case MessageType.Arguments:
                    return string.Format("[IN] {0}", message.Text);
                default:
                    return string.Format(
                        "[{0}] {1}",
                        message.Type.ToString().ToUpper(),
                        message.Text
                    );
            }
        }

        private void SetColor(MessageType type)
        {
            switch (type)
            {
                case MessageType.Error:
                    this.ForegroundColor = TWidgetColor.Red;
                    break;
                case MessageType.Warning:
                case MessageType.Environment:
                    this.ForegroundColor = TWidgetColor.Yellow;
                    break;
                case MessageType.Data:
                    this.ForegroundColor = TWidgetColor.Green;
                    break;
                case MessageType.Resume:
                    this.ForegroundColor = TWidgetColor.Cyan;
                    break;
                case MessageType.Help:
                case MessageType.Progress:
                    this.ForegroundColor = TWidgetColor.White;
                    break;
                case MessageType.Arguments:
                case MessageType.Highlight:
                    this.ForegroundColor = TWidgetColor.Magenta;
                    break;
                default:
                    this.ForegroundColor = this.DefaultColor;
                    break;
            }
        }
    }
}
