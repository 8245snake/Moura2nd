using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MrCoverage
{
    public class MouraTemplateEngine
    {
        private int _IterationCount = 0;

        /// <summary>
        /// 網羅するコレクション
        /// </summary>
        public List<IterationItemColumn> Columns { get; set; }

        private int[] CurrentIndexArray;
        private int[] MaxIndexArray;

        /// <summary>
        /// 現在の網羅カウント
        /// </summary>
        public int IterationCount { get => _IterationCount; }

        private void InitializeIndexArray()
        {
            Array.Resize(ref CurrentIndexArray, Columns.Count);
            Array.Resize(ref MaxIndexArray, Columns.Count);
            for (int i = 0; i < MaxIndexArray.Length; i++)
            {
                MaxIndexArray[i] = Columns[i].Items.Count - 1;
            }
        }

        private void IncreaseIndex()
        {
            // 末尾の桁から始める
            for (int i = CurrentIndexArray.Length -1 ; i >= 0; i--)
            {
                if (CurrentIndexArray[i] < MaxIndexArray[i])
                {
                    CurrentIndexArray[i]++;
                    _IterationCount++;
                    return;
                }
                // 繰り上がるのでこの桁は初期化
                CurrentIndexArray[i] = 0;
            }
            _IterationCount++;
        }

        private MouraItemSet GetCurrentMouraItemSet()
        {
            MouraItemSet set = new MouraItemSet();
            for (int i = 0; i < CurrentIndexArray.Length; i++)
            {
                set.Add(Columns[i].Name, Columns[i].Items[CurrentIndexArray[i]]);
            }
            return set;
        }

        /// <summary>
        /// カウントを次に進めて反復子を返す
        /// </summary>
        /// <returns>反復子セット</returns>
        private MouraItemSet GetAndIncrease()
        {
            var set = GetCurrentMouraItemSet();
            IncreaseIndex();
            return set;
        }

        /// <summary>
        /// 最後のインデックスまで達したかを判定する
        /// </summary>
        /// <returns>最後のインデックスまで達したらtrue</returns>
        private bool IsLastIndex()
        {
            for (int i = 0; i < CurrentIndexArray.Length; i++)
            {
                if (CurrentIndexArray[i] < MaxIndexArray[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 網羅セットを順番に返す
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MouraItemSet> EnumMouraItemSet()
        {
            // 初期化
            InitializeIndexArray();

            MouraItemSet set = null;
            while (!IsLastIndex())
            {
                set = GetAndIncrease();
                if (set != null)
                {
                    yield return set;
                }
            }
            // 最後の1件
            set = GetAndIncrease();
            if (set != null)
            {
                yield return set;
            }
        }


    }
}
