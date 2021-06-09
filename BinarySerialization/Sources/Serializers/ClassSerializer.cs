using System.IO;
using BinarySerializer.Helping;
using BinarySerializer.Mapping.Types;

namespace BinarySerializer.Serializers
{
    public static class ClassSerializer
    {
        public static void Serialize(object instance, ClassMap map, byte[] buffer, Stream stream)
        {
            if (instance == null)
            {
                ByteStream.WriteBytes(true, buffer, stream);
                return;
            }
            ByteStream.WriteBytes(false, buffer, stream);
            TypeReadWrite.Write(map.type, buffer, stream);

            for (int index = 0; index < map.fields.Length; index++)
            {
                ClassMap.FieldMap fieldMap = map.fields[index];
                object value = fieldMap.GetValue(instance);
                if (fieldMap.map is ValueMap valueMap) ValueSerializer.Serialize(value, valueMap, buffer, stream);
                else _InternalSerializer.SerializeInternal(value, buffer, stream);
            }
        }
    }
}
