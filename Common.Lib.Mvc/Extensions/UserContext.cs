using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Routing;
using Common.Lib.Security;

namespace Common.Lib.MVC.Extensions
{
    public static class UserContext
    {
        public static IIdentity Identity
        {
            get
            {
                return Thread.CurrentPrincipal.Identity;
            }
        }

        public static bool IsAuthenticated
        {
            get
            {
                return Thread.CurrentPrincipal.Identity.IsAuthenticated;
            }
        }

        public static Claim RetrieveTenantClaim()
        {

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            if (!identity.Identity.IsAuthenticated)
                throw new Exception("User not authenticated.");

            var claim = identity.Claims.FirstOrDefault(p=>p.Type == ClaimsConstants.TenantIdClaimType);

            if (claim == null)
                throw new Exception("No tenant claim found.");

            return claim;
        }

        public static Guid RetrieveTenantId()
        {

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            if (!identity.Identity.IsAuthenticated)
                throw new Exception("User not authenticated.");

            var claim = identity.Claims.FirstOrDefault(p => p.Type == ClaimsConstants.TenantIdClaimType);

            if (claim == null)
                throw new Exception("No tenant claim found.");

            return Guid.Parse(claim.Value);
        }

        /// <summary>
        /// Retrieves the claims.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Claim> AllRetrieveClaims()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            return identity.Claims;
        }

        /// <summary>
        /// Retrieves the menu claims.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Claim> RetrieveMenuClaims()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            return identity.Claims.Where(claim => claim.Type.Contains(ClaimsConstants.MvcClaimType)).ToList();
        }

        /// <summary>
        /// Retrieves the menu claims.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Claim> RetrieveApiClaims()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            return identity.Claims.Where(claim => claim.Type.Contains(ClaimsConstants.ApiClaimType)).ToList();
        }

        /// <summary>
        /// Retrieves the user culture information.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public static CultureInfo RetrieveUserCultureInfo(RouteData routeData, HttpRequestBase request, HttpResponseBase response)
        {
            CultureInfo userCultureInfo;
            HttpCookie cookie;
            if (routeData.Values["lang"] != null &&
                !string.IsNullOrWhiteSpace(routeData.Values["lang"].ToString()))
            {
                // set the culture from the route data (url)
                var lang = routeData.Values["lang"].ToString();
                userCultureInfo = CultureInfo.CreateSpecificCulture(lang);
            }
            else
            {
                // load the culture info from the cookie
                cookie = request.Cookies["Common.Localization.CurrentUICulture"];
                string langHeader;
                if (cookie != null)
                {
                    // set the culture by the cookie content
                    langHeader = cookie.Value;
                    userCultureInfo = CultureInfo.CreateSpecificCulture(langHeader);
                }
                else
                {
                    // set the culture by the location if not speicified
                    langHeader = request.UserLanguages[0];
                    userCultureInfo = CultureInfo.CreateSpecificCulture(langHeader);
                }
                // set the lang value into route data
                routeData.Values["lang"] = langHeader;
            }

            // save the location into cookie
            cookie = new HttpCookie("Common.Localization.CurrentUICulture", userCultureInfo.Name) { Expires = DateTime.Now.AddYears(1) };
            response.SetCookie(cookie);

            return userCultureInfo;
        }
    }
}
