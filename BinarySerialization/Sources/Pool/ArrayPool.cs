using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BinarySerializer.Pool
{
    public static class ArrayPool<TArray>
    {
        private static readonly Dictionary<int, ConcurrentBag<TArray[]>> _pool = new Dictionary<int, ConcurrentBag<TArray[]>>();

        public static TArray[] Rent(int length)
        {
            if (_pool.TryGetValue(length, out ConcurrentBag<TArray[]> bag))
            {
                return bag.TryTake(out TArray[] array) ? array : new TArray[length];
            }
            bag = new ConcurrentBag<TArray[]>();
            _pool[length] = bag;
            return new TArray[length];

        }

        public static void Pay(TArray[] array)
        {
            if (!_pool.TryGetValue(array.Length, out ConcurrentBag<TArray[]> bag)) return;
            bag.Add(array);
        }
    }
}
