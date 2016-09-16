using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Controls.DragNDrop
{
    public class InsertInfo
    {
        public InsertInfo(int index, bool before)
        {
            Index = index;
            Before = before;
        }

        public int Index { get; set; }
        
        public bool Before { get; set; }
    }
}
