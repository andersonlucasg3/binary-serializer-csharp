using System;
using System.IO;
using BinarySerializer.Helping;
using BinarySerializer.Mapping.Types;

namespace BinarySerializer.Deserializers
{
    public static class ValueDeserializer
    {
        public static object Deserialize(ValueMap valueMap, byte[] buffer, Stream stream)
        {
            if (valueMap.isNullable && ByteStream.ReadBytes<bool>(buffer, stream)) return null;
            
            if (valueMap.type == Helpers.stringType) return ByteStream.ReadString(buffer, stream);
            if (valueMap.type == Helpers.bytesType) return ByteStream.ReadByteArray(buffer, stream);
            if (valueMap.type == Helpers.boolType) return ByteStream.ReadBytes<bool>(buffer, stream);
            if (valueMap.type == Helpers.byteType) return ByteStream.ReadBytes<byte>(buffer, stream);
            if (valueMap.type == Helpers.shortType) return ByteStream.ReadBytes<short>(buffer, stream);
            if (valueMap.type == Helpers.intType) return ByteStream.ReadBytes<int>(buffer, stream);
            if (valueMap.type == Helpers.longType) return ByteStream.ReadBytes<long>(buffer, stream);
            if (valueMap.type == Helpers.floatType) return ByteStream.ReadBytes<float>(buffer, stream);
            if (valueMap.type == Helpers.doubleType) return ByteStream.ReadBytes<double>(buffer, stream);
            if (valueMap.type == Helpers.runtimeType) return TypeReadWrite.Read(buffer, stream);
            throw new ArgumentException($"Unexpected type when reading value map: {valueMap}", nameof(valueMap));
        }
    }
}
