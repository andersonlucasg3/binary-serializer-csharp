using System;
using BinarySerializer.Pool;

namespace BinarySerializer.Extensions
{
    internal static class ArrayExt
    {
        private static bool IsEmpty(this Array array) => array.Length == 0;
        
        internal static bool IsNull(this Array array) => array == null;
        internal static bool IsNullOrEmpty(this Array array) => IsNull(array) || IsEmpty(array);

        internal static bool Contains<TArray, TCompare>(this TArray[] array) where TCompare : TArray
        {
            if (array.IsNullOrEmpty()) return false;
            for (int index = 0; index < array.Length; index++)
                if (array[index] is TCompare)
                    return true;
            return false;
        }

        internal static bool Contains<TArray>(this TArray[] array, TArray item)
        {
            if (array.IsNullOrEmpty()) return false;
            for (int index = 0; index < array.Length; index++)
                if (array[index].Equals(item))
                    return true;
            return false;
        }

        internal static TArray[] Filter<TArray>(this TArray[] array, Predicate<TArray> predicate)
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
    }
}
