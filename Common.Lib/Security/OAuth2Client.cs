using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Thinktecture.IdentityModel.Tokens.Http;

namespace Common.Lib.Security
{
	public class OAuth2Client
	{
		HttpClient _client;

		public OAuth2Client(Uri address)
		{
            var handler = new WebRequestHandler
            {
                ServerCertificateValidationCallback = CertificateHelper.ServerCertificateValidationCallbackAllowAll,
                //CookieContainer = new CookieContainer()
                //AllowAutoRedirect = false
            };

            _client = new HttpClient(handler) { BaseAddress = address };
		}

		public OAuth2Client(Uri address, string clientId, string clientSecret)
			: this(address)
		{
			_client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(clientId, clientSecret);
		}

		public static string CreateCodeFlowUrl(string endpoint, string clientId, string scope, string redirectUri, string state = null)
		{
			return CreateUrl(
				endpoint, 
				clientId, 
				scope, 
				redirectUri, 
				OAuth2Constants.ResponseTypes.Code, 
				state);
		}

		public static string CreateImplicitFlowUrl(string endpoint, string clientId, string scope, string redirectUri, string state = null)
		{
			return CreateUrl(
				endpoint, 
				clientId, 
				scope, 
				redirectUri, 
				OAuth2Constants.ResponseTypes.Token, 
				state);
		}

		private static string CreateUrl(string endpoint, string clientId, string scope, string redirectUri, string responseType, string state = null)
		{
			var url = string.Format("{0}?client_id={1}&scope={2}&redirect_uri={3}&response_type={4}",
				endpoint,
				clientId,
				scope,
				redirectUri,
				responseType);

			if (!string.IsNullOrWhiteSpace(state))
			{
				url = string.Format("{0}&state={1}", url, state);
			}

			return url;
		}

		public AccessTokenResponse RequestAccessTokenUserName(string userName, string password, string scope, Dictionary<string, string> additionalProperties = null)
		{
			var response = _client.PostAsync("", CreateFormUserName(userName, password, scope, additionalProperties)).Result;
            if (!response.IsSuccessStatusCode)
		    {
		        throw new Exception(response.Content.ReadAsStringAsync().Result);
		    }

			var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
			return CreateResponseFromJson(json);
		}

		public AccessTokenResponse RequestAccessTokenClientCredentials(string scope, Dictionary<string, string> additionalProperties = null)
		{
			var response = _client.PostAsync("", CreateFormClientCredentials(scope, additionalProperties)).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

			var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
			return CreateResponseFromJson(json);
		}

		public AccessTokenResponse RequestAccessTokenRefreshToken(string refreshToken, Dictionary<string, string> additionalProperties = null)
		{
            var response = _client.PostAsync("", CreateFormRefreshToken(refreshToken, additionalProperties)).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

			var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
			return CreateResponseFromJson(json);
		}

		public AccessTokenResponse RequestAccessTokenCode(string code, Dictionary<string, string> additionalProperties = null)
		{
			var response = _client.PostAsync("", CreateFormCode(code, additionalProperties)).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

			var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
			return CreateResponseFromJson(json);
		}

		public AccessTokenResponse RequestAccessTokenCode(string code, Uri redirectUri, Dictionary<string, string> additionalProperties = null)
		{
			var response = _client.PostAsync("", CreateFormCode(code, redirectUri, additionalProperties)).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

			var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
			return CreateResponseFromJson(json);
		}

		public AccessTokenResponse RequestAccessTokenAssertion(string assertion, string assertionType, string scope, Dictionary<string, string> additionalProperties = null)
		{
			var response = _client.PostAsync("", CreateFormAssertion(assertion, assertionType, scope, additionalProperties)).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

			var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
			return CreateResponseFromJson(json);
		}

		protected virtual FormUrlEncodedContent CreateFormClientCredentials(string scope, Dictionary<string, string> additionalProperties = null)
		{
			var values = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.ClientCredentials },
				{ OAuth2Constants.Scope, scope }
			};

