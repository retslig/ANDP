using System;
using System.Collections.Generic;
using Common.Lib.Common.Caching;
using Common.Lib.Services;
using Common.Lib.Utility;

namespace Common.Lib.Security
{
    public static class BearerTokenHelper
    {
        /// <summary>
        /// Retrieves the new bearer token.
        /// </summary>
        /// <param name="authenticationSettings">The authentication settings.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// authenticationSettings.ClientId
        /// or
        /// authenticationSettings.ClientSecret
        /// or
        /// authenticationSettings.Url
        /// or
        /// authenticationSettings.Password
        /// or
        /// authenticationSettings.Username
        /// or
        /// authenticationSettings.TenantName
        /// </exception>
        public static AccessTokenResponse RetrieveNewBearToken(Oauth2AuthenticationSettings authenticationSettings)
        {
            if (string.IsNullOrEmpty(authenticationSettings.ClientId))
                throw new ArgumentNullException("authenticationSettings.ClientId");

            if (string.IsNullOrEmpty(authenticationSettings.ClientSecret))
                throw new ArgumentNullException("authenticationSettings.ClientSecret");

            if (string.IsNullOrEmpty(authenticationSettings.Url))
                throw new ArgumentNullException("authenticationSettings.Url");

            if (string.IsNullOrEmpty(authenticationSettings.Password))
                throw new ArgumentNullException("authenticationSettings.Password");

            if (string.IsNullOrEmpty(authenticationSettings.Username))
                throw new ArgumentNullException("authenticationSettings.Username");

            if (string.IsNullOrEmpty(authenticationSettings.TenantName))
                throw new ArgumentNullException("authenticationSettings.TenantName");

            var oauthClient = new OAuth2Client(new Uri(authenticationSettings.Url + "token"), authenticationSettings.ClientId, authenticationSettings.ClientSecret);
            var accessTokenResponse = oauthClient.RequestAccessTokenUserName(authenticationSettings.Username, authenticationSettings.Password, authenticationSettings.TenantName);

            string key = string.Concat("AuthHash:", EncryptionHelper.Md5Encryption.GetMd5Hash(string.Concat(authenticationSettings.TenantName, authenticationSettings.Username)));
            //Cache Token in Memory 
            var memoryCachingService = new MemoryCacheProvider();
            memoryCachingService.SetCache(key, accessTokenResponse, SecurityTokenConstants.TokenLifeTime);

            return accessTokenResponse;
        }

        /// <summary>
        /// Retrieves the new refresh bearer token.
        /// </summary>
        /// <param name="authenticationSettings">The authentication settings.</param>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">authenticationSettings.ClientId
        /// or
        /// authenticationSettings.ClientSecret
        /// or
        /// authenticationSettings.Url
        /// or
        /// authenticationSettings.Username
        /// or
        /// authenticationSettings.TenantName
        /// or
        /// expiredToken</exception>
        public static AccessTokenResponse RetrieveNewRefreshBearToken(Oauth2AuthenticationSettings authenticationSettings, string refreshToken)
        {
            //Not working get a invalid Grant error.
            //http://bitoftech.net/2014/07/16/enable-oauth-refresh-tokens-angularjs-app-using-asp-net-web-api-2-owin/

            if (string.IsNullOrEmpty(authenticationSettings.ClientId))
                throw new ArgumentNullException("authenticationSettings.ClientId");

            if (string.IsNullOrEmpty(authenticationSettings.ClientSecret))
                throw new ArgumentNullException("authenticationSettings.ClientSecret");

            if (string.IsNullOrEmpty(authenticationSettings.Url))
                throw new ArgumentNullException("authenticationSettings.Url");

            if (string.IsNullOrEmpty(authenticationSettings.Username))
                throw new ArgumentNullException("authenticationSettings.Username");

            if (string.IsNullOrEmpty(authenticationSettings.TenantName))
                throw new ArgumentNullException("authenticationSettings.TenantName");

            if (string.IsNullOrEmpty(refreshToken))
                throw new ArgumentNullException("refreshToken");

            var oauthClient = new OAuth2Client(new Uri(authenticationSettings.Url + "token"), authenticationSettings.ClientId, authenticationSettings.ClientSecret);
            var accessTokenResponse = oauthClient.RequestAccessTokenRefreshToken(refreshToken, new Dictionary<string, string>
			{
				{ OAuth2Constants.ClientId, authenticationSettings.ClientId },
				{ OAuth2Constants.ClientSecret, authenticationSettings.ClientSecret }
			});

            string key = string.Concat("AuthHash:", EncryptionHelper.Md5Encryption.GetMd5Hash(string.Concat(authenticationSettings.TenantName, authenticationSettings.Username)));
            //Cache Token in Memory 
            var memoryCachingService = new MemoryCacheProvider();
            memoryCachingService.SetCache(key, accessTokenResponse, SecurityTokenConstants.TokenLifeTime);

            return accessTokenResponse;
        }

