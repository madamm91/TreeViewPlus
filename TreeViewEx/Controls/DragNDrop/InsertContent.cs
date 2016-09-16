using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Controls.DragNDrop
{
    public class InsertContent : ItemContent
    {
        public bool Before { get; set; }

        public Point Position { get; set; }
    }
}
