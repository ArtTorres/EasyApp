using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QApp.Documentation
{
    //TODO: To Delete
    public class HelpTemplate
    {
        public static string[] Build(string assemblyFile, IEnumerable<HelpAttribute> helpItems)
        {
            var output = new StringBuilder();

            string groupName = null;

            foreach (var helpItem in helpItems.OrderBy(s => s.Order))
            {
                if (!helpItem.Group.Equals(groupName))
                {
                    groupName = helpItem.Group;
                    output.AppendLine("[{0}]", groupName);
                }

                //var option = helpItem.GetOption();
                //if (null != option)
                //{
                //    output.Append(option.Name);

                //    if (!string.IsNullOrEmpty(option.Alias))
                //        output.AppendFormat(",{0}", option.Alias);

                //    output.Append(':');

                //    if (option.IsRequired)
                //        output.Append(" (Required)");

                //    if (option.IfPresent)
                //        output.Append(" (Present)");

                //    if (!string.IsNullOrEmpty(option.DefaultValue))
                //        output.AppendFormat(" (Default:{0})", option.DefaultValue);

                //    output.Append('\n');
                //}

                output.AppendLine("\t{0}", helpItem.Description);
                output.AppendLine("Example:");
                output.AppendLine("\t{0} {1}", assemblyFile, helpItem.Example);
            }

            return output.ToString().Split('\n');
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
