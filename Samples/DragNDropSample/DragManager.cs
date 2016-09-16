using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DragNDrop;

namespace DragNDropSample
{
    class DragManager : IDragManager
    {
        public bool CanDrag(IDragInfo dragInfo)
        {
            return true;
        }

        public void StartDrag(IDragInfo dragInfo)
        {
            dragInfo.Data = dragInfo.DraggedData.ToArray();
            dragInfo.Effects = DragDropEffects.All;
        }
    }
}
