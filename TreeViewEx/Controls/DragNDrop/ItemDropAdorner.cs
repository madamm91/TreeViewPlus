using System.Windows.Documents;
using System.Windows.Media;

namespace System.Windows.Controls.DragNDrop
{
    public class ItemDropAdorner : Adorner, IDisposable, IPositionable
    {
        Point position;
        AdornerLayer layer;
        readonly ContentPresenter contentPresenter;

        public ItemDropAdorner(TreeViewEx treeViewEx, DataTemplate dataTemplate, object binding = null)
            : base(treeViewEx)
        {
            layer = AdornerLayer.GetAdornerLayer(treeViewEx);
            layer.Add(this);

            contentPresenter = new ContentPresenter
            {
                Content = binding,
                ContentTemplate = dataTemplate
            };
        }

        public void UpdatePosition(Point position)
        {
            this.position = position;
            layer.Update();
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(position.X, position.Y));
            return result;
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
