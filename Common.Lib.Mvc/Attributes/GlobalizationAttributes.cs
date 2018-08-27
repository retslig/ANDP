using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Common.Lib.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LocalizationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// Override the basic functionality 
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpCookie cookie;
            if (filterContext.RouteData.Values["lang"] != null &&
                !string.IsNullOrWhiteSpace(filterContext.RouteData.Values["lang"].ToString()))
            {
                // set the culture from the route data (url)
                var lang = filterContext.RouteData.Values["lang"].ToString();
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);
            }
            else
            {
                // load the culture info from the cookie
                cookie = filterContext.HttpContext.Request.Cookies["Common.Localization.CurrentUICulture"];
                var langHeader = string.Empty;
                if (cookie != null)
                {
                    // set the culture by the cookie content
                    langHeader = cookie.Value;
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
                }
                else
                {
                    // set the culture by the location if not speicified
                    langHeader = filterContext.HttpContext.Request.UserLanguages[0];
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);
                }
                // set the lang value into route data
                filterContext.RouteData.Values["lang"] = langHeader;
            }

            // save the location into cookie
            cookie = new HttpCookie("Common.Localization.CurrentUICulture", Thread.CurrentThread.CurrentUICulture.Name)
                {Expires = DateTime.Now.AddYears(1)};
            filterContext.HttpContext.Response.SetCookie(cookie);

            base.OnActionExecuting(filterContext);
        }

        [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
        public class DisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
        {
            private readonly object[] _methodParms;
            private readonly string _displayFunctionName;
            private readonly Type _resourceFactory;

            public DisplayNameAttribute(Type resourceFactory, string displayFunctionName, object[] methodParms)
            {
                _resourceFactory = resourceFactory;
                _displayFunctionName = displayFunctionName;
                _methodParms = methodParms;
            }

            public DisplayNameAttribute(Type resourceFactory, object[] methodParms)
            {
                _resourceFactory = resourceFactory;
                _displayFunctionName = "GetLocalResourceObject";
                _methodParms = methodParms;
            }

            public override string DisplayName
            {
                get
                {
                    Type ty = _resourceFactory;
                    MethodInfo methodInfo = ty.GetMethod(_displayFunctionName);
                    var o = Activator.CreateInstance(ty);
                    var result = methodInfo.Invoke(o, _methodParms);
                    return result.ToString();
                }
            }
        }

        [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
        public class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute, IClientValidatable
        {
            private readonly object[] _methodParms;
            private readonly string _displayFunctionName;
            private readonly Type _resourceFactory;

            public RequiredAttribute(Type resourceFactory, object[] methodParms)
            {
                _resourceFactory = resourceFactory;
                _displayFunctionName = "GetLocalResourceObject";
                _methodParms = methodParms;
            }

            public override string FormatErrorMessage(string name)
            {
                Type ty = _resourceFactory;
                MethodInfo methodInfo = ty.GetMethod(_displayFunctionName);
                var o = Activator.CreateInstance(ty);
                var result = methodInfo.Invoke(o, _methodParms);
                ErrorMessage = result.ToString();
                return result.ToString();
            }

            public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
            {
                var modelClientValidationRule = new ModelClientValidationRule
                {
                    ErrorMessage = FormatErrorMessage(metadata.DisplayName),
                    ValidationType = "required"
                };

                yield return modelClientValidationRule;
            }
        }
    }
}
