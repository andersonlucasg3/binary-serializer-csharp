using System;
using System.IO;
using BinarySerializer.Helping;
using BinarySerializer.Mapping.Types;

namespace BinarySerializer.Serializers
{
    public static class ValueSerializer
    {
        public static void Serialize<TValue>(TValue value, ValueMap map, byte[] buffer, Stream stream)
        {
            if (map.isNullable)
            {
                if (value == null)
                {
                    ByteStream.WriteBytes(true, buffer, stream);
                    return;
                }
                ByteStream.WriteBytes(false, buffer, stream);
            }

            switch (value)
            {
                case string stringValue: ByteStream.WriteBytes(stringValue, buffer, stream); break;
                case byte[] bytesValue: ByteStream.WriteBytes(bytesValue, buffer, stream); break;
                case bool boolValue: ByteStream.WriteBytes(boolValue, buffer, stream); break;
                case byte byteValue: ByteStream.WriteBytes(byteValue, buffer, stream); break;
                case short shortValue: ByteStream.WriteBytes(shortValue, buffer, stream); break;
                case int intValue: ByteStream.WriteBytes(intValue, buffer, stream); break;
                case long longValue: ByteStream.WriteBytes(longValue, buffer, stream); break;
                case float floatValue: ByteStream.WriteBytes(floatValue, buffer, stream); break;
                case double doubleValue: ByteStream.WriteBytes(doubleValue, buffer, stream); break;
                case Type runtimeType: TypeReadWrite.Write(runtimeType, buffer, stream); break;
                default: throw new ArgumentException($"Unexpected type ({typeof(TValue)}) when writing value: {value}, of type: {map.type}", nameof(value));
            }
        }
    }
}
