using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Lib.Utility;
namespace Common.Lib.MVC.Helpers
{
    public static class ResourceHelper
    {
        public static Stream GetEmbeddedResource(string physicalPath, string resourceName)
        {
            try
            {
                //Assembly assembly = Assembly.LoadFile(@"C:\SourceCode\KnowYourNumbers\KnowYourNumbers\bin\Common.Lib.MVC.dll");
                //Assembly assembly = Assembly.LoadFrom(physicalPath);
                var assembly = PluginHelper.LoadPluginByPathName<Assembly>(physicalPath);

                if (assembly != null)
                {
                    string tempResourceName = assembly.GetManifestResourceNames().ToList().FirstOrDefault(f => f.EndsWith(resourceName));
                    if (tempResourceName == null)
                        return null;
                    return assembly.GetManifestResourceStream(tempResourceName);
                }
            }
            catch (Exception)
            {

            }

            return null;
        }
    }
}
