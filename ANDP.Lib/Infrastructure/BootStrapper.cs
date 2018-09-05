
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using ANDP.Lib.Domain.Factories;
using ANDP.Lib.Domain.MappingProfiles;
using ANDP.Lib.Factories;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Ef;
using BrockAllen.MembershipReboot.WebHost;
using Common.Lib.Data.Repositories.Common;
using Common.Lib.Data.Repositories.LanguageResource;
using Common.Lib.Domain.Common.Interfaces;
using Common.Lib.Domain.Common.Services;
using Common.Lib.Extensions.UnityExtensions;
using Common.Lib.Interfaces;
using Common.Lib.Mapping;
using Common.Lib.Security;
using Common.Lib.Services;
using Common.NewNet.Factories;
using Microsoft.Practices.Unity;
using MySql.Data.MySqlClient;


namespace ANDP.Lib.Infrastructure
{

    public static class BootStrapper
    {
        private static readonly IUnityContainer Container = new UnityContainer();

        public static IUnityContainer Initialize()
        {
            BuildUnityContainer();
            //DependencyResolver.SetResolver(new UnityDependencyResolver(_container));
            //GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(_container);
            ProcessMakerServiceFactory.Container = Container;
            DispatcherServiceFactory.Container = Container;
            AuditServiceFactory.Container = Container;
            AuditServiceFactory.ConnectionString = AndpEntitiesBootstrapper().ConnectionString;
            OrderServiceFactory.Container = Container;
            OrderServiceFactory.ConnectionString = AndpEntitiesBootstrapper().ConnectionString;
            EngineServiceFactory.Container = Container;
            EngineServiceFactory.ConnectionString = AndpEntitiesBootstrapper().ConnectionString;
            EquipmentServiceFactory.Container = Container;
            EquipmentServiceFactory.ConnectionString = AndpEntitiesBootstrapper().ConnectionString;
            ProvisioningEngineServiceFactory.Container = Container;
            ProvisioningEngineServiceFactory.ConnectionString = AndpEntitiesBootstrapper().ConnectionString;
            UserAccountServiceFactory.ConnectionString = AuthEntitiesBootstrapper().ConnectionString;
            return Container;
        }

        private static void BuildUnityContainer()
        {
            Container.AddNewExtension<DisposableStrategyExtension>();

            RegisterCommonMapper();

            //Entities
            //Container.RegisterType<IANDP_Engine_Entities, ANDP_Engine_Entities>(
            //    new HierarchicalLifetimeManager(), new InjectionFactory(c =>
            //    {
            //        var context = c.Resolve<HttpContextBase>();
            //        var tenantId = context.User.Identity.Name;
            //        return new ANDP_Engine_Entities(AndpEntitiesBootstrapper().ConnectionString, tenantId);
            //    }));

            //Container.RegisterType<IANDP_Order_Entities, ANDP_Order_Entities>(
            //    new HierarchicalLifetimeManager(), new InjectionFactory(c =>
            //    {
            //        var context = c.Resolve<HttpContextBase>();
            //        var tenantId = context.User.Identity.Name;
            //        return new ANDP_Order_Entities(AndpEntitiesBootstrapper().ConnectionString, tenantId);
            //    }));

            //Authentication Settings for Token support
            RegisterAuthSettings();

            //Not necessary for multi-tenant architecture.  Common schema.
            Container.RegisterType<ICommon_Entities, Common_Entities>(new HierarchicalLifetimeManager(), new InjectionConstructor(AndpEntitiesBootstrapper().ConnectionString));
            Container.RegisterType<ICommon_LanguageResource_Entities, Common_LanguageResource_Entities>(new HierarchicalLifetimeManager(), new InjectionConstructor(AndpEntitiesBootstrapper().ConnectionString));

            var config = new MembershipRebootConfiguration
            {
                PasswordHashingIterationCount = 10000,
                RequireAccountVerification = false,
                //config.DefaultTenant = "",
                MultiTenant = true
            };

            //MembershipReboot Stuff
            Container.RegisterType<IUserAccountQuery, DefaultUserAccountRepository>(new DisposingTransientLifetimeManager());
            
            //Repos
            Container.RegisterType<DefaultMembershipRebootDatabase, DefaultMembershipRebootDatabase>(new DisposingTransientLifetimeManager(), new InjectionConstructor(AuthEntitiesBootstrapper().ConnectionString));
            Container.RegisterType<IUserAccountRepository, DefaultUserAccountRepository>(new DisposingTransientLifetimeManager());
            Container.RegisterType<ICommonRepository, CommonRepository>(new HierarchicalLifetimeManager());
            Container.RegisterType<ILanguageResourceRepository, LanguageResourceRepository>(new ExternallyControlledLifetimeManager());

            //Services
            Container.RegisterType<UserAccountService, UserAccountService>(new DisposingTransientLifetimeManager(), new InjectionConstructor(config, Container.Resolve<IUserAccountRepository>()));
            Container.RegisterType<AuthenticationService, SamAuthenticationService>(new DisposingTransientLifetimeManager());
            Container.RegisterType<ILogger, NLogWriterService>(new HierarchicalLifetimeManager(), new InjectionConstructor(AndpEntitiesBootstrapper()));
            Container.RegisterType<ILanguageResourceService, LanguageResourceService>(new ExternallyControlledLifetimeManager(),new InjectionConstructor(Container.Resolve<ILanguageResourceRepository>(), Container.Resolve<ICommonMapper>(), "en-US"));
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
                },
                new CommonMapperProfileResolutionSettings
                {
                    AssemblyName = "Common.Lib.Mvc",
                    Namespace = null
                },
                new CommonMapperProfileResolutionSettings
                {
                    AssemblyName = "Common.Lib.Integration",
                    Namespace = null
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
        
        public static SqlConnectionStringBuilder AndpEntitiesBootstrapper()
        {
            var sqlBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["ANDP_Entities"].ConnectionString);
            sqlBuilder.ApplicationName += " - " + Assembly.GetExecutingAssembly().GetName().Name;
            return sqlBuilder;
        }

        public static SqlConnectionStringBuilder AuthEntitiesBootstrapper()
        {
            var sqlBuilder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["Auth_Entities"].ConnectionString);
            sqlBuilder.ApplicationName += " - " + Assembly.GetExecutingAssembly().GetName().Name;
            return sqlBuilder;
        }

        public static MySqlConnectionStringBuilder BillingEntitiesBootstrapper()
        {
            var sqlBuilder = new MySqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["Billing_Entities"].ConnectionString);
            return sqlBuilder;
        }
    }
}
