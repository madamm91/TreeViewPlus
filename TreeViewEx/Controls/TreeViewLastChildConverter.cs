using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace System.Windows.Controls
{
    class TreeViewLastChildConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var item = values.OfType<TreeViewExItem>().Single();
            var ic = ItemsControl.ItemsControlFromItemContainer(item);
            if (ic == null) return false;
            return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
