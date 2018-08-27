using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Security.Principal;
using Common.Lib.Security;

namespace Common.Lib.MVC.Security.Claims
{
    public class WcfAuthorizationManager : IAuthorizationPolicy
    {
        private readonly Oauth2AuthenticationSettings _oauth2AuthenticationSettings;
        private string id;
        private IIdentity GetClientIdentity(EvaluationContext evaluationContext)
        {
            object obj;
            if (!evaluationContext.Properties.TryGetValue("Identities", out obj))
                throw new Exception("No Identity found");

            var identities = obj as IList<IIdentity>;
            if (identities == null || identities.Count <= 0)
                throw new Exception("No Identity found");

            return identities[0];
        }

        public WcfAuthorizationManager()
        {
            id = Guid.NewGuid().ToString();

            _oauth2AuthenticationSettings = new Oauth2AuthenticationSettings
            {
                ClientId = ConfigurationManager.AppSettings["WebApiClientId"],
                ClientSecret = ConfigurationManager.AppSettings["WebApiClientSecert"],
                Url = ConfigurationManager.AppSettings["WebApiUrl"]
            };
        }

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        public string Id
        {
            get { return id; }
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            // get the authenticated client identity
            var client = GetClientIdentity(evaluationContext);

            string tenantName;
            string userName = client.Name;

            if (userName.Contains("\\"))
            {
                var parts = userName.Split('\\');
                tenantName = parts[0];
                userName = parts[1];
            }
            else
            {
                throw new Exception("Cannot determine tenant and username.");
            }

            _oauth2AuthenticationSettings.Username = userName;
            _oauth2AuthenticationSettings.TenantName = tenantName;
            var accessTokenResponse = BearerTokenHelper.RetrieveBearTokenFromCache(_oauth2AuthenticationSettings);
            var claims = ClaimsWebApiHelper.GetClaims(_oauth2AuthenticationSettings, accessTokenResponse.AccessToken);
            ((System.Security.Claims.ClaimsIdentity)client).AddClaims(claims);
            // set the custom principal
            evaluationContext.Properties["Principal"] = new GenericPrincipal(client, null);

            return true;
        }
    }
}


