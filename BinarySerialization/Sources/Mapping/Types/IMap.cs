using System;

namespace BinarySerializer.Mapping.Types
{
    public interface IMap
    {
        Type type { get; }
    }

    public interface IEnumerableMap : IMap
    {
        IMap elementMap { get; }
    }
}
