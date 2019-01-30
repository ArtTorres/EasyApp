using MagnetArgs;
using QApp.Documentation;
using QApp.Events;
using System;
using System.Collections.Generic;
using System.Text;

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
        }

        public void Dispose()
        {
        }
    }
}
