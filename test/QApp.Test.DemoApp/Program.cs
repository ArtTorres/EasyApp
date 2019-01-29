using System;
using MagnetArgs;
using QApp;
using QApp.Documentation;

namespace QApp.Test.DemoApp
{
    class Program
    {
        private class DemoOptions : MagnetOption
        {
            [Arg("directory", Alias = "dir")]
            [Help("Sets the input directory.", Example = "--directory <path>", Group = "Authentication", Order = 1)]
            public string Directory { get; set; }
        }

        private class Application : QApplication
        {
            [OptionSet]
            private DemoOptions DemoOptions { get; set; }

            [OptionSet]
            private ExampleOptions TaskOptions { get; set; }

            public override void ExecutionProcess()
            {
                using (var process = new ExampleTask(this.TaskOptions))
                {
                    this.MonitorTask(process);
                    process.Start();
                }
            }
        }

        static void Main(string[] args)
        {
            args = new string[] { "-h" };
            var application = new Application();
            application.Execute(args);
            Console.ReadKey();
        }
    }
}
