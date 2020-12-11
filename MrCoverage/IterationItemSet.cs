using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MrCoverage
{
    public class IterationItemSet
    {
        public static int MaxColumnIndex = 0;

        public ObservableCollection<IterationItemColumn> Colmuns { get; set; }

        public int SelectedIColumnIndex { get; set; }

        public IterationItemSet()
        {
            Colmuns = new ObservableCollection<IterationItemColumn>();
        }

        public void AddNewColumn()
        {
            MaxColumnIndex++;
            IterationItemColumn col = new IterationItemColumn("{#" + MaxColumnIndex + "}") { ColumnIndex = MaxColumnIndex };
            col.Items = new ObservableCollection<IterationItem>();
            col.Items.Add(new IterationItem() { Text = "" });
            Colmuns.Add(col);
        }
    }
}
