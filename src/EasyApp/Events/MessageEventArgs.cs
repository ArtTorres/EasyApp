using System;

namespace EasyApp.Events
{
    public class MessageEventArgs : EventArgs
    {
        public Message Message
        {
            get;
            private set;
        }

        public MessageEventArgs(string message, params object[] args)
            : this(string.Format(message, args))
        {
        }

        public MessageEventArgs(MessageType type, Priority priority, string message, params object[] args)
            : this(string.Format(message, args), type, priority)
        {
        }

        public MessageEventArgs(string message, MessageType type = MessageType.Info, Priority priority = Priority.High)
        {
            this.Message = new Message()
            {
                Text = message,
                Type = type,
                Priority = priority
            };
        }
    }
}
