using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using BinarySerializer.Helping;
using BinarySerializer.Mapping;
using BinarySerializer.Mapping.Types;
using BinarySerializer.Serializers;

namespace BinarySerializer
{
    public static class Serializer
    {
        public static void Serialize(object obj, Stream stream)
        {
            byte[] buffer = Helpers.RentBuffer();
            _InternalSerializer.SerializeInternal(obj, buffer, stream);
            Helpers.PayBuffer(buffer);
        }
        
        public static async Task SerializeAsync(object obj, Stream stream)
        {
            Serialize(obj, stream);
            await Task.Yield();
        }
    }

    internal static class _InternalSerializer
    {
        internal static void SerializeInternal(object obj, byte[] buffer, Stream stream)
        {
            if (obj == null) ByteStream.WriteBytes(true, buffer, stream);
            else
            {
                Type objType = obj.GetType();
                IMap map = Mapper.Map(objType, null);

                switch (map)
                {
                    case ArrayMap arrayMap:
                        ArraySerializer.Serialize((Array) obj, arrayMap, buffer, stream);
                        break;
                    case ClassMap classMap:
                        ClassSerializer.Serialize(obj, classMap, buffer, stream);
                        break;
                    case DictionaryMap dictionaryMap:
                        DictionarySerializer.Serialize((IDictionary) obj, dictionaryMap, buffer, stream);
                        break;
                    case ListMap listMap:
                        ListSerializer.Serialize((IList) obj, listMap, buffer, stream);
                        break;
                    case ValueMap valueMap:
                        ValueSerializer.Serialize(obj, valueMap, buffer, stream);
                        break;
                    case SetMap setMap:
                        ListSerializer.Serialize(obj, setMap, buffer, stream);
                        break;
                    default: throw new ArgumentException($"Unexpected map type: {map}, for object: {obj}", nameof(map));
                }
            }
        }
    }
}
