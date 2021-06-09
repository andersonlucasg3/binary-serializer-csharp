using System;
using BinarySerializer.Extensions;
using BinarySerializer.Helping;

namespace BinarySerializer.Mapping.Types
{
    public readonly struct ListMap : IEnumerableMap
    {
        public Type type { get; }
        public IMap elementMap { get; }

        public ListMap(Type listType)
        {
            type = listType;
            if (!listType.GetInterfaces().Contains(Helpers.listType)) throw new ArgumentException($"Expected IList derived type is {listType}", nameof(listType));
            Type[] typeArguments = listType.GenericTypeArguments;
            elementMap = Mapper.Map(typeArguments[0], listType);
        }

        public override string ToString()
        {
            return $"{{ Type: {type}, ElementMap: {elementMap} }}";
        }
    }
}
