using System.Configuration;
using System.Web.Http;
using Common.Lib.Interfaces;
using Common.Lib.MVC.Security.Claims;
using Common.Lib.Security;
using Common.Lib.Services;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Thinktecture.IdentityModel.Tokens.Http;

namespace ANDP.Provisioning.API.Rest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Enable Cross Origin Resource Sharing (CORS)
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new ClaimsAuthorizeAttribute());

            var dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
            var nlogWriterService = (NLogWriterService)dependencyResolver.GetService(typeof(ILogger));
            var oauth2AuthenticationSettings = (Oauth2AuthenticationSettings)dependencyResolver.GetService(typeof(Oauth2AuthenticationSettings));

            // authentication configuration for identity controller
            config.MessageHandlers.Add(new AuthenticationHandler(ProvisioningAuthenticationConfigurationHelper.Create(oauth2AuthenticationSettings, nlogWriterService)));
        }
    }
}
