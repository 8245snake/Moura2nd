using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MrCoverage
{
    /// <summary>
    /// 網羅セット
    /// </summary>
    public class MouraItemSet : IEnumerable<IterationItem>
    {
        private Dictionary<string, IterationItem> _IterationItemDictionary = new Dictionary<string, IterationItem>();

        public IterationItem this[string key]
        {
            get => _IterationItemDictionary[key];
            set => _IterationItemDictionary[key] = value;
        }

        public IEnumerable<string> Keys { get => _IterationItemDictionary.Keys; }

        public int Count => _IterationItemDictionary.Count;

        public void Add(string key, IterationItem value)
        {
            _IterationItemDictionary.Add(key, value);
        }

        public IEnumerator<IterationItem> GetEnumerator()
        {
            return _IterationItemDictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(" ", _IterationItemDictionary.Keys.Select(key => key + "=" + _IterationItemDictionary[key].Text));
        }
    }
}
