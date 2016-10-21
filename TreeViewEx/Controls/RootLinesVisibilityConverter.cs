using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace System.Windows.Controls
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class RootLinesVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TreeViewExItem item = (TreeViewExItem)value;
            return item.ParentTreeViewItem == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}