        /// <summary>
        /// Retrieves the bearer token from cache or gets a new token.
        /// </summary>
        /// <param name="authenticationSettings">The authentication settings.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// authenticationSettings.ClientId
        /// or
        /// authenticationSettings.ClientSecret
        /// or
        /// authenticationSettings.Url
        /// or
        /// authenticationSettings.Password
        /// or
        /// authenticationSettings.Username
        /// or
        /// authenticationSettings.TenantName
        /// </exception>
        public static AccessTokenResponse RetrieveBearTokenFromCacheOrNew(Oauth2AuthenticationSettings authenticationSettings)
        {
            if (string.IsNullOrEmpty(authenticationSettings.ClientId))
                throw new ArgumentNullException("authenticationSettings.ClientId");

            if (string.IsNullOrEmpty(authenticationSettings.ClientSecret))
                throw new ArgumentNullException("authenticationSettings.ClientSecret");

            if (string.IsNullOrEmpty(authenticationSettings.Url))
                throw new ArgumentNullException("authenticationSettings.Url");

            if (string.IsNullOrEmpty(authenticationSettings.Password))
                throw new ArgumentNullException("authenticationSettings.Password");

            if (string.IsNullOrEmpty(authenticationSettings.Username))
                throw new ArgumentNullException("authenticationSettings.Username");

            if (string.IsNullOrEmpty(authenticationSettings.TenantName))
                throw new ArgumentNullException("authenticationSettings.TenantName");

            var oauthClient = new OAuth2Client(new Uri(authenticationSettings.Url + "token"), authenticationSettings.ClientId, authenticationSettings.ClientSecret);

            string key = string.Concat("AuthHash:", EncryptionHelper.Md5Encryption.GetMd5Hash(string.Concat(authenticationSettings.TenantName, authenticationSettings.Username)));
            //Cache Token in Memory 
            var memoryCachingService = new MemoryCacheProvider();
            var accessTokenResponse = memoryCachingService.FetchAndCache(key, () =>
                oauthClient.RequestAccessTokenUserName(authenticationSettings.Username, authenticationSettings.Password, authenticationSettings.TenantName),
                SecurityTokenConstants.TokenLifeTime);

            //If token is within the threshold of expiring get refresh token.
            var timspan = accessTokenResponse.ExpiresOn - DateTime.Now;
            //if (accessTokenResponse.ExpiresOn >= DateTime.Now - SecurityTokenConstants.TokenLifeTimeEndOfLifeThreshold)
            if (timspan > new TimeSpan(0, 0, 0, 0) && timspan < SecurityTokenConstants.TokenLifeTimeEndOfLifeThreshold)
            {
                accessTokenResponse = RetrieveNewRefreshBearToken(authenticationSettings, accessTokenResponse.RefreshToken);
            }

            if (accessTokenResponse == null || accessTokenResponse.ExpiresOn <= DateTime.Now)
            {
                memoryCachingService.ClearCache(key);
                accessTokenResponse = memoryCachingService.FetchAndCache(key, () =>
                    oauthClient.RequestAccessTokenUserName(authenticationSettings.Username, authenticationSettings.Password, authenticationSettings.TenantName),
                    SecurityTokenConstants.TokenLifeTime);
                return accessTokenResponse;
            }

            return accessTokenResponse;
        }

        /// <summary>
        /// Retrieves the bearer token from cache.
        /// </summary>
        /// <param name="authenticationSettings">The authentication settings.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">authenticationSettings.Username
        /// or
        /// authenticationSettings.TenantName
        /// or
        /// refreshToken</exception>
        public static AccessTokenResponse RetrieveBearTokenFromCache(Oauth2AuthenticationSettings authenticationSettings)
        {
            if (string.IsNullOrEmpty(authenticationSettings.Username))
                throw new ArgumentNullException("authenticationSettings.Username");

            if (string.IsNullOrEmpty(authenticationSettings.TenantName))
                throw new ArgumentNullException("authenticationSettings.TenantName");

            string key = string.Concat("AuthHash:", EncryptionHelper.Md5Encryption.GetMd5Hash(string.Concat(authenticationSettings.TenantName, authenticationSettings.Username)));
            //Cache Token in Memory 
            var memoryCachingService = new MemoryCacheProvider();
            var accessTokenResponse = memoryCachingService.Fetch<AccessTokenResponse>(key);

            //If token is within the threshold of expiring get refresh token.
            var timspan = accessTokenResponse.ExpiresOn - DateTime.Now;
            //if (accessTokenResponse.ExpiresOn >= DateTime.Now - SecurityTokenConstants.TokenLifeTimeEndOfLifeThreshold)
            if (timspan > new TimeSpan(0, 0, 0, 0) && timspan < SecurityTokenConstants.TokenLifeTimeEndOfLifeThreshold)
            {
                accessTokenResponse = RetrieveNewRefreshBearToken(authenticationSettings, accessTokenResponse.RefreshToken);
            }

            return accessTokenResponse;
        }
    }
}