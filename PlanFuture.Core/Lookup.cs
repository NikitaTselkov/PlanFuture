using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanFuture.Core
{
    public sealed class Lookup<TKey, TElement> : ILookup<TKey, TElement>
    {
        public readonly Dictionary<TKey, IList<TElement>> _dictionaryLookup = new();

        public Lookup() { }

        public Lookup(ILookup<TKey, TElement> lookup)
        {
            foreach (var grouping in lookup)
            {
                foreach (var element in grouping)
                    Add(grouping.Key, element);
            }
        }

        public IEnumerable<TElement> AllElements
        {
            get
            {
                return (from key in _dictionaryLookup.Keys
                        select _dictionaryLookup[key])
                        .SelectMany(list => list);
            }
        }

        public int Count => AllElements.Count();

        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                if (_dictionaryLookup.TryGetValue(key, out IList<TElement> list))
                    return list;

                return Array.Empty<TElement>();
            }
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            return GetGroupings().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (GetGroupings() as IEnumerable).GetEnumerator();
        }

        public bool Contains(TKey key)
        {
            return _dictionaryLookup.ContainsKey(key);
        }

        public void Add(TKey key, TElement element)
        {
            if (!_dictionaryLookup.TryGetValue(key, out IList<TElement> list))
            {
                list = new List<TElement>();
                _dictionaryLookup.Add(key, list);
            }

            list.Add(element);
        }

        public void Remove(TKey key, TElement element)
        {
            _dictionaryLookup[key].Remove(element);
        }

        public void RemoveKey(TKey key)
        {
            _dictionaryLookup.Remove(key);
        }

        private IEnumerable<IGrouping<TKey, TElement>> GetGroupings()
        {
            return from key in _dictionaryLookup.Keys
                   select new LookupDictionaryGrouping<TKey, TElement>
                   {
                       Key = key,
                       Elements = _dictionaryLookup[key]
                   } as IGrouping<TKey, TElement>;
        }
    }

    public class LookupDictionaryGrouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        public TKey Key { get; set; }

        public IEnumerable<TElement> Elements { get; set; }

        public IEnumerator<TElement> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (Elements as IEnumerable).GetEnumerator();
        }
    }
}
