#if !UNITY_2017_1_OR_NEWER
using System.Linq;

namespace BinarySerializationTests.Models
{
    public struct SimpleTestStruct
    {
        public string stringValue;
        public readonly int intValue;

        public byte[] bytes;

        public float floatProperty { get; private set; }

        public SimpleTestStruct(string stringValue, int intValue, byte[] bytes, float floatProperty)
        {
            this.stringValue = stringValue;
            this.intValue = intValue;
            this.bytes = bytes;
            this.floatProperty = floatProperty;
        }

        public bool Equals(SimpleTestStruct other)
        {
            return stringValue == other.stringValue && intValue == other.intValue && 
                   bytes.SequenceEqual(other.bytes) && floatProperty.Equals(other.floatProperty);
        }

        public override bool Equals(object obj)
        {
            return obj is SimpleTestStruct other && Equals(other);
        }
    }
}
#endif