using EasyApp.Events;
using System;

namespace EasyApp
{
    public interface IEasyTask
    {
        event EventHandler<MessageEventArgs> Started;

        event EventHandler<MessageEventArgs> Completed;

        event EventHandler<MessageEventArgs> Progress;

        event EventHandler<MessageEventArgs> Failed;

        event EventHandler<MessageEventArgs> Notification;

        void BeforeStart();

        void Start();

        void AfterCompleted();
    }
}
