using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;
using Common.Lib.Common.Enums;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Domain.Common.Services;
using Common.Lib.Interfaces;
using Common.Lib.Security;
using Common.Pannaway;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace ANDP.Provisioning.API.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/pannaway")]
    public class PannawayController : ApiController
    {
        private readonly Oauth2AuthenticationSettings _oauth2AuthenticationSettings;
        private readonly ILogger _logger;
        private Guid _tenantId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApMaxController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="oauth2AuthenticationSettings">The oauth2 authentication settings.</param>
        public PannawayController(ILogger logger, Oauth2AuthenticationSettings oauth2AuthenticationSettings)
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
            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

            const string result = "OK";

            _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { result }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response(" + HttpStatusCode.OK + ")."), LogLevelType.Info);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Sends the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("postmessage")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage PostMessage(HttpRequestMessage request)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var jsonString = request.Content.ReadAsStringAsync().Result;
                var requestObject = JObject.Parse(jsonString);
                int equipmentId = Convert.ToInt32(requestObject["pannawayRequest"]["equipmentId"]);
                removeFields(requestObject["pannawayRequest"], new[] { "equipmentId" });
                var equipmentConnectionSetting = Setup(equipmentId);

                var service = new PannawayService(_tenantId, equipmentConnectionSetting, _logger);
                var resultObject = service.PostMessage(requestObject["pannawayRequest"]);
                //_logger.WriteLogEntry(_tenantId.ToString(), new List<object> { resultObject, requestObject }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.OK), LogLevelType.Info);

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
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("postmessagewithfakeresponse")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage PostMessageWithFakeResponse(HttpRequestMessage request)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var jsonString = request.Content.ReadAsStringAsync().Result;
                var requestObject = JObject.Parse(jsonString);

                var resultObject = JObject.Parse("{\"pannawayRequest\":{\"equipmentId\":\"4\",\"response\":{\"@id\":\"183\",\"@xmlns\":\"Manogia\",\"taskId\":\"0\",\"status\":{\"@level\":\"1\",\"@code\":\"-1\",\"@xmlns\":\"Manogia\",\"code\":\"-1\",\"lang\":\"en\"},\"msgElement\":{\"@xmlns\":\"Manogia\",\"overwrite\":\"false\",\"status\":{\"@level\":\"1\",\"@code\":\"-1\",\"@xmlns\":\"Manogia\",\"code\":\"-1\",\"lang\":\"en\"},\"totalResultCount\":\"-1\",\"emsData\":{\"basPhonePortConfig\":{\"@xmlns\":\"Pannaway\",\"contextIdElement\":{\"@xmlns\":\"Manogia\",\"id\":\"0\",\"classification\":\"0\",\"groupId\":\"0\",\"templateType\":\"0\",\"type\":\"Pannaway.BASPhonePortConfig\"},\"timeStamp\":\"-1\",\"addedTimeStamp\":\"-1\",\"port\":\"40\",\"portName\":null,\"subscriberState\":\"DISABLED\",\"serviceType\":\"2\",\"serviceLocked\":\"false\",\"subscriberName\":null,\"primaryNumber\":null,\"secondaryNumber\":null,\"sipPrimaryUserName\":null,\"sipSecondaryUserName\":null,\"lifeLine\":\"false\",\"dialPlan\":\"northamerica\",\"primaryRegistrationStatus\":null,\"secondaryRegistrationStatus\":null,\"dialPlanList\":{\"entry\":[{\"@index\":\"0\",\"#text\":\"northamerica\"},{\"@index\":\"1\",\"#text\":\"northamerica-access-8\"},{\"@index\":\"2\",\"#text\":\"northamerica-access-9\"}]},\"sipRealm\":null,\"sipUserName\":null,\"sipPassword\":null,\"callerId\":\"false\",\"callerIdCallWaiting\":\"false\",\"callTransfer\":\"false\",\"callWaiting\":\"false\",\"distinctiveRing\":\"false\",\"doNotDisturb\":\"false\",\"msgWaitingIndicator\":\"false\",\"threeWayCalling\":\"false\",\"warmLine\":\"false\",\"warmLineNumber\":null,\"forwardDisconnectInterval\":\"0\",\"timeReleaseInterval\":null,\"jitterBufferSettings\":{\"minimum\":\"20\",\"maximum\":\"80\",\"initial\":\"40\"},\"impedance\":\"2\",\"receiveLoss\":\"0\",\"transmitLoss\":\"0\"}}}}}}");
                //_logger.WriteLogEntry(_tenantId.ToString(), new List<object> { resultObject }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.OK), LogLevelType.Info);

                return Request.CreateResponse(HttpStatusCode.OK, resultObject.ToString(Newtonsoft.Json.Formatting.None));
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

        private void removeFields(JToken token, string[] fields)
        {
            var container = token as JContainer;
            if (container == null) return;

            var removeList = new List<JToken>();
            foreach (var el in container.Children())
            {
                var p = el as JProperty;
                if (p != null && fields.Contains(p.Name))
                {
                    removeList.Add(el);
                }
                removeFields(el, fields);
            }

            foreach (var el in removeList)
            {
                el.Remove();
            }
        }
    }
}
