using System;
namespace QApp.Events
{
    public class MessageEventArgs : EventArgs
    {
        public QMessage Message
        {
            get;
            private set;
        }

        public MessageEventArgs(string message, params object[] args)
            : this(string.Format(message, args))
        {
        }

        public MessageEventArgs(MessageType type, MessagePriority priority, string message, params object[] args)
            : this(string.Format(message, args), type, priority)
        {
        }

        public MessageEventArgs(string message, MessageType type = MessageType.Info, MessagePriority priority = MessagePriority.High)
        {
            this.Message = new QMessage()
            {
                Text = message,
                MessageType = type,
                Priority = priority
            };
        }
    }
}
