using System;
using BinarySerializer.Extensions;
using BinarySerializer.Helping;

namespace BinarySerializer.Mapping.Types
{
    public readonly struct DictionaryMap : IMap
    {
        public Type type { get; }
        public readonly IMap keyMap;
        public readonly IMap valueMap;

        public DictionaryMap(Type type)
        {
            this.type = type;
            if (!type.GetInterfaces().Contains(Helpers.dictionaryType)) 
                throw new ArgumentException($"Expected IDictionary derived type is {type}", nameof(type));
            Type[] typeArguments = type.GenericTypeArguments;
            keyMap = Mapper.Map(typeArguments[0], type);
            valueMap = Mapper.Map(typeArguments[1], type);
        }

        public override string ToString()
        {
            return $"{{ Type: {type}, KeyMap: {keyMap}, ValueMap: {valueMap} }}";
        }
    }
}
