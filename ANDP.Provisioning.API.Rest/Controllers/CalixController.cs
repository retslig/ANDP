/*
 * Notes:
 * The soap login requests for e7 and c7 look to be the same; however, the rest of the commands/replies seem to be slightly different.
 * Leaving as 1 calix controll for the time being until I get further along with the project then will probably have to split into
 * 2 different prov api controllers and 2 different services.
 
 * Need to redeploy the prov api. Then change the login to the new way to make sure it works.
 * 
 * 
*/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.UI.WebControls;
using System.Xml;
using ANDP.Provisioning.API.Rest.Infrastructure;
using ANDP.Provisioning.API.Rest.Models.Calix;
using Common.Calix;
using Common.Lib.Common.Enums;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Domain.Common.Services;
using Common.Lib.Interfaces;
using Common.Lib.Mapping;
using Common.Lib.Security;
using Common.NeoNova.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Thinktecture.IdentityModel.Authorization.WebApi;
using Formatting = Newtonsoft.Json.Formatting;

namespace ANDP.Provisioning.API.Rest.Controllers
{
    /// <summary>
    /// This controller handles all MetaSwitch requests..
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/calix")]
    public class CalixController : ApiController
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
        public CalixController(ILogger logger, ICommonMapper commonMapper, Oauth2AuthenticationSettings oauth2AuthenticationSettings)
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

        [Route("login/equipment/{equipmentId}")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage Login(int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionSetting = Setup(equipmentId);

                JObject response = new CalixService(_tenantId, equipmentConnectionSetting, _logger).Login();

                // To convert an XML node contained in string xml into a JSON string   
                //var doc = new XmlDocument();
                //doc.LoadXml(response);
                //var jsonResponse = JsonConvert.SerializeXmlNode(doc);
                //var myObject = JsonConvert.DeserializeObject(jsonResponse);

                // To convert JSON text contained in string json into an XML node
                //XmlDocument doc = JsonConvert.DeserializeXmlNode(json);

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { response }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.OK), LogLevelType.Info);

                return this.Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        [Route("logout/equipment/{equipmentId}/session/{sessionId}")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage Logout(int equipmentId, int sessionId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionSetting = Setup(equipmentId);

                var response = new CalixService(_tenantId, equipmentConnectionSetting, _logger).Logout(sessionId);

                //var doc = new XmlDocument();
                //doc.LoadXml(response);
                //var jsonResponse = JsonConvert.SerializeXmlNode(doc);
                //var myObject = JsonConvert.DeserializeObject(jsonResponse);

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { response }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.OK), LogLevelType.Info);

                return this.Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Posts the message.
        /// </summary>
        /// <returns></returns>
        [Route("postmessage/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage PostMessage([FromBody]ProvisioningPostMessage message)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                if (message == null)
                    throw new Exception("Did not receive an json message in the post body.");

                var equipmentConnectionSetting = Setup(message.EquipmentId);
                if (equipmentConnectionSetting == null)
                    throw new Exception("Could not find and connection settings for the equipmentId.");

                var service = new CalixService(_tenantId, equipmentConnectionSetting, _logger);
                var response = service.PostMessageRaw(message.SoapMessage);

                //var doc = new XmlDocument();
                //doc.LoadXml(response);
                //var jsonResponse = JsonConvert.SerializeXmlNode(doc);
                //var myObject = JsonConvert.DeserializeObject(jsonResponse);

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { response }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.OK), LogLevelType.Info);

