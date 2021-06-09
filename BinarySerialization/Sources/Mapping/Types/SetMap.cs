using System;
using System.Reflection;
using BinarySerializer.Helping;

namespace BinarySerializer.Mapping.Types
{
    public readonly struct SetMap : IEnumerableMap
    {
        public Type type { get; }
        public IMap elementMap { get; }
        public readonly PropertyInfo countProperty;

        public SetMap(Type type)
        {
            this.type = type;
            if (!Helpers.ImplementsGenericInterface(type, Helpers.setType)) 
                throw new ArgumentException($"Expected ISet<> derived type is {type}", nameof(type));
            Type[] genericArguments = type.GetGenericArguments();
            elementMap = Mapper.Map(genericArguments[0], type);

            countProperty = type.GetProperty("Count");
        }

        public override string ToString()
        {
            return $"{{ Type: {type}, ElementMap: {elementMap} }}";
        }
    }
}
