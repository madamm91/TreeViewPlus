using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace System.Windows.Controls.DragNDrop
{
    class DropInfo : IDropInfo
    {
        public IList<Adorner> Adorners { get; set; } = new List<Adorner>();
        public TreeViewEx Tree { get; set; }
        public InsertInfo InsertInfo { get; set; }
        public TreeViewExItem TargetItem { get; set; }
        public DragEventArgs DragEventArgs { get; set; }
        public object TargetData { get; set; }
    }
}
