using System;
using System.Collections;
using System.IO;
using System.Reflection;
using BinarySerializer.Mapping.Types;
using BinarySerializer.Pool;

namespace BinarySerializer.Deserializers
{
    public static class ListDeserializer
    {
        public static object Deserialize(ListMap listMap, byte[] buffer, Stream stream)
        {
            int count = ByteStream.ReadBytes<int>(buffer, stream);

            IList list = (IList) Activator.CreateInstance(listMap.type);
            if (listMap.elementMap is ValueMap)
                for (int index = 0; index < count; index++)
                    list.Add(_InternalDeserializer.DeserializeInternalMapped(listMap.elementMap, buffer, stream));
            else
                for (int index = 0; index < count; index++)
                    list.Add(_InternalDeserializer.DeserializeInternal(buffer, stream));
            return list;
        }

        public static object Deserialize(SetMap setMap, byte[] buffer, Stream stream)
        {
            object instance = Activator.CreateInstance(setMap.type);
            DeserializeDynamic(instance, setMap, buffer, stream);
            return instance;
        }

        private static void DeserializeDynamic(object set, SetMap setMap, byte[] buffer, Stream stream)
        {
            int count = ByteStream.ReadBytes<int>(buffer, stream);
            MethodInfo methodInfo = setMap.type.GetMethod("Add");
            object[] objects = ArrayPool<object>.Rent(1);
            
            if (setMap.elementMap is ValueMap valueMap)
                for (int index = 0; index < count; index++)
                {
                    objects[0] = _InternalDeserializer.DeserializeInternalMapped(valueMap, buffer, stream);
                    methodInfo.Invoke(set, objects);
                }
            else
                for (int index = 0; index < count; index++)
                {
                    objects[0] = _InternalDeserializer.DeserializeInternal(buffer, stream);
                    methodInfo.Invoke(set, objects);
                }
            
            ArrayPool<object>.Pay(objects);
        }
    }
}
