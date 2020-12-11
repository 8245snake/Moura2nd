using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MrCoverage
{
    public class IterationItem : INotifyPropertyChanged
    {
        private string _Text = "";
        private bool _IsEnabled = true;

        public string Text { get => _Text; set { _Text = value; OnPropertyChanged(nameof(Text)); } }
        public bool IsEnabled { get => _IsEnabled; set { _IsEnabled = value; OnPropertyChanged(nameof(IsEnabled)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (null == this.PropertyChanged) return;
            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
