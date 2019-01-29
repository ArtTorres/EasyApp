using System;
using System.Collections.Generic;
using System.Text;
using TWidgets.Core.Drawing;
using TWidgets.Widgets;

namespace QApp.Widgets
{
    public class Notification:Message
    {
        public Notification(string id):base(id)
        {
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }
    }
}
