using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Controls.DragNDrop
{
    public interface IDragManager
    {
        bool CanDrag(IDragInfo dragInfo);
        void StartDrag(IDragInfo dragInfo);
    }
}
