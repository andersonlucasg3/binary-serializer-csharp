using System.Collections;
using System.IO;
using BinarySerializer.Helping;
using BinarySerializer.Mapping.Types;

namespace BinarySerializer.Serializers
{
    public static class ListSerializer
    {
        public static void Serialize(IList list, ListMap map, byte[] buffer, Stream stream)
        {
            if (list == null)
            {
                ByteStream.WriteBytes(true, buffer, stream);
                return;
            }
            ByteStream.WriteBytes(false, buffer, stream);
            TypeReadWrite.Write(map.type, buffer, stream);

            ByteStream.WriteBytes(list.Count, buffer, stream);

            if (map.elementMap is ValueMap valueMap)
                for (int index = 0; index < list.Count; index++)
                    ValueSerializer.Serialize(list[index], valueMap, buffer, stream);
            else
                for (int index = 0; index < list.Count; index++)
                    _InternalSerializer.SerializeInternal(list[index], buffer, stream);
        }

        public static void Serialize(object set, SetMap map, byte[] buffer, Stream stream)
        {
            if (set == null)
            {
                ByteStream.WriteBytes(true, buffer, stream);
                return;
            }
            ByteStream.WriteBytes(false, buffer, stream);
            TypeReadWrite.Write(map.type, buffer, stream);
            
            SerializeDynamic((IEnumerable) set, map, buffer, stream);
        }

        private static void SerializeDynamic(IEnumerable set, SetMap setMap, byte[] buffer, Stream stream)
        {
            ByteStream.WriteBytes((int)setMap.countProperty.GetValue(set), buffer, stream);
            
            if (setMap.elementMap is ValueMap valueMap)
            {
                IEnumerator enumerator = set.GetEnumerator();
                while (enumerator.MoveNext()) ValueSerializer.Serialize(enumerator.Current, valueMap, buffer, stream);
            }
            else
            {
                IEnumerator enumerator = set.GetEnumerator();
                while (enumerator.MoveNext()) _InternalSerializer.SerializeInternal(enumerator.Current, buffer, stream);
            }
        }
    }
}
