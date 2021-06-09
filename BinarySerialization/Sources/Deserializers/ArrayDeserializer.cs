using System;
using System.IO;
using BinarySerializer.Mapping.Types;

namespace BinarySerializer.Deserializers
{
    public static class ArrayDeserializer
    {
        public static Array Deserialize(ArrayMap map, byte[] buffer, Stream stream)
        {
            int length = ByteStream.ReadBytes<int>(buffer, stream);
            
            if (map.elementMap is ValueMap valueMap)
            {
                Array array = Array.CreateInstance(valueMap.type, length);
                for (int index = 0; index < length; index++) array.SetValue(ValueDeserializer.Deserialize(valueMap, buffer, stream), index);
                return array;    
            }
            else
            {
                Array array = Array.CreateInstance(map.elementMap.type, length);
                for (int index = 0; index < array.Length; index++) array.SetValue(_InternalDeserializer.DeserializeInternal(buffer, stream), index);
                return array;
            }
        }
    }
}
