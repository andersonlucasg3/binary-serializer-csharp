using System;
using System.IO;
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
            unsafe
            {
                ToBytes(value, buffer, 0);
                Write(buffer, 0, sizeof(TValue), stream);
            }
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

        public static TValue ReadBytes<TValue>(byte[] buffer, Stream stream) where TValue : unmanaged
        {
            unsafe
            {
                Read(buffer, 0, sizeof(TValue), stream);
                return FromBytes<TValue>(buffer, 0);
            }
        }

        #endregion

        private static void Write(byte[] buffer, int index, int count, Stream stream) => stream.Write(buffer, index, count);
        private static void Read(byte[] buffer, int offset, int count, Stream stream)
        {
            int read = 0;
            do read += stream.Read(buffer, offset + read, count - read);
            while (read < count);
        }

        private static unsafe void ToBytes<TValue>(TValue value, byte[] buffer, int offset) where TValue : unmanaged
        {
            if (buffer.IsNull()) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0 || offset + sizeof(TValue) > buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            fixed (byte* ptrToStart = buffer) *(TValue*) (ptrToStart + offset) = value;
        }

        private static unsafe TValue FromBytes<TValue>(byte[] buffer, int offset) where TValue : unmanaged
        {
            if (buffer.IsNullOrEmpty()) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0 || offset + sizeof(TValue) > buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            fixed (byte* ptrToStart = buffer)
            {
                TValue * ptr = (TValue *) (ptrToStart + offset);
                return *ptr;
            }
        }
    }
}
