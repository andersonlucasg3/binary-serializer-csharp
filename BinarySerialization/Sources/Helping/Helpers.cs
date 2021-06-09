using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BinarySerializer.Pool;

namespace BinarySerializer.Helping
{
    internal static class Helpers
    {
        internal static readonly Type stringType = typeof(string);
        internal static readonly Type bytesType = typeof(byte[]);
        internal static readonly Type boolType = typeof(bool);
        internal static readonly Type byteType = typeof(byte);
        internal static readonly Type shortType = typeof(short);
        internal static readonly Type intType = typeof(int);
        internal static readonly Type longType = typeof(long);
        internal static readonly Type floatType = typeof(float);
        internal static readonly Type doubleType = typeof(double);
        internal static readonly Type listType = typeof(IList);
        internal static readonly Type dictionaryType = typeof(IDictionary);
        internal static readonly Type setType = typeof(ISet<>);
        internal static readonly Type runtimeType = typeof(Type);

        internal static readonly Type[] referenceValueTypes = {stringType, bytesType, runtimeType};

        internal static bool ImplementsGenericInterface(Type type, Type interfaceType)
        {
            IEnumerable<Type> enumerable = type.GetTypeInfo().ImplementedInterfaces;
            using (IEnumerator<Type> implementedInterfaces = enumerable.GetEnumerator())
            {
                while (implementedInterfaces.MoveNext())
                {
                    Type x = implementedInterfaces.Current;
                    if (x == null) continue;
                    bool isInterfaceItself = x == interfaceType;
                    bool isGenericInterface = x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == interfaceType; 
                    if (isInterfaceItself || isGenericInterface) return true;
                }
                return false;
            }
        }

        internal static byte[] RentBuffer() => ArrayPool<byte>.Rent(1024 * 128);
        internal static void PayBuffer(byte[] buffer) => ArrayPool<byte>.Pay(buffer);
    }
}
