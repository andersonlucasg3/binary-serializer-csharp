using System;
using System.IO;
using BinarySerializer.Extensions;
using BinarySerializer.Helping;
using BinarySerializer.Mapping.Types;

namespace BinarySerializer.Serializers
{
    public static class ArraySerializer
    {
        public static void Serialize(Array array, ArrayMap map, byte[] buffer, Stream stream)
        {
            if (array.IsNull())
            {
                ByteStream.WriteBytes(true, buffer, stream);
                return;
            }
            ByteStream.WriteBytes(false, buffer, stream);
            TypeReadWrite.Write(map.type, buffer, stream);

            ByteStream.WriteBytes(array.Length, buffer, stream);

            if (map.elementMap is ValueMap valueMap)
                for (int index = 0; index < array.Length; index++)
                    ValueSerializer.Serialize(array.GetValue(index), valueMap, buffer, stream);
            else
                for (int index = 0; index < array.Length; index++)
                    _InternalSerializer.SerializeInternal(array.GetValue(index), buffer, stream);
        }
    }
}
