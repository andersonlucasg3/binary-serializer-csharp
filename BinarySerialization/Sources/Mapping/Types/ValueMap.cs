using System;
using BinarySerializer.Extensions;
using BinarySerializer.Helping;

namespace BinarySerializer.Mapping.Types
{
    public readonly struct ValueMap : IMap
    {
        private Type ownerType { get; }
        public Type type { get; }
        public bool isNullable { get; }

        public ValueMap(Type type, Type ownerType)
        {
            this.type = type;
            this.ownerType = ownerType;
            isNullable = Helpers.referenceValueTypes.Contains(type);
        }

        public override string ToString() => $"{{ Type: {type}, OwnerType: {ownerType} }}";
    }
}
