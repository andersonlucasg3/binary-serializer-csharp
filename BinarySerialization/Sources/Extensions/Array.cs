using System;
using BinarySerializer.Pool;

namespace BinarySerializer.Extensions
{
    public static class ArrayExt
    {
        public delegate bool FilterMapFunc<in TArray, TResult>(TArray item, out TResult result);

        public static bool IsNull(this Array array) => array == null;
        public static bool IsEmpty(this Array array) => array.Length == 0;
        public static bool IsNullOrEmpty(this Array array) => IsNull(array) || IsEmpty(array);

        public static bool Contains<TArray, TCompare>(this TArray[] array) where TCompare : TArray
        {
            if (array.IsNullOrEmpty()) return false;
            for (int index = 0; index < array.Length; index++)
                if (array[index] is TCompare)
                    return true;
            return false;
        }

        public static bool Contains<TArray>(this TArray[] array, TArray item)
        {
            if (array.IsNullOrEmpty()) return false;
            for (int index = 0; index < array.Length; index++)
                if (array[index].Equals(item))
                    return true;
            return false;
        }

        public static TResult[] Map<TArray, TResult>(this TArray[] array, Func<TArray, TResult> mapper)
        {
            if (array.IsNull()) return null;
            if (array.IsEmpty()) return Array.Empty<TResult>();
            TResult[] results = new TResult[array.Length];
            for (int index = 0; index < array.Length; index++) results[index] = mapper.Invoke(array[index]);
            return results;
        }

        public static TArray[] Filter<TArray>(this TArray[] array, Predicate<TArray> predicate)
        {
            using (ListPool<TArray> resultsList = new ListPool<TArray>())
            {
                for (int index = 0; index < array.Length; index++)
                {
                    TArray item = array[index];
                    if (predicate.Invoke(item)) resultsList.Add(item);
                }
                return resultsList.ToArray();
            }
        }

        public static TResult[] FilterMap<TArray, TResult>(this TArray[] array, FilterMapFunc<TArray, TResult> filterMapFunc)
        {
            if (array.IsNull()) return null;
            if (array.IsEmpty()) return Array.Empty<TResult>();
            using (ListPool<TResult> resultsList = ListPool<TResult>.Rent())
            {
                for (int index = 0; index < array.Length; index++)
                    if (filterMapFunc.Invoke(array[index], out TResult result))
                        resultsList.Add(result);
                return resultsList.ToArray();
            }
        }
    }
}
