using System;
using System.IO;

namespace BinarySerializer.Helping
{
    public static class TypeReadWrite
    {
        public static Type Read(byte[] buffer, Stream stream)
        {
            string typeName = ByteStream.ReadString(buffer, stream);
            return Type.GetType(typeName);
        }

        public static void Write(Type type, byte[] buffer, Stream stream)
        {
            string typeName = type.AssemblyQualifiedName;
            ByteStream.WriteBytes(typeName, buffer, stream);
        }
    }
}
