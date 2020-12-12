using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MrCoverage
{
    /// <summary>
    /// 画面全体のビューモデル
    /// </summary>
    public class IterationItemSet
    {
        public static int MaxColumnIndex = 0;

        /// <summary>
        /// 合計網羅回数
        /// </summary>
        public int TotalIterationCount { get {
                int total = 1;
                foreach (var col in Colmuns)
                {
                    int count = col.Items.Count(item => item.IsEnabled);
                    if (count > 0)
                    {
                        total *= count;
                    }
                }
                return total;
            } }

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
