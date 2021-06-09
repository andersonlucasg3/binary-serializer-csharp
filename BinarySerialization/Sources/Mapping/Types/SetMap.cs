using System;
using System.Reflection;
using BinarySerializer.Helping;
using JetBrains.Annotations;

namespace BinarySerializer.Mapping.Types
{
    public readonly struct SetMap : IEnumerableMap
    {
        [UsedImplicitly] public Type type { get; }
        [UsedImplicitly] public IMap elementMap { get; }
        [UsedImplicitly] public readonly PropertyInfo countProperty;
        [UsedImplicitly] public readonly MethodInfo addMethod;

        public SetMap(Type type)
        {
            this.type = type;
            if (!Helpers.ImplementsGenericInterface(type, Helpers.setType)) 
                throw new ArgumentException($"Expected ISet<> derived type is {type}", nameof(type));
            Type[] genericArguments = type.GetGenericArguments();
            elementMap = Mapper.Map(genericArguments[0], type);

            countProperty = type.GetProperty("Count");
            addMethod = type.GetMethod("Add");
        }

        public override string ToString()
        {
            return $"{{ Type: {type}, ElementMap: {elementMap} }}";
        }
    }
}
