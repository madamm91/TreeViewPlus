using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace System.Windows.Controls
{
    class TreeViewLastChildConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TreeViewExItem item = (TreeViewExItem)value;
            ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
            return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}
