#if !UNITY_2017_1_OR_NEWER
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySerializationTests.Models
{
    public interface ITestInterface { }

    public enum TestEnum
    {
        value1,
        value2
    }
    
    public class SimpleTestClass : ITestInterface
    {
        public string stringValue;
        public readonly int intValue;

        public byte[] bytes;

        public float floatProperty { get; private set; }
        
        public TestEnum value1;
        public TestEnum value2;

        public Dictionary<TestEnum, string> testDictionary = new Dictionary<TestEnum, string>();
        public HashSet<int> testHashSet = new HashSet<int>();

        public Type serializedType;

        public SimpleTestClass(string stringValue, int intValue, byte[] bytes, float floatProperty)
        {
            this.stringValue = stringValue;
            this.intValue = intValue;
            this.bytes = bytes;
            this.floatProperty = floatProperty;
            value1 = TestEnum.value1;
            value2 = TestEnum.value2;
            testDictionary[value1] = "value1";
            testDictionary[value2] = "value2";

            testHashSet.Add(30);
            testHashSet.Add(20);
            testHashSet.Add(310595);

            serializedType = typeof(string);
        }

        public bool Equals(SimpleTestClass other)
        {
            return stringValue == other.stringValue && intValue == other.intValue && 
                   bytes.SequenceEqual(other.bytes) && floatProperty.Equals(other.floatProperty) &&
                   value1.Equals(other.value1) && value2.Equals(other.value2) &&
                   testDictionary.SequenceEqual(other.testDictionary) &&
                   testHashSet.SequenceEqual(other.testHashSet) &&
                   serializedType == other.serializedType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((SimpleTestClass) obj);
        }
    }
}
#endif
