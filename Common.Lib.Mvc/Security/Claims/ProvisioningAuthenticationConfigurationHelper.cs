using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Reflection;
using Common.Lib.Common.Caching;
using Common.Lib.Common.Enums;
using Common.Lib.Extensions;
using Common.Lib.Interfaces;
using Common.Lib.Security;
using Common.Lib.Services;
using Common.Lib.Utility;
using Thinktecture.IdentityModel.Tokens.Http;
using System.Security.Claims;

namespace Common.Lib.MVC.Security.Claims
{
    public static class ProvisioningAuthenticationConfigurationHelper
    {
        private static Oauth2AuthenticationSettings _oauth2AuthenticationSettings;
        private static ILogger _logger;

        public static AuthenticationConfiguration Create(Oauth2AuthenticationSettings oauth2AuthenticationSettings, ILogger logger)
        {
            _logger = logger;
            _oauth2AuthenticationSettings = oauth2AuthenticationSettings;

            var authentication = new AuthenticationConfiguration
            {
                ClaimsAuthenticationManager = new ProvisioningClaimsAuthenticationManager(GetClaimsForUser),
                RequireSsl = !AuthenticationConstants.AllowInsecureHttp,
                EnableSessionToken = true
            };

            #region Basic Authentication

            authentication.AddBasicAuthentication(AuthenticateUser);

            #endregion

            return authentication;
        }

        private static bool AuthenticateUser(string userName, string password)
        {
            
            string tenantName = "";
            try
            {
                var parts = userName.Split('\\');
                if (parts.Length > 1)
                {
                    tenantName = parts[0];
                    userName = parts[1];
                }
                else
                {
                    throw new AuthenticationException("Could not determine tenant name and user name")
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        ReasonPhrase = "Could not determine tenant name and user name"
                    };
                }

                _oauth2AuthenticationSettings.Password = password;
                _oauth2AuthenticationSettings.Username = userName;
                _oauth2AuthenticationSettings.TenantName = tenantName;

                //Get Token for this user.
                var accessTokenResponse = BearerTokenHelper.RetrieveBearTokenFromCacheOrNew(_oauth2AuthenticationSettings);
                if (accessTokenResponse == null || string.IsNullOrEmpty(accessTokenResponse.AccessToken))
                {
                    throw new AuthenticationException("Unable to retrieve token")
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        ReasonPhrase = "Unable to retrieve token"
                    };
                }

                //If token was cached we did not guarantee that tenant, user name and password are correct.
                //We only verified that the tenant and user name are the same.
                var memoryCachingService = new MemoryCacheProvider();
                var hashedPassword = memoryCachingService.FetchAndCache(accessTokenResponse.AccessToken, () => EncryptionHelper.Md5Encryption.GetMd5Hash(password), SecurityTokenConstants.TokenLifeTime);
                if (EncryptionHelper.Md5Encryption.GetMd5Hash(password) != hashedPassword)
                    throw new AuthenticationException("username or password does not match")
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        ReasonPhrase = "username or password does not match"
                    };

                //Validates that the token is good.
                ClaimsWebApiHelper.Authenticate(_oauth2AuthenticationSettings.Url, accessTokenResponse.AccessToken);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(tenantName, null, MethodBase.GetCurrentMethod().Name + " " + ex.GetInnerMostException(), LogLevelType.Error, ex);
                throw;
            }

            return true;
        }

        private static IEnumerable<Claim> GetClaimsForUser(string userName, string tenantName)
        {
            try
            {
                _oauth2AuthenticationSettings.Username = userName;
                _oauth2AuthenticationSettings.TenantName = tenantName;
                var accessTokenResponse = BearerTokenHelper.RetrieveBearTokenFromCache(_oauth2AuthenticationSettings);

                var claims = ClaimsWebApiHelper.GetClaims(_oauth2AuthenticationSettings, accessTokenResponse.AccessToken);
                return claims;
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(tenantName, null, MethodBase.GetCurrentMethod().Name + " " + ex.ToString() + " " + ex.Message, LogLevelType.Error, ex);
                throw;
            }
        }
    }
}
