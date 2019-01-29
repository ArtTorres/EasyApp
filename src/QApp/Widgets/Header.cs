using QApp.Util;
using TWidgets.Core.Drawing;
using TWidgets.Widgets;

namespace QApp.Widgets
{
    public class Header: Marquee
    {
        public Header(string id) : base(id)
        {
            this.Margin.All = 1;
            this.Margin.Top = 0;
            this.Padding.All = 1;
            this.Lines = AssemblyDescription.Description;
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }
    }
}
