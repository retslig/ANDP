using System.IdentityModel.Services;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ANDP.Provisioning.API.Rest.Infrastructure;
using BrockAllen.MembershipReboot;
using Common.Lib.Data.Repositories.Common;
using Common.Lib.Mapping;
using Common.Lib.MVC.Security.Claims;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ANDP.Provisioning.API.Rest
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var unityContainer = BootStrapper.Initialize();
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(unityContainer);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var jSettings = new JsonSerializerSettings();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = jSettings;

            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            FederatedAuthentication.FederationConfigurationCreated += FederatedAuthentication_FederationConfigurationCreated;
        }

        void FederatedAuthentication_FederationConfigurationCreated(object sender, System.IdentityModel.Services.Configuration.FederationConfigurationCreatedEventArgs e)
        {
           // var dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
            //e.FederationConfiguration.IdentityConfiguration.ClaimsAuthenticationManager = new RestClaimsTransformerAuthenticationManager(commonRepository);
            e.FederationConfiguration.IdentityConfiguration.ClaimsAuthorizationManager = new ApiAuthorizationManager();
        }
    }
}
