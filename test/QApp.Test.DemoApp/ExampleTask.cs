using MagnetArgs;
using QApp.Documentation;
using QApp.Events;
using System;

namespace QApp.Test.DemoApp
{
    class ExampleOptions : MagnetOption
    {
        [Arg("output-directory", Alias = "out"), IsRequired]
        [Help("Sets the output directory.", Example = "--output-directory <path>", Group = "Authentication", Order = 2)]
        public string OutputDirectory { get; set; }
    }

    class ExampleTask : QTask, IDisposable
    {
        private ExampleOptions _options;

        public ExampleTask(ExampleOptions options)
        {
            _options = options;
        }

        public override void Start()
        {
            this.OnStarted("Task Execution");
            this.OnNotification(MessageType.Text, "Output Directory: {0}", _options.OutputDirectory);

            for (int i = 0; i < 10;)
            {
                this.OnProgress(MessageType.Progress, $"Count: {++i}");
                System.Threading.Thread.Sleep(500);
            }

            this.OnNotification(MessageType.Text, "End of application");
        }

        public void Dispose()
        {
        }
    }
}
