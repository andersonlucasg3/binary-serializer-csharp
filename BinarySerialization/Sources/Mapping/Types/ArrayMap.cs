using System;

namespace BinarySerializer.Mapping.Types
{
    internal readonly struct ArrayMap : IEnumerableMap
    {
        public Type type { get; }
        public IMap elementMap { get; }

        public ArrayMap(Type arrayType)
        {
            type = arrayType;
            if (!arrayType.IsArray) throw new ArgumentException("Expected Array<>", nameof(arrayType));
            elementMap = Mapper.Map(arrayType.GetElementType(), arrayType);
        }

        public override string ToString()
        {
            return $"{{ Type: {type}, ElementMap: {elementMap} }}";
        }
    }
}
