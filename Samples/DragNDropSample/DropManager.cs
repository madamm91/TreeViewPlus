using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.DragNDrop;
using DragNDropSample.Model;
using W7StyleSample.Model;

namespace DragNDropSample
{
    class DropManager : IDropManager
    {
        public DataTemplate DropZoneTemplate { get; set; }

        public DataTemplate InsertTemplate { get; set; }

        public DataTemplate ItemDropTemplate { get; set; }

        public DataTemplate ItemBorderTemplate { get; set; }

        private ItemDropAdorner floatingAdorner;

        private static int count = 0;

        public void DragOver(IDropInfo dropInfo)
        {
            var dataobject = dropInfo.DragEventArgs.Data as DataObject;
            if (dataobject?.GetDataPresent("File") == true)
            {
                dropInfo.Adorners.Add(new DropZoneAdorner(dropInfo.Tree, DropZoneTemplate));
                return;
            }

            var ballon = new BallonModel("Test");

            dropInfo.Adorners.Add(new ItemDropAdorner(dropInfo.Tree, ItemDropTemplate, ballon));
            Debug.WriteLine($"Called {++count} times.");

            if (dropInfo.InsertInfo != null)
            {
                dropInfo.Adorners.Add(new InsertAdorner(dropInfo.TargetItem, new InsertContent { Before = dropInfo.InsertInfo.Before }, InsertTemplate));
            }
            else
            {
                if (dropInfo.TargetItem == null)
                {
                    //dropInfo.Adorner = new DropZoneAdorner(dropInfo.Tree, DropZoneTemplate);
                }
                else
                {
                    //dropInfo.Adorner = new ItemDropAdorner(dropInfo.Tree, dropInfo.DragContent, ItemDropTemplate);
                    dropInfo.Adorners.Add(new ItemBorderAdorner(dropInfo.TargetItem, ItemBorderTemplate));
                }
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            var dataobject = dropInfo.DragEventArgs.Data as DataObject;
            var x = dropInfo.DragEventArgs;



            /*if (dataobject?.GetDataPresent("File") == true)
            {
                MessageBox.Show("File loaded");
                return;
            }

            if (dropInfo.DragEventArgs.Effects.HasFlag(DragDropEffects.Copy))
            {
                //copy
                var source = dropInfo.DragEventArgs.Data as DataObject;
                var items = source.GetData(typeof(object[])) as object[];
                var item = items[0] as Node;
                var newItem = new Node() { Name = item.Name + " Copy" };
                var target = dropInfo.TargetData as Node;
                target.Children.Add(newItem);
            }
            else
            {
                //addnew
                var source = dropInfo.DragEventArgs.Data as DataObject;
                var items = source.GetData(typeof(object[])) as object[];
                var item = items[0] as Node;
                var target = dropInfo.TargetData as Node;
                target.Children.Add(item);
            }*/

            //var x = 
            //throw new NotImplementedException();
        }
    }
}
