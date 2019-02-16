using EasyApp.Events;
using MagnetArgs;
using System;

namespace EasyApp
{
    public static class AppRunner
    {
        public static void Execute<T>(string[] args, params IEasyTask[] tasks) where T : IEasyApp, new()
        {
            var app = new T();

            try
            {
                if (app.Settings.DisplayHeader)
                    app.ShowHeader();

                Magnet.Magnetize(app, args);

                if (app.Settings.DisplayArguments)
                    app.ShowArguments(args);

                if (app.Settings.ShowHelp)
                {
                    app.ShowHelp();
                }
                else if (!app.ShowInputExceptions())
                {
                    if (app.Settings.DisplayEnvironment)
                        app.ShowEnvironment();

                    foreach (var task in tasks)
                    {
                        Magnet.Magnetize(task, args);

                        app.MonitorTask(task);

                        task.BeforeStart();
                        task.Start();
                        task.AfterCompleted();
                    }
                }
            }
            catch (Exception ex)
            {
                app.Print(new Message()
                {
                    Type = MessageType.Error,
                    Priority = Priority.High,
                    Text = ex.Message
                });
            }
        }
    }
}
