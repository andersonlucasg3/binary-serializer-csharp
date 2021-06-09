using System;
using System.Reflection;
using BinarySerializer.Extensions;
using BinarySerializer.Pool;

namespace BinarySerializer.Mapping.Types
{
    public readonly struct ClassMap : IMap
    {
        private const BindingFlags FIELDS_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        
        private static readonly Predicate<FieldInfo> _filterFieldsFunc = FilterFieldsFunc;

        public Type type { get; }
        public readonly FieldMap[] fields;

        public ClassMap(Type type)
        {
            this.type = type;

            var isStruct = type.IsValueType && !type.IsPrimitive;
            
            if (!type.IsClass && !isStruct && !type.IsInterface) throw new ArgumentException("Expected class object", nameof(type));

            using (ListPool<FieldMap> fieldMaps = ListPool<FieldMap>.Rent())
            {
                Type current = type;
                do
                {
                    FieldInfo[] fieldInfoArray = current.GetFields(FIELDS_FLAGS).Filter(_filterFieldsFunc);
                    for (int index = 0; index < fieldInfoArray.Length; index++)
                    {
                        FieldMap map = new FieldMap(fieldInfoArray[index]);
                        if (!fieldMaps.Contains(map)) fieldMaps.Add(map);
                    }
                    current = current.BaseType;
                } while (current != null && current != typeof(object));

                fields = fieldMaps.ToArray();
            }
        }
        
        private static bool FilterFieldsFunc(FieldInfo info)
        {
            object[] customAttributes = info.GetCustomAttributes(true);
            return !customAttributes.Contains<object, DoNotSerializeAttribute>();
        }

        public override string ToString()
        {
            return $"{{ Type: {type} }}";
        }

        public readonly struct FieldMap
        {
            private readonly FieldInfo info;
            public readonly IMap map;

            public FieldMap(FieldInfo info)
            {
                this.info = info;
                map = Mapper.Map(info.FieldType, info.DeclaringType);
            }
            
            public object GetValue(object instance) => info.GetValue(instance);
            public void SetValue(object instance, object value) => info.SetValue(instance, value);

            private bool Equals(FieldMap other) => info.Name.Equals(other.info.Name) && info.FieldType == other.info.FieldType;

            public override bool Equals(object obj)
            {
                if (obj is FieldMap other) return Equals(other);
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((info != null ? info.GetHashCode() : 0) * 397) ^ (map != null ? map.GetHashCode() : 0);
                }
            }

            public override string ToString() => $"{{ Map: {map}, FieldInfo: {info} }}";
        }
    }
}
