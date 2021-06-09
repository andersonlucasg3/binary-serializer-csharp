#if !UNITY_2017_1_OR_NEWER
using System;
using System.Collections.Generic;
using System.IO;
using BinarySerializationTests.Models;
using BinarySerializer;

namespace ConsoleApplication
{
    internal static class Program
    {
        public static void Main()
        {
            List<GraphClass> classes = new List<GraphClass>();
            for (int index = 0; index < 10000; index++)
                classes.Add(new GraphClass(new[] {"str1"},
                    new[] {20010L},
                    "str2", 103,
                    new byte[] {193, 59},
                    23958533F));

            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(classes, ms);
                
                ms.Position = 0;

                List<GraphClass> newClasses = (List<GraphClass>) Deserializer.Deserialize(ms);
                
                Console.WriteLine(newClasses.Count);
            }
        }
    }
}
#endif
