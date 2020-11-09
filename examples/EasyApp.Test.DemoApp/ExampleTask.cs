using EasyApp.Documentation;
using EasyApp.Events;
using MagnetArgs;
using TWidgets;

namespace EasyApp.Test.DemoApp
{
    class ExampleOptions : MagnetSet
    {
        [Arg("output-directory", Alias = "out"), IsRequired]
        [Help("Sets the output directory.", Example = "--output-directory <path>", Group = "Authentication", Order = 2)]
        public string OutputDirectory { get; set; }
    }

    class ExampleTask : EasyTask
    {
        public ExampleOptions Options { get; set; }

        public override void Start()
        {
            this.OnStarted("Task Execution");
            this.OnNotification(MessageType.Text, "Output Directory: {0}", Options.OutputDirectory);

            for (int i = 0; i < 10;)
            {
                this.OnProgress(MessageType.Progress, $"Count: {++i}");
                System.Threading.Thread.Sleep(500);
            }

            this.OnNotification(MessageType.Text, "End of application");
        }

        public override void AfterCompleted()
        {
            TWidgetPlayer.Mount(new StopMessage("demo_stop") { Text = "-- Press Any Key To Finish --" });
        }
    }
}
