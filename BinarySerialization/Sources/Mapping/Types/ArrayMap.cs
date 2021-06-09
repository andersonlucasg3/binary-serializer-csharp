using System;
using JetBrains.Annotations;

namespace BinarySerializer.Mapping.Types
{
    public readonly struct ArrayMap : IEnumerableMap
    {
        [UsedImplicitly] public Type type { get; }
        [UsedImplicitly] public IMap elementMap { get; }

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
