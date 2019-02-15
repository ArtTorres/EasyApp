using EasyApp.Util;
using TWidgets.Widgets;

namespace EasyApp.Widgets
{
    public class Header: Marquee
    {
        public Header(string id) : base(id)
        {
            this.Margin.All = 1;
            this.Margin.Top = 0;
            this.Padding.All = 1;
            this.Items = AssemblyDescription.Description;
        }
    }
}
