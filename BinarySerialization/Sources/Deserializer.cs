using System;
using System.IO;
using BinarySerializer.Deserializers;
using BinarySerializer.Helping;
using BinarySerializer.Mapping;
using BinarySerializer.Mapping.Types;

namespace BinarySerializer
{
    public static class Deserializer
    {
        public static object Deserialize(Stream stream)
        {
            if (stream.Length == 0) return default;
            byte[] buffer = Helpers.RentBuffer();
            object instance = _InternalDeserializer.DeserializeInternal(buffer, stream);
            Helpers.PayBuffer(buffer);
            return instance;
        }
    }

    internal static class _InternalDeserializer
    {
        internal static object DeserializeInternal(byte[] buffer, Stream stream)
        {
            bool isNull = ByteStream.ReadBytes<bool>(buffer, stream);
            if (isNull) return null;

            Type type = TypeReadWrite.Read(buffer, stream);
            IMap map = Mapper.Map(type, null);
            return DeserializeInternalMapped(map, buffer, stream);
        }

        internal static object DeserializeInternalMapped(IMap map, byte[] buffer, Stream stream)
        {
            switch (map)
            {
                case ArrayMap arrayMap: return ArrayDeserializer.Deserialize(arrayMap, buffer, stream);
                case ClassMap classMap: return ClassDeserializer.Deserialize(classMap, buffer, stream);
                case DictionaryMap dictionaryMap: return DictionaryDeserializer.Deserialize(dictionaryMap, buffer, stream);
                case ListMap listMap: return ListDeserializer.Deserialize(listMap, buffer, stream);
                case SetMap setMap: return ListDeserializer.Deserialize(setMap, buffer, stream);
                case ValueMap valueMap: return ValueDeserializer.Deserialize(valueMap, buffer, stream);
                default: throw new ArgumentException($"Unexpected map type: {map}", nameof(map));
            }
        }
    }
}
