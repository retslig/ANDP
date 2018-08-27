using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using ANDP.Lib.Data.Repositories.Engine;
using ANDP.Lib.Data.Repositories.Equipment;
using ANDP.Lib.Data.Repositories.Order;
using ANDP.Lib.Domain.Factories;
using ANDP.Lib.Domain.Interfaces;
using ANDP.Lib.Domain.MappingProfiles;
using ANDP.Lib.Domain.Services;
using Common.Lib.Data.Repositories.Common;
using Common.Lib.Data.Repositories.LanguageResource;
using Common.Lib.Extensions.UnityExtensions;
using Common.Lib.Interfaces;
using Common.Lib.Mapping;
using Common.Lib.Services;
using Microsoft.Practices.Unity;

namespace ANDP.Domain.Test.Infrastructure
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
            RegisterCommonMapper();
            Container.RegisterType<ICommon_Entities, Common_Entities>(new HierarchicalLifetimeManager(), new InjectionConstructor(AndpEntitiesBootstrapper().ConnectionString));
            Container.RegisterType<IANDP_Engine_Entities, ANDP_Engine_Entities>(new DisposingTransientLifetimeManager(), new InjectionConstructor(AndpEntitiesBootstrapper().ConnectionString));
            Container.RegisterType<IANDP_Order_Entities, ANDP_Order_Entities>(new DisposingTransientLifetimeManager(), new InjectionConstructor(AndpEntitiesBootstrapper().ConnectionString));
            Container.RegisterType<IANDP_Equipment_Entities, ANDP_Equipment_Entities>(new DisposingTransientLifetimeManager(), new InjectionConstructor(AndpEntitiesBootstrapper().ConnectionString));
            
            //Repos
            Container.RegisterType<ICommonRepository, CommonRepository>(new HierarchicalLifetimeManager());
            Container.RegisterType<IOrderRepository, OrderRepository>(new HierarchicalLifetimeManager());
            Container.RegisterType<IEngineRepository, EngineRepository>(new HierarchicalLifetimeManager());
            Container.RegisterType<ICommonRepository, CommonRepository>(new HierarchicalLifetimeManager());
            Container.RegisterType<ILanguageResourceRepository, LanguageResourceRepository>(new ExternallyControlledLifetimeManager());
            Container.RegisterType<IEquipmentRepository, EquipmentRepository>(new HierarchicalLifetimeManager());

            //Services
            Container.RegisterType<IOrderService, OrderService>(new HierarchicalLifetimeManager());
        }

        private static void RegisterCommonMapper()
        {
            //Inject CommonMapper the Auto Mapping Engine.
            var commonMapperProfileResolutionSettings = new List<CommonMapperProfileResolutionSettings>
            {
                new CommonMapperProfileResolutionSettings
                {
                    AssemblyName = Assembly.GetAssembly(typeof(OrderProfile)).GetName().Name,
                    Namespace = new List<string> {typeof(OrderProfile).Namespace}
                },
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
