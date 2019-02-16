using EasyApp.Events;
using System;

namespace EasyApp
{
    public abstract class BaseApp : IEasyApp
    {
        public AppSettings Settings { get; set; }

        public virtual void ShowHeader() { }

        public virtual void ShowHelp() { }

        public abstract bool ShowInputExceptions();

        public virtual void ShowEnvironment()
        {
            this.Print(new Message()
            {
                Type = MessageType.Environment,
                Priority = Priority.High,
                Text = $"OS: {Environment.OSVersion}, Runtime: {Environment.Version}"
            });
        }

        public virtual void ShowArguments(string[] args)
        {
            this.Print(new Message()
            {
                Type = MessageType.Arguments,
                Priority = Priority.High,
                Text = string.Join(" ", args)
            });
        }

        public abstract void Print(Message message);

        #region Events

        public void MonitorTask(IEasyTask task)
        {
            task.Started += this.OnTaskStarted;
            task.Completed += this.OnTaskCompleted;
            task.Failed += this.OnTaskFailed;
            task.Progress += this.OnTaskProgress;
            task.Notification += this.OnTaskNotification;
        }

        public void ForgetTask(IEasyTask task)
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
