using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Common.Lib.MVC.Helpers
{
    public static class GlobalizationHelper
    {
        public static IHtmlString MetaAcceptLanguage<T>(this HtmlHelper<T> html)
        {
            var acceptLanguage = HttpUtility.HtmlAttributeEncode(Thread.CurrentThread.CurrentUICulture.ToString());
            return new HtmlString(String.Format("<meta name=\"accept-language\" content=\"{0}\">", acceptLanguage));
        }

        public class Language
        {
            public string Url { get; set; }
            public string ActionName { get; set; }
            public string ControllerName { get; set; }
            public RouteValueDictionary RouteValues { get; set; }
            public bool IsSelected { get; set; }

            public MvcHtmlString HtmlSafeUrl
            {
                get { return MvcHtmlString.Create(Url); }
            }
        }

        public static Language LanguageUrl(this HtmlHelper helper, string cultureName,
                                           string languageRouteName = "lang", bool strictSelected = false)
        {
            // set the input language to lower
            cultureName = cultureName.ToLower();
            // retrieve the route values from the view context
            var routeValues = new RouteValueDictionary(helper.ViewContext.RouteData.Values);
            // copy the query strings into the route values to generate the link
            var queryString = helper.ViewContext.HttpContext.Request.QueryString;
            foreach (string key in queryString)
            {
                if (queryString[key] != null && !string.IsNullOrWhiteSpace(key))
                {
                    if (routeValues.ContainsKey(key))
                    {
                        routeValues[key] = queryString[key];
                    }
                    else
                    {
                        routeValues.Add(key, queryString[key]);
                    }
                }
            }
            var actionName = routeValues["action"].ToString();
            var controllerName = routeValues["controller"].ToString();
            // set the language into route values
            routeValues[languageRouteName] = cultureName;
            // generate the language specify url
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var url = urlHelper.RouteUrl("Localization", routeValues);
            // check whether the current thread ui culture is this language
            var current_lang_name = Thread.CurrentThread.CurrentUICulture.Name.ToLower();
            var isSelected = strictSelected
                                 ? current_lang_name == cultureName
                                 : current_lang_name.StartsWith(cultureName);
            return new Language
                {
                Url = url,
                ActionName = actionName,
                ControllerName = controllerName,
                RouteValues = routeValues,
                IsSelected = isSelected
            };
        }

        public static MvcHtmlString LanguageSelectorLink(this HtmlHelper helper,
                                                         string cultureName, string selectedText, string unselectedText,
                                                         IDictionary<string, object> htmlAttributes,
                                                         string languageRouteName = "lang", bool strictSelected = false)
        {
            var language = helper.LanguageUrl(cultureName, languageRouteName, strictSelected);
            var link = helper.RouteLink(language.IsSelected ? selectedText : unselectedText,
                                        "Localization", language.RouteValues, htmlAttributes);
            return link;
        }
    }
}
