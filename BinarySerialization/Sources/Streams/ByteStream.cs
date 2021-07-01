using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using BinarySerializer.Extensions;

namespace BinarySerializer
{
    public static class ByteStream
    {
        #region Writing
        
        public static void WriteBytes(string value, byte[] buffer, Stream stream)
        {
            if (value == null)
            {
                WriteBytes(true, buffer, stream);
                return;
            }
            WriteBytes(false, buffer, stream);
            WriteBytes(value.Length, buffer, stream);
            Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, 0);
            Write(buffer, 0, value.Length, stream);
        }
        
        public static void WriteBytes(byte[] value, byte[] buffer, Stream stream)
        {
            if (value == null)
            {
                WriteBytes(true, buffer, stream);
                return;
            }
            WriteBytes(false, buffer, stream);
            WriteBytes(value.Length, buffer, stream);
            Write(value, 0, value.Length, stream);
        }
        
        public static void WriteBytes<TValue>(TValue value, byte[] buffer, Stream stream) where TValue : unmanaged
        {
            int size = ToBytes(value, buffer);
            Write(buffer, 0, size, stream);
        }
        
        #endregion
        
        #region Reading

        public static string ReadString(byte[] buffer, Stream stream)
        {
            if (ReadBytes<bool>(buffer, stream)) return null;
            int length = ReadBytes<int>(buffer, stream);
            Read(buffer, 0, length, stream); 
            return Encoding.UTF8.GetString(buffer, 0, length);
        }

        public static byte[] ReadByteArray(byte[] buffer, Stream stream)
        {
            if (ReadBytes<bool>(buffer, stream)) return null;
            int length = ReadBytes<int>(buffer, stream);
            byte[] newBuffer = new byte[length];
            Read(newBuffer, 0, length, stream);
            return newBuffer;
        }

        public static TValue ReadBytes<TValue>(byte[] buffer, Stream stream)
        {
            Read(buffer, 0, ByteConverter<TValue>.GetSize(), stream);
            return FromBytes<TValue>(buffer);
        }

        #endregion

        private static void Write(byte[] buffer, int index, int count, Stream stream) => stream.Write(buffer, index, count);
        private static void Read(byte[] buffer, int offset, int count, Stream stream)
        {
            int read = 0;
            do read += stream.Read(buffer, offset + read, count - read);
            while (read < count);
        }

        private static int ToBytes<TValue>(TValue value, byte[] buffer) where TValue : unmanaged
        {
            if (buffer.IsNull())
            {
                throw new ArgumentNullException(nameof(buffer));
            }
            
            // ReSharper disable once UseObjectOrCollectionInitializer, NotAccessedVariable
            ByteConverter<TValue> byteConverter = new ByteConverter<TValue>();
            byteConverter.buffer = buffer;
            byteConverter.value = value;
            return ByteConverter<TValue>.GetSize();
        }

        private static TValue FromBytes<TValue>(byte[] buffer)
        {
            if (buffer.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            // ReSharper disable once UseObjectOrCollectionInitializer
            ByteConverter<TValue> byteConverter = new ByteConverter<TValue>();
            byteConverter.buffer = buffer;
            return byteConverter.value;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct ByteConverter<TValue>
        {
            [FieldOffset(0)] public byte[] buffer;

            [FieldOffset(0)] public TValue value;

            public static int GetSize()
            {
                if (typeof(TValue) == typeof(bool))
                {
                    return sizeof(bool);
                }

                if (typeof(TValue) == typeof(byte))
                {
                    return sizeof(byte);
                }

                if (typeof(TValue) == typeof(short))
                {
                    return sizeof(short);
                }

                if (typeof(TValue) == typeof(int))
                {
                    return sizeof(int);
                }

                if (typeof(TValue) == typeof(long))
                {
                    return sizeof(long);
                }

                if (typeof(TValue) == typeof(float))
                {
                    return sizeof(float);
                }

                if (typeof(TValue) == typeof(double))
                {
                    return sizeof(double);
                }

                throw new ArgumentOutOfRangeException(nameof(TValue), typeof(TValue), "Unexpected type");
            }
        }
    }
}
