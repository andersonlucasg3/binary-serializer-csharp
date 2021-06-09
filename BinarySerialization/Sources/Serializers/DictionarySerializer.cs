using System.Collections;
using System.IO;
using BinarySerializer.Helping;
using BinarySerializer.Mapping.Types;

namespace BinarySerializer.Serializers
{
    public static class DictionarySerializer
    {
        public static void Serialize(IDictionary dictionary, DictionaryMap map, byte[] buffer, Stream stream)
        {
            if (dictionary == null)
            {
                ByteStream.WriteBytes(true, buffer, stream);
                return;
            }
            ByteStream.WriteBytes(false, buffer, stream);
            TypeReadWrite.Write(map.type, buffer, stream);
            
            ByteStream.WriteBytes(dictionary.Count, buffer, stream);
            IEnumerator enumerator = dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DictionaryEntry entry = (DictionaryEntry) enumerator.Current;
                if (map.keyMap is ValueMap keyValueMap) ValueSerializer.Serialize(entry.Key, keyValueMap, buffer, stream);
                else _InternalSerializer.SerializeInternal(entry.Key, buffer, stream);
                if (map.valueMap is ValueMap valueValueMap) ValueSerializer.Serialize(entry.Value, valueValueMap, buffer, stream);
                else _InternalSerializer.SerializeInternal(entry.Value, buffer, stream);
            }
        }
    }
}
