using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace System.Windows.Controls.DragNDrop
{
    public class DropZoneAdorner : Adorner, IDisposable
    {
        private AdornerLayer layer;
        readonly ContentPresenter contentPresenter;

        public DropZoneAdorner(TreeViewEx treeViewEx, DataTemplate dataTemplate, object binding = null) : base(treeViewEx)
        {
            layer = AdornerLayer.GetAdornerLayer(treeViewEx);
            layer.Add(this);

            contentPresenter = new ContentPresenter
            {
                Content = binding,
                ContentTemplate = dataTemplate
            };
        }

        protected override Visual GetVisualChild(int index) => contentPresenter;

        protected override int VisualChildrenCount => 1;

        protected override Size MeasureOverride(Size constraint) => AdornedElement.RenderSize;

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
