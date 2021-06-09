#if !UNITY_2017_1_OR_NEWER
using System.IO;
using BinarySerializationTests.Models;
using BinarySerializer;
using NUnit.Framework;

namespace BinarySerializationTests
{
    public class ClassSerializationTests
    {
        [Test]
        public void TestEmptyClassSerialization()
        {
            EmptyClass instance = new EmptyClass();

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(instance, ms);

                ms.Position = 0;

                object other = Deserializer.Deserialize(ms);
                
                Assert.AreEqual(instance, other);

                ms.Position = 0;
                
                Serializer.Serialize(null, ms);

                ms.Position = 0;

                other = Deserializer.Deserialize(ms);
                
                Assert.AreEqual(null, other);
            }
        }

        [Test]
        public void TestClassWithPropertiesAndFields()
        {
            SimpleTestClass simpleObject = new SimpleTestClass("Test string", 135135, new byte[] {10, 50, 255, 44}, 193581.44F);

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(simpleObject, ms);

                ms.Position = 0;

                SimpleTestClass otherObject = (SimpleTestClass) Deserializer.Deserialize(ms);
                
                Assert.AreEqual(simpleObject, otherObject);
            }
        }

        [Test]
        public void TestComplexGraphClass()
        {
            GraphClass graphClass = new GraphClass(
                new[] {"string1", "string2", "string3"},
                new[] {50L, 30L, 10351053100L},
                "Some string", 2104104,
                new byte[] {50, 30, 110, 54},
                1293815.0F
            );

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(graphClass, ms);

                ms.Position = 0;

                GraphClass otherClass = (GraphClass) Deserializer.Deserialize(ms);
                
                Assert.AreEqual(graphClass, otherClass);
            }
        }
    }
}
#endif
