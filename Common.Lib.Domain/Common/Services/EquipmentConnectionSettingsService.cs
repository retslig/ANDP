using System;
using System.IdentityModel.Tokens;
using System.Net.Http;
using System.Net.Http.Headers;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Security;
using Newtonsoft.Json.Linq;

namespace Common.Lib.Domain.Common.Services
{
    public static class EquipmentConnectionSettingsService
    {
        /// <summary>
        /// Retrieves the equipment connection settings.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <param name="authSettings">The authentication settings.</param>
        /// <returns></returns>
        public static ProvisioningEquipment RetrieveProvisioningEquipmentSettings(int equipmentId, Oauth2AuthenticationSettings authSettings)
        {
            var accessTokenResponse = BearerTokenHelper.RetrieveBearTokenFromCache(authSettings);
            return GetEquipmentConnectionSettings(equipmentId, authSettings.Url, accessTokenResponse);
        }

        private static ProvisioningEquipment GetEquipmentConnectionSettings(int equipmentId, string url, AccessTokenResponse accessTokenResponse)
        {
            using (var handler = new WebRequestHandler())
            {
                handler.ServerCertificateValidationCallback = CertificateHelper.ServerCertificateValidationCallback;

                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Basic Auth Way
                    //httpClient.SetBasicAuthentication(username, password);

                    //Token Way
                    httpClient.SetBearerToken(accessTokenResponse.AccessToken);

                    var response = httpClient.GetAsync("api/equipment/equipmentid/" + equipmentId).Result;
                    var result = response.Content.ReadAsStringAsync().Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        var json = JObject.Parse(result);

                        if (json["ExceptionMessage"] != null)
                        {
                            throw new Exception(json["ExceptionMessage"].ToString()); 
                        }

                        throw new Exception("Error retrieving Equipment Connection Settings: " + result); 
                    }

                    if (string.IsNullOrEmpty(result))
                        throw new Exception("Could not find equipment for this equipmentId: " + equipmentId);

                    //Check if Catch Token expired error.
                    if (result.Contains("SecurityTokenExpiredException"))
                        throw new SecurityTokenExpiredException();

                    var equipment = Newtonsoft.Json.JsonConvert.DeserializeObject<ProvisioningEquipment>(result);
                    return equipment;
                }
            }
        }
    }
}