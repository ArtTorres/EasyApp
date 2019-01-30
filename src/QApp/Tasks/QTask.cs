using QApp.Events;
using System;

namespace QApp
{
    public abstract class QTask
    {
        public event EventHandler<MessageEventArgs> Started;
        public void OnStarted(string message, params object[] args)
        {
            this.Started?.Invoke(this, new MessageEventArgs(MessageType.Resume, MessagePriority.High, message, args));
        }
        public void OnStarted(MessageType type, string message, params object[] args)
        {
            this.Started?.Invoke(this, new MessageEventArgs(type, MessagePriority.High, message, args));
        }
        public void OnStarted(MessageEventArgs e)
        {
            this.Started?.Invoke(this, e);
        }

        public event EventHandler<MessageEventArgs> Completed;
        public void OnCompleted(string message, params object[] args)
        {
            this.Completed?.Invoke(this, new MessageEventArgs(MessageType.Resume, MessagePriority.High, message, args));
        }
        public void OnCompleted(MessageType type, string message, params object[] args)
        {
            this.Completed?.Invoke(this, new MessageEventArgs(type, MessagePriority.High, message, args));
        }
        public void OnCompleted(MessageEventArgs e)
        {
            this.Completed?.Invoke(this, e);
        }

        public event EventHandler<MessageEventArgs> Progress;
        public void OnProgress(string message, params object[] args)
        {
            this.Progress?.Invoke(this, new MessageEventArgs(MessageType.Progress, MessagePriority.Medium, message, args));
        }
        public void OnProgress(MessageType type, string message, params object[] args)
        {
            this.Progress?.Invoke(this, new MessageEventArgs(type, MessagePriority.Medium, message, args));
        }
        public void OnProgress(MessageEventArgs e)
        {
            this.Progress?.Invoke(this, e);
        }

        public event EventHandler<MessageEventArgs> Failed;
        public void OnFailed(string message, params object[] args)
        {
            this.Failed?.Invoke(
                this,
                new MessageEventArgs(MessageType.Error, MessagePriority.High, message, args)
            );
        }
        public void OnFailed(MessageType type, string message, params object[] args)
        {
            this.Failed?.Invoke(
                this,
                new MessageEventArgs(type, MessagePriority.High, message, args)
            );
        }
        public void OnFailed(MessageEventArgs e)
        {
            this.Failed?.Invoke(this, e);
        }

        public event EventHandler<MessageEventArgs> Notification;
        public void OnNotification(MessageType type, string message, params object[] args)
        {
            this.Notification?.Invoke(
                this, 
                new MessageEventArgs(
                    type, 
                    MessagePriority.High, 
                    message, 
                    args
                )
            );
        }
        public void OnNotification(MessageEventArgs e)
        {
            this.Notification?.Invoke(this, e);
        }

        public abstract void Start();
    }
}
