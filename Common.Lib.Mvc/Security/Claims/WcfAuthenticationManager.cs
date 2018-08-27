using System;
using System.Configuration;
using System.IdentityModel.Selectors;
using Common.Lib.Security;

namespace Common.Lib.MVC.Security.Claims
{
    public class WcfAuthenticationManager : UserNamePasswordValidator
    {
        private readonly Oauth2AuthenticationSettings _oauth2AuthenticationSettings;

        public WcfAuthenticationManager()
        {
            _oauth2AuthenticationSettings = new Oauth2AuthenticationSettings
            {
                ClientId = ConfigurationManager.AppSettings["WebApiClientId"],
                ClientSecret = ConfigurationManager.AppSettings["WebApiClientSecert"],
                Url = ConfigurationManager.AppSettings["WebApiUrl"]
            };
        }

        public override void Validate(string userName, string password)
        {
            if (null == userName || null == password)
            {
                throw new ArgumentNullException();
            }

            string tenantName;

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
            _oauth2AuthenticationSettings.Password = password;
            var accessTokenResponse = BearerTokenHelper.RetrieveBearTokenFromCacheOrNew(_oauth2AuthenticationSettings);
            ClaimsWebApiHelper.Authenticate(_oauth2AuthenticationSettings.Url, accessTokenResponse.AccessToken);
        }
    }
}