using System.Collections.Generic;

namespace System.Windows.Controls.DragNDrop
{
    public interface IDragInfo
    {
        object Data { get; set; }
        DragDropEffects Effects { get; set; }
        IEnumerable<TreeViewExItem> DraggedItems { get; set; }
        IEnumerable<object> DraggedData { get; set; }
    }
}
