using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;

namespace MrCoverage
{
    /// <summary>
    /// 反復子のコレクションクラス
    /// </summary>
    public class IterationItemColumn : INotifyPropertyChanged
    {
        private IterationItem _SelectedItem;
        private int _CurrentWidth = 100;
        public int CurrentWidth { 
            get => _CurrentWidth;
            set { _CurrentWidth = value;OnPropertyChanged(nameof(CurrentWidth)); } }

        public ObservableCollection<IterationItem> Items { get; set; }

        public IterationItem SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChanged(nameof(SelectedItem)); } }

        public int SelectedIndex
        {
            get {
                if (SelectedItem == null) { return -1; }
                return Items.IndexOf(SelectedItem);
            }
            set {
                IterationItem item = Items[value];
                SelectedItem = item;
            }
        }

        public string Name { get; set; }

        public int ColumnIndex { get; set; }

        public IterationItemColumn(string name)
        {
            Items = new ObservableCollection<IterationItem>();
            CurrentWidth = 200;
            Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (null == this.PropertyChanged) return;
            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
