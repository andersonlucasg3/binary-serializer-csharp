#if !UNITY_2017_1_OR_NEWER
namespace BinarySerializationTests.Models
{
    public class EmptyClass
    {
        public bool Equals(EmptyClass other)
        {
            return other != null;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((EmptyClass) obj);
        }
    }
}
#endif