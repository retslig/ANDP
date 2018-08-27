using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using ANDP.Lib.Data.Repositories.Engine;
using ANDP.Lib.Data.Repositories.Order;
using ANDP.Lib.Domain.Factories;
using Common.Lib.Data.Repositories.Common;
using Common.Lib.Data.Repositories.LanguageResource;
using Common.Lib.Domain.Common.Interfaces;
using Common.Lib.Domain.Common.Services;
using Common.Lib.Extensions.UnityExtensions;
using Common.Lib.Interfaces;
using Common.Lib.Mapping;
using Common.Lib.Services;
using Microsoft.Practices.Unity;

namespace ANDP.Dispatcher.Console.Infrastructure
{

    public static class BootStrapper
    {
        private static readonly IUnityContainer Container = new UnityContainer();

        public static IUnityContainer Initialize()
        {
            BuildUnityContainer();

            DispatcherServiceFactory.Container = Container;
            OrderServiceFactory.Container = Container;
            OrderServiceFactory.ConnectionString = AndpEntitiesBootstrapper().ConnectionString;
            EngineServiceFactory.Container = Container;
            EngineServiceFactory.ConnectionString = AndpEntitiesBootstrapper().ConnectionString;
            ProvisioningEngineServiceFactory.Container = Container;
            ProvisioningEngineServiceFactory.ConnectionString = AndpEntitiesBootstrapper().ConnectionString;
            return Container;
        }

        private static void BuildUnityContainer()
        {
            Container.AddNewExtension<DisposableStrategyExtension>();

            RegisterCommonMapper();

            Container.RegisterType<IANDP_Engine_Entities, ANDP_Engine_Entities>(
                new DisposingTransientLifetimeManager(), new InjectionConstructor(AndpEntitiesBootstrapper().ConnectionString, typeof(string)));
            Container.RegisterType<IANDP_Order_Entities, ANDP_Order_Entities>(
                new DisposingTransientLifetimeManager(), new InjectionConstructor(AndpEntitiesBootstrapper().ConnectionString, typeof(string)));
            
            //Not necessary for multi-tenant architecture.  Common schema.
            Container.RegisterType<ICommon_Entities, Common_Entities>(
                new HierarchicalLifetimeManager(), new InjectionConstructor(AndpEntitiesBootstrapper().ConnectionString));
            Container.RegisterType<ICommon_LanguageResource_Entities, Common_LanguageResource_Entities>(
                new HierarchicalLifetimeManager(), new InjectionConstructor(AndpEntitiesBootstrapper().ConnectionString));

            //Repos
            Container.RegisterType<ICommonRepository, CommonRepository>(new HierarchicalLifetimeManager());
            Container.RegisterType<ILanguageResourceRepository, LanguageResourceRepository>(new ExternallyControlledLifetimeManager());
            Container.RegisterType<IOrderRepository, OrderRepository>(new HierarchicalLifetimeManager());
            Container.RegisterType<IEngineRepository, EngineRepository>(new HierarchicalLifetimeManager());

            //Services
            Container.RegisterType<ILogger, NLogWriterService>(new HierarchicalLifetimeManager(), new InjectionConstructor(AndpEntitiesBootstrapper()));
            Container.RegisterType<ILanguageResourceService, LanguageResourceService>(new ExternallyControlledLifetimeManager(),
                new InjectionConstructor(Container.Resolve<ILanguageResourceRepository>(), Container.Resolve<ICommonMapper>(), "en-US"));
            //Container.RegisterType<IOrderService, OrderService>(new HierarchicalLifetimeManager());
            //Container.RegisterType<IProvisioningEngineService, ProvisioningEngineService>(new HierarchicalLifetimeManager());
        }

        private static void RegisterCommonMapper()
        {
            //Inject CommonMapper the Auto Mapping Engine.
            var commonMapperProfileResolutionSettings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                {
                    AssemblyName = "Common.Lib.Domain",
                    Namespace = null
                },
                new CommonMapperProfileResolutionSettings
                {
                    AssemblyName = "ANDP.Lib.Domain",
                    Namespace = null
                }
            };

            var mapper = new CommonMapper(commonMapperProfileResolutionSettings);
            Container.RegisterType<ICommonMapper>(new ContainerControlledLifetimeManager(), new InjectionFactory(_ => mapper));
        }

        public static SqlConnectionStringBuilder AndpEntitiesBootstrapper()
        {
            var sqlBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["ANDP_Entities"].ConnectionString);
            sqlBuilder.ApplicationName += " - " + Assembly.GetExecutingAssembly().GetName().Name;
            return sqlBuilder;
        }
    }
}
