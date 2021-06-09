using System;
using BinarySerializer.Caching;
using BinarySerializer.Extensions;
using BinarySerializer.Helping;
using BinarySerializer.Mapping.Types;

namespace BinarySerializer.Mapping
{
    public static class Mapper
    {
        public static IMap Map(Type type, Type ownerType)
        {
            if (type.IsArray) return GetMap<ArrayMap>(type, ownerType);

            if (Helpers.ImplementsGenericInterface(type, Helpers.setType)) return GetMap<SetMap>(type, ownerType);

            if (type.GetInterfaces().Contains(Helpers.listType)) return GetMap<ListMap>(type, ownerType);

            if (type.GetInterfaces().Contains(Helpers.dictionaryType)) return GetMap<DictionaryMap>(type, ownerType);

            if (type.IsValueType && type.IsPrimitive || Helpers.referenceValueTypes.Contains(type)) return GetMap<ValueMap>(type, ownerType);

            if (!type.IsClass && !type.IsValueType && !type.IsInterface) throw new ArgumentException($"Type not expected {type}", nameof(type));

            return GetMap<ClassMap>(type, ownerType);
        }

        private static IMap GetMap<TMap>(Type type, Type ownerType) where TMap : IMap
        {
            Type typeOfMap = typeof(TMap);
            if (MapCache<IMap>.TryGetCache(type, out IMap map)) return map;
            map = NewMap(typeOfMap, type, ownerType);
            MapCache<IMap>.AddCache(type, map);
            return map;
        }

        private static IMap NewMap(Type typeOfMap, Type type, Type ownerType)
        {
            if (typeOfMap == typeof(ValueMap)) return new ValueMap(type, ownerType);
            if (typeOfMap == typeof(ArrayMap)) return new ArrayMap(type);
            if (typeOfMap == typeof(ListMap)) return new ListMap(type);
            if (typeOfMap == typeof(SetMap)) return new SetMap(type);
            if (typeOfMap == typeof(ClassMap)) return new ClassMap(type);
            if (typeOfMap == typeof(DictionaryMap)) return new DictionaryMap(type);
            throw new ArgumentException($"Unexpected type ({type}) when mapping types. Type of map is: {typeOfMap}. Owner type is: {ownerType}.", nameof(typeOfMap));
        }
    }
}
