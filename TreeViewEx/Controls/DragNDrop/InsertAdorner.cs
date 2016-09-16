using System.Windows.Documents;
using System.Windows.Media;

namespace System.Windows.Controls.DragNDrop
{
    public class InsertAdorner : Adorner, IDisposable
    {
        AdornerLayer layer;
        internal TreeViewExItem treeViewItem;
        readonly ContentPresenter contentPresenter;

        public InsertAdorner(TreeViewExItem treeViewItem, InsertContent content, DataTemplate dataTemplate)
            : base(GetParentBorder(treeViewItem))
        {
            this.treeViewItem = treeViewItem;

            layer = AdornerLayer.GetAdornerLayer(AdornedElement);
            layer.Add(this);

            contentPresenter = new ContentPresenter
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Width = treeViewItem.ActualWidth - treeViewItem.Offset,
                Content = content,
                ContentTemplate = dataTemplate
            };

            content.Item = treeViewItem;
        }

        public static Border GetParentBorder(TreeViewExItem item)
        {
            Border border = item.Template.FindName("border", item) as Border;
            return border;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            Rect adornedElementRect = new Rect(AdornedElement.RenderSize);
            double positionX = adornedElementRect.Left + treeViewItem.Offset;
            var positionY = Content.Before ? adornedElementRect.Top : adornedElementRect.Bottom;

            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(positionX, positionY - contentPresenter.ActualHeight / 2));
            return result;
        }

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index) => contentPresenter;

        protected override Size MeasureOverride(Size constraint)
        {
            Rect adornedElementRect = new Rect(AdornedElement.RenderSize);
            contentPresenter.Measure(new Size(adornedElementRect.Width, constraint.Height));
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

        internal InsertContent Content => (InsertContent)contentPresenter.Content;
    }
}
