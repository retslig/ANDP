using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ANDP.ProvisionCenter.Mvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(@"Localization",
                @"{lang}/{controller}/{action}/{id}",
                new { controller = @"Home", action = @"Index", id = UrlParameter.Optional },
                new
                {
                    lang = @"\w{2,3}(-\w{4})?(-\w{2,3})?"
                },
                namespaces: new string[] { "ANDP.ProvisionCenter.Mvc.Controllers" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "ANDP.ProvisionCenter.Mvc.Controllers" }
            );
        }
    }
}