			return CreateForm(values, additionalProperties);
		}

		protected virtual FormUrlEncodedContent CreateFormUserName(string userName, string password, string scope, Dictionary<string, string> additionalProperties = null)
		{
			var values = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.Password },
				{ OAuth2Constants.UserName, userName },
				{ OAuth2Constants.Password, password },
				{ OAuth2Constants.Scope, scope }
			};

			return CreateForm(values, additionalProperties);
		}

		protected virtual FormUrlEncodedContent CreateFormRefreshToken(string refreshToken, Dictionary<string, string> additionalProperties = null)
		{
			var values = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.RefreshToken },
				{ OAuth2Constants.GrantTypes.RefreshToken, refreshToken}
			};

			return CreateForm(values, additionalProperties);
		}

		protected virtual FormUrlEncodedContent CreateFormCode(string code, Dictionary<string, string> additionalProperties = null)
		{
			var values = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.AuthorizationCode },
				{ OAuth2Constants.Code, code}
			};

			return CreateForm(values, additionalProperties);
		}

		protected virtual FormUrlEncodedContent CreateFormCode(string code, Uri redirectUri, Dictionary<string, string> additionalProperties = null)
		{
			var values = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.AuthorizationCode },
				{ OAuth2Constants.RedirectUri, redirectUri.AbsoluteUri },
				{ OAuth2Constants.Code, code}
			};

			return CreateForm(values, additionalProperties);
		}

		protected virtual FormUrlEncodedContent CreateFormAssertion(string assertion, string assertionType, string scope, Dictionary<string, string> additionalProperties = null)
		{
			var values = new Dictionary<string, string>
			{
				{ OAuth2Constants.GrantType, assertionType },
				{ OAuth2Constants.Assertion, assertion },
				{ OAuth2Constants.Scope, scope }
			};

			return CreateForm(values, additionalProperties);
		}

		private AccessTokenResponse CreateResponseFromJson(JObject json)
		{
		    int expiresin = int.Parse(json["expires_in"].ToString());
			var response = new AccessTokenResponse
			{
				AccessToken = json["access_token"].ToString(),
				TokenType = json["token_type"].ToString(),
                ExpiresIn = expiresin,
                ExpiresOn = DateTime.Now.AddSeconds(expiresin)
			};

			if (json["refresh_token"] != null)
			{
				response.RefreshToken = json["refresh_token"].ToString();
			}

			return response;
		}
		/// <summary>
		/// FormUrlEncodes both Sets of Key Value Pairs into one form object
		/// </summary>
		/// <param name="explicitProperties"></param>
		/// <param name="additionalProperties"></param>
		/// <returns></returns>
		private static FormUrlEncodedContent CreateForm(Dictionary<string, string> explicitProperties, Dictionary<string, string> additionalProperties = null)
		{

			return
				new FormUrlEncodedContent(MergeAdditionKeyValuePairsIntoExplicitKeyValuePairs(explicitProperties,
																							  additionalProperties));
		}
		/// <summary>
		/// Merges additional into explicit properties keeping all explicit properties intact
		/// </summary>
		/// <param name="explicitProperties"></param>
		/// <param name="additionalProperties"></param>
		/// <returns></returns>
		private static Dictionary<string, string> MergeAdditionKeyValuePairsIntoExplicitKeyValuePairs(
			Dictionary<string, string> explicitProperties, Dictionary<string, string> additionalProperties = null)
		{
			Dictionary<string, string> merged = explicitProperties;
			//no need to iterate if additional is null
			if (additionalProperties != null)
			{
				merged =
					explicitProperties.Concat(additionalProperties.Where(add => !explicitProperties.ContainsKey(add.Key)))
										 .ToDictionary(final => final.Key, final => final.Value);
			}
			return merged;
		}
	}

    public static class OAuth2Constants
    {
        public const string GrantType = "grant_type";
        public const string UserName = "username";
        public const string Scope = "scope";
        public const string Assertion = "assertion";
        public const string Password = "password";
        public const string Code = "code";
        public const string RedirectUri = "redirect_uri";
        public const string ClientId = "client_id";
        public const string ClientSecret = "client_secret";

        public static class GrantTypes
        {
            public const string Password = "password";
            public const string AuthorizationCode = "authorization_code";
            public const string ClientCredentials = "client_credentials";
            public const string RefreshToken = "refresh_token";
            public const string JWT = "urn:ietf:params:oauth:grant-type:jwt-bearer";
            public const string Saml2 = "urn:ietf:params:oauth:grant-type:saml2-bearer";
        }

        public static class ResponseTypes
        {
            public const string Token = "token";
            public const string Code = "code";
        }

        public static class Errors
        {
            public const string Error = "error";
            public const string InvalidRequest = "invalid_request";
            public const string InvalidClient = "invalid_client";
            public const string InvalidGrant = "invalid_grant";
            public const string UnauthorizedClient = "unauthorized_client";
            public const string UnsupportedGrantType = "unsupported_grant_type";
            public const string UnsupportedResponseType = "unsupported_response_type";
            public const string InvalidScope = "invalid_scope";
            public const string AccessDenied = "access_denied";
        }
    }

    public class AccessTokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        //Gets the point in time in which the Access Token returned in the AccessToken property ceases to be valid.  
        //This value is calculated based on current UTC time measured locally and the value expiresIn received from the service.
        public DateTime ExpiresOn { get; set; }
    }
}