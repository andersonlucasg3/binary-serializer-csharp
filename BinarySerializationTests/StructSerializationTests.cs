#if !UNITY_2017_1_OR_NEWER
using System.IO;
using BinarySerializationTests.Models;
using BinarySerializer;
using NUnit.Framework;

namespace BinarySerializationTests
{
    public class StructSerializationTests
    {
        [Test]
        public void TestStructSerialization()
        {
            EmptyStruct instance = new EmptyStruct();

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(instance, ms);

                ms.Position = 0;

                object other = Deserializer.Deserialize(ms);
                
                Assert.AreEqual(instance, other);
            }
        }
        
        [Test]
        public void TestStructWithPropertiesAndFields()
        {
            SimpleTestStruct simpleObject = new SimpleTestStruct("Test string", 135135, new byte[] {10, 50, 255, 44}, 193581.44F);

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(simpleObject, ms);

                ms.Position = 0;

                SimpleTestStruct otherObject = (SimpleTestStruct) Deserializer.Deserialize(ms);

                Assert.AreEqual(simpleObject, otherObject);
            }
        }
    }
}
#endif
