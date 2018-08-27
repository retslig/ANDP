using System;
using System.Web.Compilation;
using System.Web.Mvc;
using Common.Lib.Domain.Common.Interfaces;
using Common.Lib.MVC.Providers.Resource;

namespace Common.Lib.MVC.Providers.Resource
{
    public class DatabaseResourceProviderFactory : ResourceProviderFactory
    {
        /// <summary>
        /// When overridden in a derived class, creates a global resource provider.
        /// //Dont forget to add the web.Config entry dummy.
        /// <globalization resourceProviderFactoryType="Common.Lib.UI.MVC.Providers.Resource.DatabaseResourceProviderFactory, Common.Lib.UI.MVC" enableClientBasedCulture="true" uiCulture="auto" culture="auto" />
        /// <globalization culture="en-US" uiCulture="en-US" resourceProviderFactoryType="Common.Lib.UI.MVC.Providers.Resource.DatabaseResourceProviderFactory, Common.Lib.UI.MVC" />
        /// 
        /// //Dont forget to add RegisterTypes to your MVC project bootstrapper file for Dependency injection. So the locater below will find the the dependency.
        /// //Build Connection String and dependency for Language Resource Provider and other Language Resource Service calls..
        /// EntityConnectionStringBuilder CommonCommonEntityBuilder = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings["Common_Common_Entities"].ConnectionString);
        /// sqlBuilder = new SqlConnectionStringBuilder(CommonCommonEntityBuilder.ProviderConnectionString);
        /// sqlBuilder.ApplicationName += " - " + Assembly.GetExecutingAssembly().GetName().Name;
        /// CommonCommonEntityBuilder.ProviderConnectionString = sqlBuilder.ConnectionString;
        /// 
        /// container.RegisterType<ICommon_Common_Entities, Common_Common_Entities>(new InjectionConstructor(CommonCommonEntityBuilder.ConnectionString));
        /// container.RegisterType<ILanguageResourceRepository, LanguageResourceRepository>(new ExternallyControlledLifetimeManager(), new InjectionConstructor(container.Resolve<Common_Common_Entities>()));
        /// container.RegisterType<ILanguageResourceService, LanguageResourceService>(new ExternallyControlledLifetimeManager(), new InjectionConstructor(container.Resolve<ILanguageResourceRepository>(), "en-US"));
        /// </summary>
        /// <param name="classKey">The name of the resource class.</param>
        /// <returns>
        /// A global resource provider.
        /// </returns>
        public override IResourceProvider CreateGlobalResourceProvider(string classKey)
        {
            return new DatabaseResourceProvider(GetLanguageResourceService(), classKey) as IResourceProvider;
        }

        /// <summary>
        /// When overridden in a derived class, creates a local resource provider.
        /// </summary>
        /// <param name="virtualPath">The path to a resource file.</param>
        /// <returns>
        /// A local resource provider.
        /// </returns>
        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {

            // we should always get a path from the runtime
            string classKey = virtualPath;
            if (!string.IsNullOrEmpty(virtualPath))
            {
                //virtualPath = virtualPath.Remove(0, 1); // don't need it in ASP.NET MVC
                //Commented this out because it was ripping off the "Home/" off of the "Home/Index" for example...
                //classKey = virtualPath.Remove(0, virtualPath.IndexOf('/') + 1);
            }

            return new DatabaseResourceProvider(GetLanguageResourceService(), classKey) as IResourceProvider;
        }

        private static ILanguageResourceService GetLanguageResourceService()
        {
            var factory = DependencyResolver.Current.GetService<ILanguageResourceService>();

            if (factory != null)
            {
                return factory;
            }

            throw new InvalidOperationException(string.Format(
                "No {0} has been registered in the {1}.",
                typeof(ILanguageResourceService).FullName,
                DependencyResolver.Current.GetType().FullName));
        }
    }
}
