
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;

namespace Common.Lib.MVC.Security.Claims
{
    public class ProvisioningClaimsAuthenticationManager : ClaimsAuthenticationManager
    {
        private readonly Func<string, string, IEnumerable<Claim>> _getClaims;

        public ProvisioningClaimsAuthenticationManager(Func<string, string, IEnumerable<Claim>> getClaims)
        {
            _getClaims = getClaims;
        }

        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (_getClaims == null)
                throw new ArgumentNullException("_getClaims", "GetClaims Method must be passed in.");

            //add the tenantId claim into the token so we can use it throughout web app (and only lookup once!)
            if (incomingPrincipal.Identity.IsAuthenticated)
            {
                // validate incoming claim
                var userName = incomingPrincipal.Identity.Name;
                if (string.IsNullOrWhiteSpace(userName))
                {
                    throw new SecurityException("No user name found");
                }

                string tenant;

                var parts = userName.Split('\\');
                if (parts.Length > 1)
                {
                    tenant = parts[0];
                    userName = parts[1];
                }
                else
                {
                    throw new Exception("Cannot determine tenant and username.");
                }

                foreach (var claim in _getClaims(userName, tenant))
                {
                    incomingPrincipal.Identities.First().AddClaim(claim);
                }
            }

            return incomingPrincipal;
        }
    }
}
