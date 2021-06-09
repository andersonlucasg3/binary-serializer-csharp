using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BinarySerializer.Pool
{
    public class ListPool<TItem> : List<TItem>, IDisposable
    {
        private static readonly Dictionary<Type, ConcurrentBag<ListPool<TItem>>> _pool = new Dictionary<Type, ConcurrentBag<ListPool<TItem>>>();

        public static ListPool<TItem> Rent()
        {
            Type itemType = typeof(TItem);
            if (_pool.TryGetValue(itemType, out ConcurrentBag<ListPool<TItem>> bag))
            {
                return bag.TryTake(out ListPool<TItem> list) ? list : new ListPool<TItem>();
            }
            bag = new ConcurrentBag<ListPool<TItem>>();
            _pool[itemType] = bag;
            return new ListPool<TItem>();
        }
        
        public static void Pay(ListPool<TItem> list)
        {
            if (!_pool.TryGetValue(typeof(TItem), out ConcurrentBag<ListPool<TItem>> bag)) return;
            bag.Add(list);
        }

        public void Dispose()
        {
            Clear();
            Pay(this);
        }
    }
}
