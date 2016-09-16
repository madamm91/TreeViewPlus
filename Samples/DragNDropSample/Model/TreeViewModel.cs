using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls.DragNDrop;
using W7StyleSample.Model;

namespace DragNDropSample.Model
{
    class TreeViewModel
    {
        public TreeViewModel()
        {
            Children = new ObservableCollection<Node>();
            SelectedItems = new ObservableCollection<Node>();
        }

        public ObservableCollection<Node> Children { get; set; }

        public ObservableCollection<Node> SelectedItems { get; set; } 
    }
}
