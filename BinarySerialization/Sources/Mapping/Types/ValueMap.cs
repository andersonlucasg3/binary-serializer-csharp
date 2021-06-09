using System;
using BinarySerializer.Extensions;
using BinarySerializer.Helping;
using JetBrains.Annotations;

namespace BinarySerializer.Mapping.Types
{
    public readonly struct ValueMap : IMap
    {
        [UsedImplicitly] public Type ownerType { get; }
        [UsedImplicitly] public Type type { get; }
        [UsedImplicitly] public bool isNullable { get; }

        public ValueMap(Type type, Type ownerType)
        {
            this.type = type;
            this.ownerType = ownerType;
            isNullable = Helpers.referenceValueTypes.Contains(type);
        }

        public override string ToString() => $"{{ Type: {type}, OwnerType: {ownerType} }}";
    }
}
