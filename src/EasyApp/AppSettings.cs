using EasyApp.Documentation;
using EasyApp.Events;
using EasyApp.Parsers;
using MagnetArgs;

namespace EasyApp
{
    public class AppSettings : MagnetSet
    {
        public bool DisplayHeader { get; set; } = true;
        public bool DisplayArguments { get; set; } = true;
        public bool DisplayEnvironment { get; set; } = true;

        [Arg("help", Alias = "h"), IfPresent]
        [Help("Displays the application help instructions.", Example = "--help", Group = "Help", Order = 0)]
        public bool ShowHelp { get; set; }

        [Arg("log", Alias = "l"), Parser(typeof(PriorityParser)), Default("low")]
        [Help("Displays the logging options.", Example = "--log", Group = "Help", Order = 1)]
        public Priority MessagePriority { get; set; }
    }
}
