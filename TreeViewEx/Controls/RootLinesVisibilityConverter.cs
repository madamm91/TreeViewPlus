using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace System.Windows.Controls
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class RootLinesVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var item = values.OfType<TreeViewExItem>().Single();
            return item.ParentTreeViewItem == null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
