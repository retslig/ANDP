using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;

namespace Common.Lib.Utility
{
    public static class PluginHelper
    {
        public static T LoadPluginByPathName<T>(string pathName)
        {
            string viewType = typeof(T).GUID.ToString();

            if (MemoryCache.Default[viewType] != null)
                return MemoryCache.Default[viewType] is T ? (T)MemoryCache.Default[viewType] : default(T);

            object plugin = Assembly.LoadFrom(pathName);
            if (plugin != null)
            {
                //Cache this object as we want to only load this assembly into memory once.
                MemoryCache.Default.Set(viewType, plugin, new CacheItemPolicy() { Priority = CacheItemPriority.Default });
                return (T)plugin;
            }
            
            return default(T);
        }

        public static T LoadPluginsOfType<T>()
        {
            string viewType = typeof (T).GUID.ToString();

            if (MemoryCache.Default[viewType] != null)
                return MemoryCache.Default[viewType] is T ? (T)MemoryCache.Default[viewType] : default(T);

            foreach (string name in GetPluginNames())
            {
                var plugin = LoadOnlyType<T>(name);
                if (plugin != null)
                {
                    //Cache this object as we want to only load this assembly into memory once.
                    MemoryCache.Default.Set(viewType, plugin, new CacheItemPolicy() { Priority = CacheItemPriority.Default });
                    return plugin;
                }
            }

            return default(T);
        }

        private static IEnumerable<string> GetPluginNames()
        {
            return Directory.GetFiles("Plugins", "*.dll");
        }

        private static T LoadOnlyType<T>(string assemblyFileName)
        {
            object pluginFound = null;
            Type iPluginType = typeof (T);
            Assembly assembly = Assembly.LoadFrom(assemblyFileName);

            if (assembly != null)
            {
                Type[] types = assembly.GetExportedTypes();
                pluginFound =(from t in types where iPluginType.IsAssignableFrom(t) select Activator.CreateInstance(t)).OfType<T>().FirstOrDefault();
            }

            return (T)pluginFound;
        }
    }
}

