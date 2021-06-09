#if !UNITY_2017_1_OR_NEWER
using System.IO;
using BinarySerializer;
using NUnit.Framework;

namespace BinarySerializationTests
{
    public class ByteStreamTests
    {
        [Test]
        public void TestAllGetBytes()
        {
            byte[] buffer = new byte[1024 * 1024];
            using (MemoryStream ms = new MemoryStream(new byte[1024 * 1024]))
            {
                const string stringValue = "My test string";
                byte[] bytesValue = {10, 20, 30, 40, 50, 60, 70, 153, 0};

                const bool bool1 = true;
                const bool bool2 = false;
                const byte byte1 = 255;
                const byte byte2 = 10;
                const short short1 = 16000;
                const short short2 = -16000;
                const int int1 = 124091;
                const long long1 = 39082135908L;
                const float float1 = 13985.251F;
                const double double1 = 139061038.1309681396;

                ByteStream.WriteBytes(stringValue, buffer, ms);
                ByteStream.WriteBytes(bytesValue, buffer, ms);
                
                ByteStream.WriteBytes(bool1, buffer, ms);
                ByteStream.WriteBytes(bool2, buffer, ms);

                ByteStream.WriteBytes(byte1, buffer, ms);
                ByteStream.WriteBytes(byte2, buffer, ms);
                
                ByteStream.WriteBytes(short1, buffer, ms);
                ByteStream.WriteBytes(short2, buffer, ms);
                
                ByteStream.WriteBytes(int1, buffer, ms);
                ByteStream.WriteBytes(long1, buffer, ms);
                ByteStream.WriteBytes(float1, buffer, ms);
                ByteStream.WriteBytes(double1, buffer, ms);

                ms.Position = 0;

                string readString = ByteStream.ReadString(buffer, ms);
                byte[] readByteArray = ByteStream.ReadByteArray(buffer, ms);

                bool readBool1 = ByteStream.ReadBytes<bool>(buffer, ms);
                bool readBool2 = ByteStream.ReadBytes<bool>(buffer, ms);

                byte readByte1 = ByteStream.ReadBytes<byte>(buffer, ms);
                byte readByte2 = ByteStream.ReadBytes<byte>(buffer, ms);

                short readShort1 = ByteStream.ReadBytes<short>(buffer, ms);
                short readShort2 = ByteStream.ReadBytes<short>(buffer, ms);

                int readInt = ByteStream.ReadBytes<int>(buffer, ms);
                long readLong = ByteStream.ReadBytes<long>(buffer, ms);
                float readFloat = ByteStream.ReadBytes<float>(buffer, ms);
                double readDouble = ByteStream.ReadBytes<double>(buffer, ms);
                
                Assert.AreEqual(stringValue, readString);
                Assert.AreEqual(bytesValue, readByteArray);
                Assert.AreEqual(bool1, readBool1);
                Assert.AreEqual(bool2, readBool2);
                Assert.AreEqual(byte1, readByte1);
                Assert.AreEqual(byte2, readByte2);
                Assert.AreEqual(short1, readShort1);
                Assert.AreEqual(short2, readShort2);
                Assert.AreEqual(int1, readInt);
                Assert.AreEqual(long1, readLong);
                Assert.AreEqual(float1, readFloat);
                Assert.AreEqual(double1, readDouble);
            }
        }
    }
}
#endif
