using System;
using System.Collections.Generic;
using System.Text;
using TWidgets.Widgets;

namespace QApp.Widgets
{
    public class EndMessage : Message
    {
        public EndMessage(string id) : base(id)
        {
        }

        public override void DrawComplete()
        {
            base.DrawComplete();

            Console.ReadKey();
        }
    }
}
