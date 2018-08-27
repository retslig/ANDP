using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Common.Lib.Common.Enums;
using Common.Lib.Data.Repositories.Common;
using Common.Lib.Interfaces;
using Common.Lib.Mapping;
using Common.Lib.Security;
using Common.Lib.Services;
using Microsoft.Practices.Unity;

namespace ANDP.Provisioning.API.Rest.Infrastructure
{
    public static class BootStrapper
    {
        private static readonly IUnityContainer Container = new UnityContainer();

        public static IUnityContainer Initialize()
        {
            BuildUnityContainer();
            return Container;
        }

        private static void BuildUnityContainer()
        {
            bool allow = false;
            //See if we want to accept self signed certificates.
            if (bool.TryParse(ConfigurationManager.AppSettings["AllowSelfSignedCertificateForOrderApi"], out allow))
            {
                Common.Lib.Security.CertificateHelper.AllowSelfSignedCertificates = allow;
            }

            RegisterCommonMapper();

            //Authentication Settings for Token support
            RegisterAuthSettings();

            //Services
            Container.RegisterType<ILogger, NLogWriterService>(new HierarchicalLifetimeManager(), new InjectionConstructor(true, SerializerType.Json));
        }

        private static void RegisterCommonMapper()
        {
            //Inject CommonMapper the Auto Mapping Engine.
            var commonMapperProfileResolutionSettings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                {
                    AssemblyName = "ANDP.Provisioning.API.Rest",
                    Namespace = null
                },
                new CommonMapperProfileResolutionSettings
                {
                    AssemblyName = "ANDP.Lib",
                    Namespace = null
                },
                new CommonMapperProfileResolutionSettings
                {
                    AssemblyName = "ANDP.Lib.Domain",
                    Namespace = null
                },
                new CommonMapperProfileResolutionSettings
                {
                    AssemblyName = "Common.Lib.Domain",
                    Namespace = null
                },
                new CommonMapperProfileResolutionSettings
                {
                    AssemblyName = "Common.Lib.Mvc",
                    Namespace = null
                },
                new CommonMapperProfileResolutionSettings  //this is needed for the domain models such as tenant.
                {
                    AssemblyName = Assembly.GetAssembly(typeof(Tenant)).GetName().Name,
                    Namespace = new List<string> {typeof(Tenant).Namespace}
                }
            };

            var mapper = new CommonMapper(commonMapperProfileResolutionSettings);
            Container.RegisterType<ICommonMapper>(new ContainerControlledLifetimeManager(), new InjectionFactory(_ => mapper));
        }

        private static void RegisterAuthSettings()
        {
            Container.RegisterInstance(new Oauth2AuthenticationSettings
            {
                ClientId = ConfigurationManager.AppSettings["WebApiClientId"],
                ClientSecret = ConfigurationManager.AppSettings["WebApiClientSecert"],
                Url = ConfigurationManager.AppSettings["WebApiUrl"]
            });
        }
    }
}
