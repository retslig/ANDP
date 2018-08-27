using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using Thinktecture.IdentityModel.Tokens.Http;

namespace Common.Lib.Security
{
    public static class ClaimsWebApiHelper
    {
        public static void Authenticate(string baseUrl, string token)
        {
            using (var handler = new WebRequestHandler())
            {
                handler.ServerCertificateValidationCallback =
                    CertificateHelper.ServerCertificateValidationCallbackAllowAll;

                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(baseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Basic Auth Way
                    //httpClient.SetBasicAuthentication();

                    //Token Way
                    httpClient.SetBearerToken(token);

                    var response = httpClient.GetAsync("api/authentication/authenticate/").Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new AuthenticationException(response.Content.ReadAsStringAsync().Result)
                        {
                            StatusCode = HttpStatusCode.Unauthorized,
                            ReasonPhrase = "Invalid Credentials"
                        };
                    }
                }
            }
        }

        public static void Authenticate(string baseUrl, string tenantAndUsername, string password)
        {
            using (var handler = new WebRequestHandler())
            {
                handler.ServerCertificateValidationCallback =
                    CertificateHelper.ServerCertificateValidationCallbackAllowAll;

                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(baseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Basic Auth Way
                    httpClient.SetBasicAuthentication(tenantAndUsername, password);

                    //Token Way
                    //httpClient.SetBearerToken(token);

                    var response = httpClient.GetAsync("api/authentication/authenticate/").Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new AuthenticationException(response.Content.ReadAsStringAsync().Result)
                        {
                            StatusCode = HttpStatusCode.Unauthorized,
                            ReasonPhrase = "Invalid Credentials"
                        };
                    }
                }
            }
        }

        public static IEnumerable<Claim> GetClaims(Oauth2AuthenticationSettings oauth2AuthenticationSettings, string token)
        {
            using (var handler = new WebRequestHandler())
            {
                handler.ServerCertificateValidationCallback = CertificateHelper.ServerCertificateValidationCallbackAllowAll;

                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(oauth2AuthenticationSettings.Url);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Basic Auth Way
                    //httpClient.SetBasicAuthentication();

                    //Token Way
                    httpClient.SetBearerToken(token);

                    var response = httpClient.GetAsync("api/authentication/claims/").Result;
                    var result = response.Content.ReadAsStringAsync().Result;

                    if (string.IsNullOrEmpty(result))
                        throw new Exception("Could not find claims for user: " + oauth2AuthenticationSettings.Username);

                    var jobject = JObject.Parse("{\"wrapper\":" + result + "}");

                    var claims = new List<Claim>();
                    foreach (var obj in jobject["wrapper"])
                    {
                        claims.Add(new Claim(obj["m_type"].ToString(),
                            obj["m_value"].ToString(),
                            obj["m_valueType"].ToString(),
                            obj["m_issuer"].ToString(),
                            obj["m_originalIssuer"].ToString()));
                    }

                    claims.Add(new Claim(ClaimsConstants.TenantNameClaimType, oauth2AuthenticationSettings.TenantName));
                    claims.Add(new Claim(ClaimsConstants.UserNameWithoutTenant, oauth2AuthenticationSettings.Username));
                    return claims;
                }
            }
        }


        public static IEnumerable<Claim> GetClaims(Oauth2AuthenticationSettings oauth2AuthenticationSettings, string tenantAndUsername, string password)
        {
            using (var handler = new WebRequestHandler())
            {
                handler.ServerCertificateValidationCallback = CertificateHelper.ServerCertificateValidationCallbackAllowAll;

                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(oauth2AuthenticationSettings.Url);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Basic Auth Way
                    httpClient.SetBasicAuthentication(tenantAndUsername, password);

                    //Token Way
                    //httpClient.SetBearerToken(token);

                    var response = httpClient.GetAsync("api/authentication/claims/").Result;
                    var result = response.Content.ReadAsStringAsync().Result;

                    if (string.IsNullOrEmpty(result))
                        throw new Exception("Could not find claims for user: " + oauth2AuthenticationSettings.Username);

                    var jobject = JObject.Parse("{\"wrapper\":" + result + "}");

                    var claims = new List<Claim>();
                    foreach (var obj in jobject["wrapper"])
                    {
                        claims.Add(new Claim(obj["m_type"].ToString(),
                            obj["m_value"].ToString(),
                            obj["m_valueType"].ToString(),
                            obj["m_issuer"].ToString(),
                            obj["m_originalIssuer"].ToString()));
                    }

                    claims.Add(new Claim(ClaimsConstants.TenantNameClaimType, oauth2AuthenticationSettings.TenantName));
                    claims.Add(new Claim(ClaimsConstants.UserNameWithoutTenant, oauth2AuthenticationSettings.Username));
                    return claims;
                }
            }
        }
    }
}
