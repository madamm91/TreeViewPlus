using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Threading;

namespace System.Windows.Controls.DragNDrop
{
    class DragNDropController : InputSubscriberBase, IDisposable
    {
        private AutoScroller autoScroller;

        private List<TreeViewExItem> draggableItems;

        private Cursor initialCursor;

        Stopwatch stopWatch;

        private DispatcherTimer dragAdornerClearer;

        private List<Adorner> actualAdorners = new List<Adorner>();

        const int dragAreaSize = 5;

        public DragNDropController(AutoScroller autoScroller)
        {
            this.autoScroller = autoScroller;
            InitDragAdornerClearer();
        }

        private void InitDragAdornerClearer()
        {
            // It appears there's a quirk in the drag/drop system.  While the user is dragging the object
            // over our control it appears the system will send us (quite frequently) DragLeave followed 
            // immediately by DragEnter events.  So when we get DragLeave, we can't be sure that the 
            // drag/drop operation was actually terminated. Cake is a lie. Therefore, instead of doing cleanup
            // immediately, we schedule the cleanup to execute later and if during that time we receive
            // another DragEnter or DragOver event, then we don't do the cleanup.

            dragAdornerClearer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(50)
            };
            dragAdornerClearer.Tick += DragAdornerClearerTick;
        }

        private void DragAdornerClearerTick(object sender, EventArgs e)
        {
            CleanUpAdorners();
        }

        internal override void Initialized()
        {
            base.Initialized();
            TreeView.AllowDrop = true;

            TreeView.Drop += OnDrop;
            TreeView.DragOver += OnDragOver;
            TreeView.DragLeave += OnDragLeave;
            TreeView.DragEnter += OnDragEnter;
            TreeView.GiveFeedback += OnGiveFeedBack;
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            dragAdornerClearer.Stop();
        }

        void OnDragLeave(object sender, DragEventArgs e)
        {
            dragAdornerClearer.Start();
        }

       

        public bool Enabled { get; set; }
        //private bool CanDrag { get { return draggableItems != null && draggableItems.Count > 0; } }
        internal override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (CheckOverScrollBar(e.GetPosition(TreeView))) return;

