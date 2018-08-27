using System;
using System.Web.Mvc;

namespace Common.Lib.MVC.Infastructure
{
    public static class MvcServiceLocator
    {
        public static T GetService<T>()
        {
            var factory = DependencyResolver.Current.GetService<T>();

            if (factory != null)
            {
                return factory;
            }

            throw new InvalidOperationException(string.Format(
                "No {0} has been registered in the {1}.",
                typeof(T).FullName,
                DependencyResolver.Current.GetType().FullName));
        }

        public static T GetInstance<T>(Type type)
        {
            var factory = DependencyResolver.Current.GetService(type);

            if (factory != null)
            {
                return (T)factory;
            }

            throw new InvalidOperationException(string.Format(
                "No {0} has been registered in the {1}.",
                typeof(T).FullName,
                DependencyResolver.Current.GetType().FullName));
        }
    }
}