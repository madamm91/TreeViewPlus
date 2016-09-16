using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace System.Windows.Controls.DragNDrop
{
    public interface IDropInfo
    {
        IList<Adorner> Adorners { get; set; }
        TreeViewEx Tree { get; set; }
        InsertInfo InsertInfo { get; set; }
        TreeViewExItem TargetItem { get; set; }
        object TargetData { get; set; }
        DragEventArgs DragEventArgs { get; set; }
    }
}
