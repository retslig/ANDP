
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Common.Lib.Extensions;
using Common.Lib.MVC.Extensions;
using Common.Lib.Security;

namespace Common.Lib.MVC.Security.Claims
{
    public class ApiAuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            var resources = context.Resource.Select(resource => ClaimsConstants.ApiClaimType + resource.Value.ToLower()).ToList();
            var actions = context.Action.Select(action => action.Value.ToLower()).ToList();
            var claims = context.Principal.Claims;

            if (!context.Principal.Identity.IsAuthenticated)
                return false;

            return
                resources.Any(
                    resource => actions.Any(action => claims.HasClaim(resource, action, false)))
                || resources.Any(resource => actions.Any(action => claims.HasClaim(resource, "all", false)));
        }

        public bool CheckAccess(IEnumerable<string> actions, IEnumerable<string> resources)
        {
            resources = resources.Select(resource => ClaimsConstants.ApiClaimType + resource.ToLower());
            actions = actions.Select(action => action.ToLower());
            var claims = UserContext.AllRetrieveClaims();

            if (!UserContext.IsAuthenticated)
                return false;

            return
                resources.Any(
                    resource => actions.Any(action => claims.HasClaim(resource, action, false)))
                || resources.Any(resource => actions.Any(action => claims.HasClaim(resource, "all", false)));
        }
    }

    public class MvcAuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            var resources = context.Resource.Select(resource => ClaimsConstants.MvcClaimType + resource.Value.ToLower()).ToList();
            var actions = context.Action.Select(action => action.Value.ToLower()).ToList();
            var claims = context.Principal.Claims;

                if (!context.Principal.Identity.IsAuthenticated)
                return false;

            return
                resources.Any(
                    resource => actions.Any(action => claims.HasClaim(resource, action, false)))
                || resources.Any(resource => actions.Any(action => claims.HasClaim(resource, "all", false)));
        }
    }
}
