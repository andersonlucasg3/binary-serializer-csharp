using System.IO;
using System.Runtime.Serialization;
using BinarySerializer.Mapping.Types;

namespace BinarySerializer.Deserializers
{
    internal static class ClassDeserializer
    {
        internal static object Deserialize(ClassMap classMap, byte[] buffer, Stream stream)
        {
            object instance = FormatterServices.GetUninitializedObject(classMap.type);
            for (int index = 0; index < classMap.fields.Length; index++)
            {
                ClassMap.FieldMap fieldMap = classMap.fields[index];
                if (fieldMap.map is ValueMap)
                {
                    object value = _InternalDeserializer.DeserializeInternalMapped(fieldMap.map, buffer, stream);
                    fieldMap.SetValue(instance, value);
                }
                else
                {
                    object value = _InternalDeserializer.DeserializeInternal(buffer, stream);
                    fieldMap.SetValue(instance, value);
                }
            }
            return instance;
        }
    }
}
