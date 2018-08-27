using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using Common.Lib.Common.Enums;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Domain.Common.Services;
using Common.Lib.Interfaces;
using Common.Lib.Mapping;
using Common.Lib.Security;
using Common.NeoNova.Services;
using Newtonsoft.Json.Linq;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace ANDP.Provisioning.API.Rest.Controllers
{
    /// <summary>
    /// This controller handles all MetaSwitch requests..
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/neonova")]
    public class NeoNovaController : ApiController
    {
        private readonly Oauth2AuthenticationSettings _oauth2AuthenticationSettings;
        private readonly ILogger _logger;
        private ICommonMapper _commonMapper;
        private Guid _tenantId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApMaxController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="commonMapper">The mapper.</param>
        /// <param name="oauth2AuthenticationSettings">The oauth2 authentication settings.</param>
        public NeoNovaController(ILogger logger, ICommonMapper commonMapper, Oauth2AuthenticationSettings oauth2AuthenticationSettings)
        {
            _commonMapper = commonMapper;
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
            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

            const string result = "OK";

            _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { result }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response(" + HttpStatusCode.OK + ")."), LogLevelType.Info);

            return this.Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Posts the message.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        [Route("postmessage/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage PostMessage([FromBody]JObject json)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                if (json == null)
                    throw new Exception("Did not receive an json message in the post body.");

                var queryItems = Request.RequestUri.ParseQueryString();
                if (queryItems["equipmentId"] == null)
                    throw new Exception("EquipmentId is a required parameter.");

                int equipmentId;
                int.TryParse(queryItems["equipmentId"], out equipmentId);

                var equipmentConnectionString = Setup(equipmentId);
                if (equipmentConnectionString == null)
                    throw new Exception("Could not find and connection settings for the equipmentId.");

                var service = new NeoNovaService(equipmentConnectionString.Url, equipmentConnectionString.Username, equipmentConnectionString.Password, equipmentConnectionString.CustomString1);
                var response = service.PostMessage(json.ToString());
                var obj = JObject.Parse(response);
                return this.Request.CreateResponse(HttpStatusCode.OK, obj);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }
    }
}
