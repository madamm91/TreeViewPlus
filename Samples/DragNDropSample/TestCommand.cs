using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DragNDropSample
{
    class TestCommand : ICommand
    {
        public void Execute(object parameter)
        {
            Debug.WriteLine("Hit");
        }

        public bool CanExecute(object parameter)=>true;

        public event EventHandler CanExecuteChanged;
    }
}
