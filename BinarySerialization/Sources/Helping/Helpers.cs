using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BinarySerializer.Pool;

namespace BinarySerializer.Helping
{
    public static class Helpers
    {
        public static readonly Type stringType = typeof(string);
        public static readonly Type bytesType = typeof(byte[]);
        public static readonly Type boolType = typeof(bool);
        public static readonly Type byteType = typeof(byte);
        public static readonly Type shortType = typeof(short);
        public static readonly Type intType = typeof(int);
        public static readonly Type longType = typeof(long);
        public static readonly Type floatType = typeof(float);
        public static readonly Type doubleType = typeof(double);
        public static readonly Type listType = typeof(IList);
        public static readonly Type dictionaryType = typeof(IDictionary);
        public static readonly Type setType = typeof(ISet<>);
        public static readonly Type runtimeType = typeof(Type);

        public static readonly Type[] referenceValueTypes = {stringType, bytesType, runtimeType};

        public static bool ImplementsGenericInterface(Type type, Type interfaceType)
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
        
        public static byte[] RentBuffer() => ArrayPool<byte>.Rent(1024 * 128);
        public static void PayBuffer(byte[] buffer) => ArrayPool<byte>.Pay(buffer);
    }
}
