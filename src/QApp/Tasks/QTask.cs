using QApp.Events;
using System;

namespace QApp
{
    public abstract class QTask
    {
        public event EventHandler<MessageEventArgs> Started;
        public void OnStarted(string message, params object[] args)
        {
            if (this.Started != null)
            {
                this.Started(this, new MessageEventArgs(MessageType.Resume, MessagePriority.High, message, args));
            }
        }
        public void OnStarted(MessageType type, string message, params object[] args)
        {
            if (this.Started != null)
            {
                this.Started(this, new MessageEventArgs(type, MessagePriority.High, message, args));
            }
        }
        public void OnStarted(MessageEventArgs e)
        {
            if (this.Started != null)
            {
                this.Started(this, e);
            }
        }

        public event EventHandler<MessageEventArgs> Completed;
        public void OnCompleted(string message, params object[] args)
        {
            if (this.Completed != null)
            {
                this.Completed(this, new MessageEventArgs(MessageType.Resume, MessagePriority.High, message, args));
            }
        }
        public void OnCompleted(MessageType type, string message, params object[] args)
        {
            if (this.Completed != null)
            {
                this.Completed(this, new MessageEventArgs(type, MessagePriority.High, message, args));
            }
        }
        public void OnCompleted(MessageEventArgs e)
        {
            if (this.Completed != null)
            {
                this.Completed(this, e);
            }
        }

        public event EventHandler<MessageEventArgs> Progress;
        public void OnProgress(string message, params object[] args)
        {
            if (this.Progress != null)
            {
                this.Progress(this, new MessageEventArgs(MessageType.Progress, MessagePriority.Medium, message, args));
            }
        }
        public void OnProgress(MessageType type, string message, params object[] args)
        {
            if (this.Progress != null)
            {
                this.Progress(this, new MessageEventArgs(type, MessagePriority.Medium, message, args));
            }
        }
        public void OnProgress(MessageEventArgs e)
        {
            if (this.Progress != null)
            {
                this.Progress(this, e);
            }
        }

        public event EventHandler<MessageEventArgs> Failed;
        public void OnFailed(string message, params object[] args)
        {
            if (this.Failed != null)
            {
                this.Failed(
                    this,
                    new MessageEventArgs(MessageType.Error, MessagePriority.High, message, args)
                );
            }
        }
        public void OnFailed(MessageType type, string message, params object[] args)
        {
            if (this.Failed != null)
            {
                this.Failed(
                    this,
                    new MessageEventArgs(type, MessagePriority.High, message, args)
                );
            }
        }
        public void OnFailed(MessageEventArgs e)
        {
            if (this.Failed != null)
            {
                this.Failed(this, e);
            }
        }

        public event EventHandler<MessageEventArgs> Notification;
        public void OnNotification(MessageEventArgs e)
        {
            if (this.Notification != null)
            {
                this.Notification(this, e);
            }
        }

        public abstract void Start();
    }
}
