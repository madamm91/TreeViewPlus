using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DragNDropSample.Model
{
    class BallonModel : INotifyPropertyChanged
    {
        public BallonModel(string hint)
        {
            Hint = hint;
        }
        private string m_hint;

        public string Hint
        {
            get { return m_hint; }
            set
            {
                m_hint = value;
                OnPropertyChanged(nameof(Hint));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
