using System.Collections.Generic;

namespace System.Windows.Controls.DragNDrop
{
    class DragInfo : IDragInfo
    {
        public object Data { get; set; }
        public DragDropEffects Effects { get; set; }
        public IEnumerable<TreeViewExItem> DraggedItems { get; set; }
        public IEnumerable<object> DraggedData { get; set; }
    }
}
