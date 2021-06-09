using System;
using System.Collections;
using System.IO;
using BinarySerializer.Mapping.Types;

namespace BinarySerializer.Deserializers
{
    internal static class DictionaryDeserializer
    {
        internal static object Deserialize(DictionaryMap dictionaryMap, byte[] buffer, Stream stream)
        {
            int count = ByteStream.ReadBytes<int>(buffer, stream);

            IDictionary dictionary = (IDictionary) Activator.CreateInstance(dictionaryMap.type);
            for (int index = 0; index < count; index++)
            {
                object key = dictionaryMap.keyMap is ValueMap ? 
                    _InternalDeserializer.DeserializeInternalMapped(dictionaryMap.keyMap, buffer, stream) : 
                    _InternalDeserializer.DeserializeInternal(buffer, stream);
                object value = dictionaryMap.valueMap is ValueMap ? 
                    _InternalDeserializer.DeserializeInternalMapped(dictionaryMap.valueMap, buffer, stream) : 
                    _InternalDeserializer.DeserializeInternal(buffer, stream);
                dictionary.Add(key, value);
            }
            return dictionary;
        }
    }
}
