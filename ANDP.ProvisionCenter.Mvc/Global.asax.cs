
using System.IdentityModel.Services;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ANDP.Lib.Infrastructure;
using ANDP.ProvisionCenter.Mvc.Controllers;
using Common.Lib.Data.Repositories.Common;
using Common.Lib.Interfaces;
using Common.Lib.Mapping;
using Common.Lib.MVC.Infastructure;
using Common.Lib.MVC.Security.Claims;
using Microsoft.Practices.Unity.Mvc;
using Newtonsoft.Json.Converters;

namespace ANDP.ProvisionCenter.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });

            var unityContainer = BootStrapper.Initialize();
            DependencyResolver.SetResolver(new UnityDependencyResolver(unityContainer));

            //Used for Authentication
            FederatedAuthentication.FederationConfigurationCreated += FederatedAuthentication_FederationConfigurationCreated;
        }

        void FederatedAuthentication_FederationConfigurationCreated(object sender, System.IdentityModel.Services.Configuration.FederationConfigurationCreatedEventArgs e)
        {
            e.FederationConfiguration.IdentityConfiguration.ClaimsAuthorizationManager = new MvcAuthorizationManager();
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "General";
            routeData.Values["exception"] = exception;
            routeData.Values["controllerName"] = exception.TargetSite.DeclaringType;
            routeData.Values["controllerAction"] = exception.TargetSite.Name;
            Response.StatusCode = 500;
            if (httpException != null)
            {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode)
                {
                    case 403:
                        routeData.Values["action"] = "Http403";
                        break;
                    case 404:
                        routeData.Values["action"] = "Http404";
                        break;
                }
            }

            IController errorController = new ErrorController(MvcServiceLocator.GetService<ILogger>());
            var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            errorController.Execute(rc);
        }
    }
}
