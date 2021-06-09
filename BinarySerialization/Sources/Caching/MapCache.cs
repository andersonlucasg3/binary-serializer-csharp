using System;
using System.Collections.Generic;
using BinarySerializer.Mapping.Types;

namespace BinarySerializer.Caching
{
    internal static class MapCache<TMap> where TMap : IMap
    {
        private static readonly Dictionary<Type, TMap> _maps = new Dictionary<Type, TMap>();

        public static void AddCache(Type classType, TMap map) => _maps[classType] = map;
        
        public static bool TryGetCache(Type classType, out TMap map) => _maps.TryGetValue(classType, out map);
    }
}
