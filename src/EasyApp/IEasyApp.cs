using EasyApp.Events;

namespace EasyApp
{
    public interface IEasyApp
    {
        AppSettings Settings { get; set; }

        void ShowHeader();

        void ShowHelp();

        bool ShowInputExceptions();

        void ShowEnvironment();

        void ShowArguments(string[] args);

        void MonitorTask(IEasyTask task);

        void ForgetTask(IEasyTask task);

        void Print(Message message);
    }
}
