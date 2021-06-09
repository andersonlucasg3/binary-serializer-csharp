#if !UNITY_2017_1_OR_NEWER
using System.Collections.Generic;
using System.Linq;
using BinarySerializer;

namespace BinarySerializationTests.Models
{
    public class GraphClass : SimpleTestClass
    {
        public string[] stringArray;
        public long[] longArray;

        public EmptyClass nullEmptyClass;
        public EmptyClass notNullEmptyClass;

        public List<string> testList;
        
        public SimpleTestClass simpleTestClass { get; set; }
        [DoNotSerialize] public string notSerializedValue;
        
        public GraphClass(string[] stringArray, long[] longArray, string stringValue, int intValue, byte[] bytes, float floatProperty) 
            : base(stringValue, intValue, bytes, floatProperty)
        {
            this.stringArray = stringArray;
            this.longArray = longArray;
            nullEmptyClass = null;
            notNullEmptyClass = new EmptyClass();
            simpleTestClass = new SimpleTestClass(stringValue, intValue, bytes, floatProperty);
            notSerializedValue = "Should be null";

            testList = new List<string>(stringArray);
        }

        public bool Equals(GraphClass other)
        {
            return base.Equals(other) && stringArray.SequenceEqual(other.stringArray) && 
                   longArray.SequenceEqual(other.longArray) && Equals(nullEmptyClass, other.nullEmptyClass) && 
                   Equals(notNullEmptyClass, other.notNullEmptyClass) && 
                   Equals(simpleTestClass, other.simpleTestClass) &&
                   testList.SequenceEqual(other.testList);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((GraphClass) obj);
        }
    }
}
#endif