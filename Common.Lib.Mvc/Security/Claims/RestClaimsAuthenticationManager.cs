
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using BrockAllen.MembershipReboot;

namespace Common.Lib.MVC.Security.Claims
{
    public class RestClaimsAuthenticationManager : ClaimsAuthenticationManager
    {
        private readonly AuthenticationService _authService;

        public RestClaimsAuthenticationManager(AuthenticationService authService)
        {
            _authService = authService;
        }

        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (_authService == null)
                throw new ArgumentNullException("AuthenticationService", "Service is Not Initialized.");

            //add the tenantId claim into the token so we can use it throughout web app (and only lookup once!)
            if (incomingPrincipal.Identity.IsAuthenticated)
            {
                // validate incoming claim
                var name = incomingPrincipal.Identity.Name;
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new SecurityException("No user name found");
                }
                
                string tenant = "";

                if (name.Contains("\\"))
                {
                    var parts = name.Split('\\');
                    tenant = parts[0];
                    name = parts[1];
                }
                else
                {
                    throw new Exception("Cannot determine tenant and username.");
                }

                var user = _authService.UserAccountService.GetByUsername(tenant, name);
                foreach (var claim in user.GetAllClaims())
                {
                    incomingPrincipal.Identities.First().AddClaim(claim);
                }
            }

            return base.Authenticate(resourceName, incomingPrincipal);
        }
    }
}
