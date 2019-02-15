using EasyApp.Documentation;
using EasyApp.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TWidgets.Core.Drawing;
using TWidgets.Widgets;

namespace EasyApp.Widgets
{
    public class Help : Widget
    {
        public string AssemblyFile { get; private set; }
        public IEnumerable<HelpAttribute> Items { get; private set; }

        public Help(string id, IEnumerable<HelpAttribute> helpItems)
            : base(id)
        {
            this.Margin.All = 0;
            this.Items = helpItems;
            this.AssemblyFile = AssemblyDescription.AssemblyFile;
        }

        public override void Draw(Graphics g)
        {
            var lines = Build(this.AssemblyFile, this.Items);
            foreach (var line in lines)
            {
                g.Draw(new Text(line, this.Margin));
            }
        }

        private string[] Build(string assemblyFile, IEnumerable<HelpAttribute> helpItems)
        {
            /*
                [Group]
                --option, -alias: (Required) (Present) (Default:<value>) (Values: A|B|C)
                  <description>
                Example:
                  assembly.exe <example>
            */

            var output = new StringBuilder();

            string groupName = null;

            foreach (var helpItem in helpItems.OrderBy(s => s.Group).ThenBy(s => s.Order))
            {
                if (!helpItem.Group.Equals(groupName))
                {
                    groupName = helpItem.Group;
                    output.AppendLine("[{0}]", groupName);
                }

                var option = helpItem.GetOption();
                if (null != option)
                {
                    output.AppendFormat("--{0}", option.Name);

                    if (!string.IsNullOrEmpty(option.Alias))
                        output.AppendFormat(", -{0}", option.Alias);

                    output.Append(':');

                    if (option.IsRequired)
                        output.Append(" (Required)");

                    if (option.IfPresent)
                        output.Append(" (Present)");

                    if (!string.IsNullOrEmpty(option.DefaultValue))
                        output.AppendFormat(" (Default:{0})", option.DefaultValue);

                    output.Append("\r\n");
                }

                output.AppendLine("   {0}", helpItem.Description);
                output.AppendLine("Example:");
                output.AppendLine("   {0} {1}", assemblyFile, helpItem.Example);
            }

            return output.ToString().Replace("\r", "").Split('\n');
        }
    }

    internal static class Extension
    {
        public static void AppendLine(this StringBuilder b, string format, params object[] args)
        {
            b.AppendLine(string.Format(format, args));
        }
    }
}
