#if !UNITY_2017_1_OR_NEWER
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BinarySerializationTests.Models;
using BinarySerializer;
using NUnit.Framework;

namespace BinarySerializationTests
{
    public class ListSerializationTests
    {
        [Test]
        public void TestListSerialization()
        {
            List<int> intList = new List<int> {10, 20, 30, 40, 50, 12401, 01405, 0139493};
            List<string> stringList = new List<string> {"str1", "str2", "str3", "str4"};

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(intList, ms);

                ms.Position = 0;

                List<int> otherIntList = (List<int>) Deserializer.Deserialize(ms);
                
                Assert.AreEqual(intList, otherIntList);

                ms.Position = 0;
                
                Serializer.Serialize(stringList, ms);

                ms.Position = 0;

                List<string> otherStringList = (List<string>) Deserializer.Deserialize(ms);
                
                Assert.AreEqual(stringList, otherStringList);
            }
        }

        [Test]
        public void TestListOfObjectsSerialization()
        {
            List<GraphClass> classes = new List<GraphClass>()
            {
                new GraphClass(new[] {"str1"},
                    new[] {20010L},
                    "str2", 103,
                    new byte[] {193, 59},
                    23958533F),
                new GraphClass(new[] {"str2"},
                    new[] {20010L},
                    "str3", 103,
                    new byte[] {193, 59},
                    23958533F)
            };

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(classes, ms);

                ms.Position = 0;

                List<GraphClass> otherClasses = (List<GraphClass>) Deserializer.Deserialize(ms);
                
                Assert.AreEqual(classes, otherClasses);
            }
        }

        [Test]
        public void TestInterfaceListSerialization()
        {
            List<ITestInterface> interfaceList = new List<ITestInterface>()
            {
                new SimpleTestClass("str1", 253, new byte[] {3, 35, 5, 6}, 23315F),
                new GraphClass(new[] {"str1"}, new[] {19L}, "str2", 135, new byte[] {135, 35}, 135135.4F)
            };

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(interfaceList, ms);

                ms.Position = 0;

                List<ITestInterface> otherInterfaceList = (List<ITestInterface>) Deserializer.Deserialize(ms);
                
                Assert.AreEqual(interfaceList, otherInterfaceList);
            }
        }

        [Test]
        public void TestBigListSerialization()
        {
            Stopwatch stopwatch = new Stopwatch();
            List<GraphClass> classes = new List<GraphClass>();
            for (int index = 0; index < 10000; index++)
                classes.Add(new GraphClass(new[] {"str1"},
                    new[] {20010L},
                    "str2", 103,
                    new byte[] {193, 59},
                    23958533F));

            using (MemoryStream ms = new MemoryStream())
            {
                stopwatch.Start();
                Serializer.Serialize(classes, ms);
                stopwatch.Stop();
                
                Console.WriteLine($"Took {stopwatch.ElapsedMilliseconds} ms to serialize");

                ms.Position = 0;

                stopwatch.Restart();
                List<GraphClass> newClasses = (List<GraphClass>) Deserializer.Deserialize(ms);
                stopwatch.Stop();
                
                Console.WriteLine($"Took {stopwatch.ElapsedMilliseconds} ms to deserialize");
                
                Assert.AreEqual(classes, newClasses);
            }
        }
    }
}
#endif
