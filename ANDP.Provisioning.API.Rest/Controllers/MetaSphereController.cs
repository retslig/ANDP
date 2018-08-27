using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using ANDP.Provisioning.API.Rest.Models.MetaSphere;
using Common.Lib.Common.Enums;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Domain.Common.Services;
using Common.Lib.Interfaces;
using Common.Lib.Security;
using Common.MetaSphere;
using Common.MetaSphere.Services;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace ANDP.Provisioning.API.Rest.Controllers
{
    /// <summary>
    /// This controller handles all MetaSphere requests...
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/metasphere")]
    public class MetaSphereController : ApiController
    {
        private readonly Oauth2AuthenticationSettings _oauth2AuthenticationSettings;
        private readonly ILogger _logger;
        private Guid _tenantId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApMaxController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="oauth2AuthenticationSettings">The oauth2 authentication settings.</param>
        public MetaSphereController(ILogger logger, Oauth2AuthenticationSettings oauth2AuthenticationSettings)
        {
            _logger = logger;
            _oauth2AuthenticationSettings = oauth2AuthenticationSettings;
        }

        /// <summary>
        /// The reason this is not in the constructor is solely for the purpose of exception handling.
        /// If you leave this in the controller and someone who is not authenticated calls the API you will not get a tenantId not found error.
        /// The error will be ugly and be hard to figure out you are not authorized.
        /// This way if the all methods have the ClaimsAuthorize attribute on them they will first be authenticated if not get a nice error message of not authorized.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">No Tenant Id Found.</exception>
        private EquipmentConnectionSetting Setup(int equipmentId)
        {
            //Get the current claims principal
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var tenant = identity.Claims.Where(c => c.Type == ClaimsConstants.TenantIdClaimType).Select(c => c.Value).SingleOrDefault();
            _oauth2AuthenticationSettings.Username = identity.Claims.Where(c => c.Type == ClaimsConstants.UserNameWithoutTenant).Select(c => c.Value).SingleOrDefault();
            _oauth2AuthenticationSettings.TenantName = identity.Claims.Where(c => c.Type == ClaimsConstants.TenantNameClaimType).Select(c => c.Value).SingleOrDefault();

            if (string.IsNullOrEmpty(tenant))
                throw new Exception("No Tenant Id Found.");

            _tenantId = Guid.Parse(tenant);
            return EquipmentConnectionSettingsService.RetrieveProvisioningEquipmentSettings(equipmentId, _oauth2AuthenticationSettings).EquipmentConnectionSettings;
        }

        /// <summary>
        /// Tests this instance.
        /// </summary>
        /// <returns></returns>
        [Route("test")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage Test()
        {
            const string result = "OK";
            return this.Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Retrieves the equipment connection settings.
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveEquipmentConnectionSettings(int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionSetting = Setup(equipmentId);

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { equipmentConnectionSetting }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response(" + HttpStatusCode.OK + ")."), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, equipmentConnectionSetting);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response.", equipmentId), LogLevelType.Error, ex);
                throw;
            }
        }


        /// <summary>
        /// Pulls the specified user identity.
        /// </summary>
        /// <param name="userIdentity">The user identity.</param>
        /// <param name="serviceIndication">The service indication.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// </exception>
        [Route("pull/useridentity/{userIdentity}/serviceindication/{serviceIndication}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage Pull(string userIdentity, string serviceIndication, int equipmentId)
        {
            try
            {
                if (string.IsNullOrEmpty(userIdentity))
                {
                    throw new Exception(MethodBase.GetCurrentMethod().Name + ": userIdentity is required and was not supplied.");
                }

                if (string.IsNullOrEmpty(serviceIndication))
                {
                    throw new Exception(MethodBase.GetCurrentMethod().Name + ": serviceIndication is required and was not supplied.");
                }

                if (equipmentId < 1)
                {
                    throw new Exception(MethodBase.GetCurrentMethod().Name + ": equipmentId is required and was not supplied.");
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { userIdentity, serviceIndication, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionSetting = Setup(equipmentId);

                tExtendedResult extendedResult;
                tUserData userData;
                var resultCode = new MetaSphereService(equipmentConnectionSetting, _logger).ShPull(userIdentity, serviceIndication, out extendedResult, out userData);

                if (resultCode == 5001 && userData == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, userData);

            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }


        /// <summary>
        /// Update subscriber.
        /// </summary>
        /// <returns></returns>
        [Route("update/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage Update([FromBody] MetaSphereUpdate update)
        {
            try
            {
                if (update == null)
                {
                    throw new Exception(MethodBase.GetCurrentMethod().Name + ": update is required and was not supplied.");
                }

                if (update.UserData == null)
                {
                    throw new Exception(MethodBase.GetCurrentMethod().Name + ": update.UserData is required and was not supplied.");
                }

                if (string.IsNullOrEmpty(update.Dn))
                {
                    throw new Exception(MethodBase.GetCurrentMethod().Name + ": update.PhoneNumber is required and was not supplied.");
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called." + Environment.NewLine + update), LogLevelType.Info);

                var equipmentConnectionSetting = Setup(update.EquipmentId);

                if (equipmentConnectionSetting == null)
                {
                    throw new Exception(MethodBase.GetCurrentMethod().Name + ": Unable to retrieve equipment settings on equipment id " + update.EquipmentId + ".");
                }

                if (string.IsNullOrEmpty(equipmentConnectionSetting.Url))
                {
                    throw new Exception(MethodBase.GetCurrentMethod().Name + ": Settings.URL is not populated for equipment id" + update.EquipmentId + ".");
                }

                //Since this is generic object serialize does not now how serialize it.
                //string json = Newtonsoft.Json.JsonConvert.SerializeObject(update.UserData.ShData);
                //var type2 = Type.GetType(string.Concat("Common.MetaSwitch.t", update.UserData.ShData.ItemElementName, ", Common.Lib.Integration"), true);
                //update.UserData.ShData.RepositoryData.ServiceData.Item.Item = Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);

                //Since this is generic object serialize does not now how serialize it.
                //assuming theres exactly 1 shdata.
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(update.UserData.ShData.First().ServiceData.Item.Item);
                var type = Type.GetType(string.Concat("Common.MetaSphere.t" + update.UserData.ShData.First().ServiceData.Item.ItemElementName, ", Common.Lib.Integration"), true);
                update.UserData.ShData.First().ServiceData.Item.Item = Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);

                var metaSphereService = new MetaSphereService(equipmentConnectionSetting, _logger);

                var validationErrors = metaSphereService.Validate(update.UserData);
                if (validationErrors.Any())
                {
                    var sb = new StringBuilder();
                    foreach (var validationError in validationErrors)
                    {
                        sb.Append(String.Format("{0}:{1}", validationError.Key, validationError.Value));
                        sb.AppendLine();
                    }
                    throw new Exception(MethodBase.GetCurrentMethod().Name + ":" + Environment.NewLine + sb);
                }

                tExtendedResult extendedResult;
                metaSphereService.ShUpdate(update.Dn, update.UserData, out extendedResult, true);

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.OK), LogLevelType.Info);

                return this.Request.CreateResponse(HttpStatusCode.OK, extendedResult);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        ///// <summary>
        ///// Delete subscriber by dn and equipment Id.
        ///// </summary>
        ///// <returns></returns>
        //[Route("subscriber/dn/{dn}/equipment/{equipmentId}")]
        //[HttpDelete]
        //[ClaimsAuthorize]
        //public HttpResponseMessage DeleteSubscriber(string dn, int equipmentId)
        //{
        //    try
        //    {
        //        _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { dn, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

         //       var equipmentConnectionString = Setup(equipmentId);
        //        new MetaSphereService(equipmentConnectionString, _logger).DeleteSubscriber(dn);

        //        _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.OK), LogLevelType.Info);
        //        return this.Request.CreateResponse(HttpStatusCode.OK);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
        //        throw;
        //    }
        //}

    }
}