            // initalize draggable items on click. Doing that in mouse move results in drag operations,
            // when the border is visible.
            draggableItems = GetDraggableItems(e.GetPosition(TreeView));
            //if (CanDrag)
            //{
            //e.Handled = true;
            //}
        }

        internal override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            // otherwise drops are triggered even if no node was selected in drop
            draggableItems = null;
        }
        internal override void OnMouseMove(MouseEventArgs e)
        {
            if (!IsLeftButtonDown || CheckOverScrollBar(e.GetPosition(TreeView)))
            {
                CleanUpAdorners();
                return;
            }

            if (draggableItems == null || draggableItems.Count == 0) return;

            var dragInfo = new DragInfo()
            {
                DraggedItems = draggableItems,
                DraggedData = draggableItems.Select(x => x.DataContext).ToList(),
            };

            if (TreeView.DragManager?.CanDrag(dragInfo) == false) return;
            TreeView.DragManager?.StartDrag(dragInfo);
            DragStart();
            DragDo(dragInfo.Data, dragInfo.Effects);
            DragEnd();
            e.Handled = true;
        }

        private void CleanUpAdorners()
        {
            foreach (var adorner in actualAdorners)
            {
                (adorner as IDisposable)?.Dispose();
            }
            actualAdorners.Clear();
        }

        private void SetAdorners(IEnumerable<Adorner> adorners)
        {
            CleanUpAdorners();
            actualAdorners.AddRange(adorners);
        }

        /// <summary>
        /// Scrolls if mouse is pressed and over scroll border. 
        /// </summary>
        /// <param name="position">Mouse position relative to treeView control.</param>
        /// <returns>Returns true if over scroll border, otherwise false.</returns>
        internal bool TryScroll(Point position)
        {
            if (!IsLeftButtonDown) return false;

            double scrollDelta;
            if (position.Y < AutoScroller.scrollBorderSize)
            {
                //scroll down
                scrollDelta = -AutoScroller.scrollDelta;
            }
            else if ((TreeView.RenderSize.Height - position.Y) < AutoScroller.scrollBorderSize)
            {
                //scroll up
                scrollDelta = AutoScroller.scrollDelta;
            }
            else
            {
                stopWatch = null;
                return false;
            }

            if (stopWatch == null || stopWatch.ElapsedMilliseconds > AutoScroller.scrollDelay)
            {
                autoScroller.Scroll(scrollDelta);
                stopWatch = new Stopwatch();
                stopWatch.Start();
            }

            return true;
        }

        private void DragDo(object dragData, DragDropEffects effect)
        {
            DragDrop.DoDragDrop(TreeView, dragData, effect);
        }

        private void DragEnd()
        {
            DragDrop.RemoveGiveFeedbackHandler(TreeView, OnGiveFeedBack);

            TreeView.Cursor = initialCursor;
            autoScroller.IsEnabled = false;

            // Remove the drag adorner from the adorner layer.
            CleanUpAdorners();
            itemMouseIsOver = null;
        }

        private void DragStart()
        {
            initialCursor = TreeView.Cursor;
            autoScroller.IsEnabled = true;
        }

        private void OnGiveFeedBack(object sender, GiveFeedbackEventArgs e)
        {
            // disable switching to default cursors
            e.UseDefaultCursors = false;
            TreeView.Cursor = Cursors.Arrow;
            e.Handled = true;
        }

        private InsertInfo GetInsertInfo(TreeViewExItem item, Func<UIElement, Point> getPositionDelegate, IDataObject data)
        {
            if (item == null) return null;
            TreeViewExItem parentItem = item.ParentTreeViewItem;
            if (parentItem == null)
            {
                return null;
            }
            // get position over element
            Size size = item.RenderSize;
            Point positionRelativeToItem = getPositionDelegate(item);

            // decide whether to insert before or after item
            bool after = true;
            if (positionRelativeToItem.Y > dragAreaSize)
            {
                if (size.Height - positionRelativeToItem.Y > dragAreaSize)
                {
                    return null;
                }
            }
            else
            {
                after = false;
            }

            // get index, where to insert
            int index = parentItem.ItemContainerGenerator.IndexFromContainer(item);
            if (after)
            {
                // dont allow insertion after item, if item has children
                if (item.HasItems)
                {
                    return null;
                }
                index++;
            }

            return new InsertInfo(index, !after);

        }

        TreeViewExItem itemMouseIsOver;
        private DropInfo dropInfo;
        void OnDragOver(object sender, DragEventArgs e)
        {
            dragAdornerClearer.Stop();
            // drag over is the only event which returns the position
            // GiveFeedback returns nonsense even from Mouse.GetPosition
            Point point = e.GetPosition(TreeView);

            if (TryScroll(point)) return;

            if (IsMouseOverAdorner(point)) return;

            itemMouseIsOver = GetTreeViewItemUnderMouse(point);

            dropInfo = new DropInfo
            {
                Tree = TreeView,
                InsertInfo = GetInsertInfo(itemMouseIsOver, e.GetPosition, e.Data),
                TargetItem = itemMouseIsOver,
                TargetData = itemMouseIsOver?.DataContext,
                DragEventArgs = e,
            };

            TreeView.DropManager?.DragOver(dropInfo);
            var positionables = dropInfo.Adorners.OfType<IPositionable>();
            UpdatePosition(positionables, point);
            SetAdorners(dropInfo.Adorners);
        }

        private static void UpdatePosition(IEnumerable<IPositionable> positionables, Point point)
        {
            foreach (var positionable in positionables)
            {
                positionable.UpdatePosition(point);
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (dropInfo != null) TreeView.DropManager?.Drop(dropInfo);
            CleanUpAdorners();
        }

        private bool CheckOverScrollBar(Point positionRelativeToTree)
        {
            if (TreeView.ScrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible
                && positionRelativeToTree.X > TreeView.RenderSize.Width - SystemParameters.ScrollWidth)
            {
                return true;
            }

            if (TreeView.ScrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible
                && positionRelativeToTree.Y > TreeView.RenderSize.Height - SystemParameters.ScrollHeight)
            {
                return true;
            }

            return false;
        }

        private List<TreeViewExItem> GetDraggableItems(Point mousePositionRelativeToTree)
        {
            List<TreeViewExItem> items = TreeView.GetTreeViewItemsFor(TreeView.SelectedItems).ToList();
            TreeViewExItem itemUnderMouse = GetTreeViewItemUnderMouse(mousePositionRelativeToTree);
            if (itemUnderMouse == null) return new List<TreeViewExItem>();

            if (items.Contains(itemUnderMouse))
            {
                return items;
            }

            //mouse is not over an selected item. We have to check if it is over the content. In this case we have to select and start drag n drop.
            var contentPresenter = itemUnderMouse.Template.FindName("content", itemUnderMouse) as ContentPresenter;
            if (contentPresenter.IsMouseOver)
            {
                return new List<TreeViewExItem> { itemUnderMouse };
            }

            return new List<TreeViewExItem>();
        }

        public void Dispose()
        {
            if (TreeView != null)
            {
                TreeView.Drop -= OnDrop;
                TreeView.DragOver -= OnDragOver;
                TreeView.DragLeave -= OnDragLeave;
                TreeView.GiveFeedback -= OnGiveFeedBack;
            }
            itemMouseIsOver = null;
        }
    }
}
