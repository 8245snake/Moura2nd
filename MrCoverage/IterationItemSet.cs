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
        public static int MaxSequenceIndex = 0;

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

        /// <summary>
        /// 反復子
        /// </summary>
        public ObservableCollection<IterationItemColumn> Colmuns { get; set; }

        /// <summary>
        /// シーケンス
        /// </summary>
        public ObservableCollection<IterationItemColumn> Sequences { get; set; }

        public IterationItemSet()
        {
            Colmuns = new ObservableCollection<IterationItemColumn>();
            Colmuns.CollectionChanged += Colmuns_CollectionChanged;
            Sequences = new ObservableCollection<IterationItemColumn>();
        }

        private void Colmuns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // 要素数が変わると網羅回数が変わるためシーケンスを更新する
        }

        public void AddNewColumn()
        {
            MaxColumnIndex++;
            IterationItemColumn col = new IterationItemColumn("{#" + MaxColumnIndex + "}") { ColumnIndex = MaxColumnIndex };
            col.Items = new ObservableCollection<IterationItem>();
            col.Items.CollectionChanged += Colmuns_CollectionChanged;
            col.Items.Add(new IterationItem() { Text = "" });
            Colmuns.Add(col);
        }

        public void AddNewSequence()
        {
        }
    }
}
