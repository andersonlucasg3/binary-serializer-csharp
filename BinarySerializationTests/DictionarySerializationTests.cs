#if !UNITY_2017_1_OR_NEWER
using System.Collections.Generic;
using System.IO;
using BinarySerializationTests.Models;
using BinarySerializer;
using NUnit.Framework;

namespace BinarySerializationTests
{
    public class DictionarySerializationTests
    {
        [Test]
        public void TestPrimitiveDictionarySerialization()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>
            {
                {"str1", 10}, {"str2", 20}, {"str3", 30}
            };

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(dictionary, ms);

                ms.Position = 0;

                Dictionary<string, int> otherDictionary = (Dictionary<string, int>) Deserializer.Deserialize(ms);
                
                Assert.AreEqual(dictionary, otherDictionary);
            }
        }

        [Test]
        public void TestObjectDictionarySerialization()
        {
            Dictionary<string, GraphClass> dictionary = new Dictionary<string, GraphClass>()
            {
                {"str1", new GraphClass(new[] {"str1"}, new[] {10L}, "str2", 20, new byte[] {0, 255}, 315F)},
                {"str2", new GraphClass(new[] {"str2"}, new[] {39L}, "str3", 30, new byte[] {25, 222}, 32515F)}
            };

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(dictionary, ms);

                ms.Position = 0;

                Dictionary<string, GraphClass> otherDictionary = (Dictionary<string, GraphClass>) Deserializer.Deserialize(ms);

                Assert.AreEqual(dictionary, otherDictionary);
            }
        }
    }
}
#endif
