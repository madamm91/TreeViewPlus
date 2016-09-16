using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;

namespace System.Windows.Controls.DragNDrop
{
    public class ItemBorderAdorner : Adorner, IDisposable
    {
        AdornerLayer layer;
        readonly ContentPresenter contentPresenter;

        public ItemBorderAdorner(TreeViewExItem treeViewItem, DataTemplate dataTemplate, object binding = null) : base(GetParentBorder(treeViewItem))
        {
            layer = AdornerLayer.GetAdornerLayer(treeViewItem);
            layer.Add(this);
            var border = AdornedElement as Border;

            contentPresenter = new ContentPresenter
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Width = border.ActualWidth,
                Height = border.ActualHeight,
                //Height = treeViewItem.ActualHeight,
                Content = binding,
                ContentTemplate = dataTemplate
            };
        }

        public static Border GetParentBorder(TreeViewExItem item)
        {
            Border border = item.Template.FindName("border", item) as Border;
            return border;
        }

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index) => contentPresenter;

        protected override Size MeasureOverride(Size constraint)
        {
            contentPresenter.Measure(AdornedElement.RenderSize);
            return contentPresenter.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            contentPresenter.Arrange(new Rect(finalSize));
            return finalSize;
        }

        public void Dispose()
        {
            if (layer == null) return;
            layer.Remove(this);
            layer = null;
        }
    }
}
