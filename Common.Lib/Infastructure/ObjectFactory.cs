using System;
using System.Reflection;
using Common.Lib.Mapping;

namespace Common.Lib.Infastructure
{
    public static class ObjectFactory
    {
        public static TDestination CreateInstanceAndMapAndValidate<TSource, TDestination>(ICommonMapper mapper, TSource source)
        {
            //Complete mapping
            TDestination result = mapper.Map<TSource, TDestination>(source);

            Type destinationType = result.GetType();
            MethodInfo methodInfo = destinationType.GetMethod("Validate");

            //add validation logic here.
            if (methodInfo != null)
            {
                object isValid = methodInfo.Invoke(result, null);
            }

            return result;
        }

        public static object CreateInstanceAndMapAndValidate(ICommonMapper mapper, Type sourceType, Type destinationType, object source)
        {
            //Complete mapping
            var result = mapper.Map(source, sourceType, destinationType);
            MethodInfo methodInfo = destinationType.GetMethod("Validate");

            //add validation logic here.
            if (methodInfo != null)
            {
                object isValid = methodInfo.Invoke(result, null);
            }

            return result;
        }

        public static TDestination CreateInstanceAndMap<TSource, TDestination>(ICommonMapper mapper, TSource source)
        {
            //Complete mapping
            return mapper.Map<TSource, TDestination>(source);
        }


        public static object CreateInstanceAndMap(ICommonMapper mapper, Type sourceType, Type destinationType, object source)
        {
            //Complete mapping
            return mapper.Map(source, sourceType, destinationType);
        }

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);
            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
            return (T)constructor.Invoke(new object[] { });
        }
    }
}
