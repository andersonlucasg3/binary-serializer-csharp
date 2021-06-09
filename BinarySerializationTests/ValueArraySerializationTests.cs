#if !UNITY_2017_1_OR_NEWER
using System.IO;
using BinarySerializer;
using NUnit.Framework;

namespace BinarySerializationTests
{
    public class ValueArraySerializationTests
    {
        [Test]
        public void TestArrayStringSerialization()
        {
            string[] stringArray = new string[10];
            for (int index = 0; index < 10; index++)
            {
                stringArray[index] = index == 0 ? null : $"string{index}";
            }

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(stringArray, ms);

                ms.Position = 0;

                string[] newStringArray = (string[]) Deserializer.Deserialize(ms);

                Assert.AreEqual(stringArray, newStringArray);
            }
        }

        [Test]
        public void TestArrayIntSerialization()
        {
            int[] intArray = new int[10];
            for (int index = 0; index < 10; index++) intArray[index] = index;

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(intArray, ms);
                
                ms.Position = 0;

                int[] newIntArray = (int[]) Deserializer.Deserialize(ms);
                
                Assert.AreEqual(intArray, newIntArray);
            }
        }
    }
}
#endif
