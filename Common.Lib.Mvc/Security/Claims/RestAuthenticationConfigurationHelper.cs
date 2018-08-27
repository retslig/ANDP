using System;
using System.Net;
using BrockAllen.MembershipReboot;
using Thinktecture.IdentityModel.Tokens.Http;

namespace Common.Lib.MVC.Security.Claims
{
    public static class RestAuthenticationConfigurationHelper
    {
        public static AuthenticationConfiguration Create(AuthenticationService authService)
        {
            var authentication = new AuthenticationConfiguration
            {
                ClaimsAuthenticationManager = new RestClaimsAuthenticationManager(authService),
                RequireSsl = false,
                EnableSessionToken = true
            };

            #region Basic Authentication

            authentication.AddBasicAuthentication((username, password) => ValidateUser(username, password, authService));

            #endregion

            return authentication;
        }

        private static bool ValidateUser(string userName, string password, AuthenticationService authService)
        {
            UserAccount user;
            string tenant = "";

            if (userName.Contains("\\"))
            {
                var parts = userName.Split('\\');
                tenant = parts[0];
                userName = parts[1];
            }
            else
            {
                throw new Exception("Cannot determine tenant and username.");
            }

            if (authService.UserAccountService.Authenticate(tenant, userName, password, out user))
                return true;

            throw new AuthenticationException
            {
                StatusCode = HttpStatusCode.BadRequest,
                ReasonPhrase = "Invalid Credentials"
            };
        }
    }
}
