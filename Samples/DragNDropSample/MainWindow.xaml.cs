using System.Linq;
using System.Windows.Controls;
using DragNDropSample.Model;

namespace W7StyleSample
{
    #region

    using System;
    using System.Windows;
    using System.Windows.Input;
    using W7StyleSample.Model;

    #endregion

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Point dragStartPoint;
        #region Constructors and Destructors

        private TreeViewModel tree;

        public MainWindow()
        {
            tree = new TreeViewModel();
            var firstNode = new Node { Name = "element" };
            tree.Children.Add(firstNode);
            var first1 = new Node { Name = "element1" };
            var first2 = new Node { Name = "element2 (Drop Allowed)" };
            var first11 = new Node { Name = "element11 (Drag Allowed)", AllowDrag = true };
            var first12 = new Node { Name = "element12 (Drag Allowed)", AllowDrag = true };
            var first13 = new Node { Name = "element13 (Insert Allowed)" };
            var first14 = new Node { Name = "element14 (Drop Allowed)" };
            var first15 = new Node { Name = "element15" };
            var first131 = new Node { Name = "element131" };
            var first132 = new Node { Name = "element132 (Drop Allowed)" };

            firstNode.Children.Add(first1);
            firstNode.Children.Add(first2);
            first1.Children.Add(first11);
            first1.Children.Add(first12);
            first1.Children.Add(first13);
            first1.Children.Add(first14);
            first1.Children.Add(first15);
            first13.Children.Add(first131);
            first13.Children.Add(first132);

            DataContext = tree;

            InitializeComponent();
        }


        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            dragStartPoint = e.GetPosition(null);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var textblock = sender as TextBlock;
            var text = textblock.Text;
            Point mousePos = e.GetPosition(null);
            Vector diff = dragStartPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed 
                && (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                var frameworkElem = ((FrameworkElement)e.OriginalSource);
                DragDrop.DoDragDrop(frameworkElem, new DataObject(text, frameworkElem.DataContext), DragDropEffects.Move);
            }
        }
        #endregion

        private void button_Click(object sender, RoutedEventArgs e)
        {
            tree.SelectedItems.Clear();
            tree.SelectedItems.Add(tree.Children.First());
        }
    }
}