                return this.Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }


        /// <summary>
        /// Posts the message.
        /// </summary>
        /// <returns></returns>
        [Route("postJsonMessage/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage PostJsonMessage(HttpRequestMessage request)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                if (request == null)
                    throw new Exception("Did not receive an json message in the post body.");

                var jsonString = request.Content.ReadAsStringAsync().Result;
                var requestObject = JObject.Parse(jsonString);
                int equipmentId = Convert.ToInt32(requestObject["calixRequest"]["equipmentId"]);
                var equipmentConnectionSetting = Setup(equipmentId);

                if (equipmentConnectionSetting == null)
                    throw new Exception("Could not find and connection settings for the equipmentId.");

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { equipmentId }, string.Format("EquipmentId."), LogLevelType.Info);

                var service = new CalixService(_tenantId, equipmentConnectionSetting, _logger);

                _logger.WriteLogEntry(_tenantId.ToString(), null, "got here", LogLevelType.Info);

                var resultObject = service.PostMessage(requestObject["calixRequest"]["request"]);

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { resultObject }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.OK), LogLevelType.Info);

                return Request.CreateResponse(HttpStatusCode.OK, resultObject);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        [Route("xmltojsonrequest")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage XmlToJsonRequest(HttpRequestMessage req)
        {
            try
            {
                string xml = req.Content.ReadAsStringAsync().Result;

                var doc = new XmlDocument();
                doc.LoadXml(xml);
                string jsonText = JsonConvert.SerializeXmlNode(doc);

                //<pannawayRequest>
                //    <equipmentId>5</equipmentId>
                //    <request taskId="1" xmlns="Manogia" id="183">
                //        <subsystem>config</subsystem>
                //        <operation>list</operation>
                //        <toIP>10.103.1.4</toIP>
                //        <requireRspElement>true</requireRspElement>
                //        <adapterKey>Pannaway.BASPhonePortConfig2</adapterKey>
                //        <msgElement xmlns="Manogia">
                //            <status level="1" code="-1" xmlns="Manogia">
                //                <code>0</code>
                //                <lang>en</lang>
                //            </status>
                //            <emsData>
                //                <emsString xmlns="Manogia">Pannaway.BASPhonePortConfig.23</emsString>
                //            </emsData>
                //        </msgElement>
                //    </request>
                //</pannawayRequest>

                return Request.CreateResponse(HttpStatusCode.OK, jsonText);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        [Route("jsontoxmlrequest")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage JosnToXmlRequest(HttpRequestMessage req)
        {
            try
            {
                string jsonString = req.Content.ReadAsStringAsync().Result;
                XmlDocument doc = JsonConvert.DeserializeXmlNode(jsonString);
                string xml = doc.OuterXml;

                //{
                //  "pannawayRequest": {
                //    "equipmentId": "5",
                //    "request": {
                //      "@taskId": "1",
                //      "@xmlns": "Manogia",
                //      "@id": "183",
                //      "subsystem": "config",
                //      "operation": "list",
                //      "toIP": "10.103.1.4",
                //      "requireRspElement": "true",
                //      "adapterKey": "Pannaway.BASPhonePortConfig2",
                //      "msgElement": {
                //        "@xmlns": "Manogia",
                //        "status": {
                //          "@level": "1",
                //          "@code": "-1",
                //          "@xmlns": "Manogia",
                //          "code": "0",
                //          "lang": "en"
                //        },
                //        "emsData": {
                //          "emsString": {
                //            "@xmlns": "Manogia",
                //            "#text": "Pannaway.BASPhonePortConfig.23"
                //          }
                //        }
                //      }
                //    }
                //  }
                //}
                return Request.CreateResponse(HttpStatusCode.OK, xml);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        ///// <summary>
        ///// Posts the message.
        ///// </summary>
        ///// <param name="json">The json.</param>
        ///// <returns></returns>
        //[Route("postmessage/")]
        //[HttpPost]
        //[ClaimsAuthorize]
        //public HttpResponseMessage PostMessage([FromBody]JObject json)
        //{
        //    try
        //    {
        //        _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

        //        if (json == null)
        //            throw new Exception("Did not receive an json message in the post body.");

        //        var queryItems = Request.RequestUri.ParseQueryString();
        //        if (queryItems["equipmentId"] == null)
        //            throw new Exception("EquipmentId is a required parameter.");

        //        int equipmentId;
        //        int.TryParse(queryItems["equipmentId"], out equipmentId);

        //        var equipmentConnectionSetting = Setup(equipmentId);
        //        if (equipmentConnectionSetting == null)
        //            throw new Exception("Could not find and connection settings for the equipmentId.");

        //        var service = new CalixService(_tenantId, equipmentConnectionSetting, _logger);
        //        var response = service.PostMessage(json.ToString());
        //        var obj = JObject.Parse(response);
        //        return this.Request.CreateResponse(HttpStatusCode.OK, obj);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
        //        throw;
        //    }
        //}
    }
}
