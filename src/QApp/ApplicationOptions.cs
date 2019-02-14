using MagnetArgs;
using QApp.Documentation;
using QApp.Events;
using QApp.Parsers;

namespace QApp
{
    public class ApplicationOptions : MagnetSet
    {
        [Arg("help", Alias = "h"), IfPresent]
        [Help("Displays the application help instructions.", Example = "--help", Group = "Help", Order = 0)]
        public bool ShowHelp { get; set; }

        [Arg("log", Alias = "l"), Parser(typeof(MessagePriorityParser)), Default("low")]
        [Help("Displays the logging options.", Example = "--log", Group = "Help", Order = 1)]
        public MessagePriority MessagePriority { get; set; }
    }
}
