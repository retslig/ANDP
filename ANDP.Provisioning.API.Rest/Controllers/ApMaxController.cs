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
using ANDP.Provisioning.API.Rest.Models.ApMax;
using Common.ApMax;
using Common.Lib.Common.Enums;
using Common.Lib.Domain.Common.Models;
using Common.Lib.Domain.Common.Services;
using Common.Lib.Infastructure;
using Common.Lib.Interfaces;
using Common.Lib.Mapping;
using Common.Lib.Security;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace ANDP.Provisioning.API.Rest.Controllers
{
    /// <summary>
    /// This controller handles all ApMax requests..
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/apmax")]
    public class ApMaxController : ApiController
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
        public ApMaxController(ILogger logger, ICommonMapper commonMapper, Oauth2AuthenticationSettings oauth2AuthenticationSettings)
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

            _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { result }, 
                string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response(" + HttpStatusCode.OK + ")."), LogLevelType.Info);

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

                var equipmentConnectionString = Setup(equipmentId);

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { equipmentConnectionString }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response(" + HttpStatusCode.OK + ")."), LogLevelType.Info);

                return this.Request.CreateResponse(HttpStatusCode.OK, equipmentConnectionString);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response.", equipmentId), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all versions.
        /// </summary>
        /// <returns></returns>
        [Route("versions/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveVersions(int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);

                var apmaxVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var serviceVersions = ObjectFactory.CreateInstanceAndMap<Common.ServiceReport.ServiceVersions, ServiceVersions>(_commonMapper, apmaxVersions);

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { serviceVersions },string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response(" + HttpStatusCode.OK + ")."), LogLevelType.Info);

                return this.Request.CreateResponse(HttpStatusCode.OK, serviceVersions);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response"), LogLevelType.Error, ex);
                throw;
            }
        }

        #region ************************** Subscriber info *******************************

        /// <summary>
        /// Retrieve the subscriber by billing account number and billing service id.
        /// </summary>
        /// <param name="billingServiceId"></param>
        /// <param name="equipmentId">The company identifier.</param>
        /// <param name="billingAccount"></param>
        /// <returns></returns>
        [Route("subscriber/billingaccount/{billingAccount}/billingserviceid/{billingserviceid}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveSubscriberByBilling(string billingAccount, string billingServiceId, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { billingAccount, billingServiceId, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var subscriberVersion = serviceVersions.Subscriber;

                SubscriberType result;

                switch (subscriberVersion)
                {
                    //Not available before 4.

                    case 4:
                        var subscriberV4Service = new SubscriberV4Service(equipmentConnectionString, serviceVersions);
                        var subscriberV4 = subscriberV4Service.RetrieveSubscriberByBillingAccount(billingAccount, billingServiceId);
                        result = ObjectFactory.CreateInstanceAndMap<Common.SubscriberV4.SubscriberType, SubscriberType>(_commonMapper, subscriberV4);
                        break;
                    default:
                        _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Subscriber version " + subscriberVersion + " not implemented."), LogLevelType.Error);
                        return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented."));
                }

                if (result == null)
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { result }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieve the subscriber by phone number.
        /// </summary>
        /// <param name="phoneNumber">The subscriber phone number.</param>
        /// <param name="equipmentId">The company identifier.</param>
        /// <returns></returns>
        [Route("subscriber/phone/{phoneNumber}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveSubscriberByDefaultPhoneNumber(string phoneNumber, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { phoneNumber, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);

                //ToDo: making sure the service versions mapping is working and if so then get rid of the dictionary.
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var subscriberVersion = serviceVersions.Subscriber;

                SubscriberType result;

                switch (subscriberVersion)
                {
                    case 3:
                        var subscriberV3Service = new SubscriberV3Service(equipmentConnectionString, serviceVersions);
                        var subscriberV3 = subscriberV3Service.RetrieveSubscriberByDefaultPhoneNumber(phoneNumber);
                        result = ObjectFactory.CreateInstanceAndMap<Common.SubscriberV3.SubscriberType, SubscriberType>(_commonMapper, subscriberV3);
                        break;
                    case 4:
                        var subscriberV4Service = new SubscriberV4Service(equipmentConnectionString, serviceVersions);
                        var subscriberV4 = subscriberV4Service.RetrieveSubscriberByDefaultPhoneNumber(phoneNumber);
                        result = ObjectFactory.CreateInstanceAndMap<Common.SubscriberV4.SubscriberType, SubscriberType>(_commonMapper, subscriberV4);
                        break;
                    default:
                        _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Subscriber version " + subscriberVersion + " not implemented."), LogLevelType.Error);
                        return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented"));
                }

                if (result == null)
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { result }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieve the subscriber by guid.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="equipmentId">The equipment identifier </param>
        /// <returns></returns>
        [Route("subscriber/guid/{guid}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveSubscriberByGuid(string guid, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object>() { guid, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var myGuid = Guid.Parse(guid);
                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var subscriberVersion = serviceVersions.Subscriber;

                SubscriberType result;

                switch (subscriberVersion)
                {
                    case 3:
                        var subscriberV3Service = new SubscriberV3Service(equipmentConnectionString, serviceVersions);
                        var subscriberV3 = subscriberV3Service.RetrieveSubscriberByGuid(myGuid);
                        result = ObjectFactory.CreateInstanceAndMap<Common.SubscriberV3.SubscriberType, SubscriberType>(_commonMapper, subscriberV3);
                        break;
                    case 4:
                        var subscriberV4Service = new SubscriberV4Service(equipmentConnectionString, serviceVersions);
                        var subscriberV4 = subscriberV4Service.RetrieveSubscriberByGuid(myGuid);
                        result = ObjectFactory.CreateInstanceAndMap<Common.SubscriberV4.SubscriberType, SubscriberType>(_commonMapper, subscriberV4);
                        break;
                    default:
                        _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Subscriber version " + subscriberVersion + " not implemented."), LogLevelType.Error);
                        return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented."));
                }

                if (result == null)
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { result }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the subscriber services by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("subscriber/services/guid/{guid}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveSubscriberServicesByGuid(string guid, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object>() { guid, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var myGuid = Guid.Parse(guid);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var subscriberVersion = serviceVersions.Subscriber;

                List<ServiceInfoType> results;

                switch (subscriberVersion)
                {
                    //Not available in vs 3.

                    case 4:
                        var subscriberV4Service = new SubscriberV4Service(equipmentConnectionString, serviceVersions);
                        var subscriberV4 = subscriberV4Service.RetrieveSubscriberServicesByGuid(myGuid);
                        results = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.SubscriberV4.ServiceInfoType>, IEnumerable<ServiceInfoType>>(_commonMapper, subscriberV4).ToList();
                        break;
                    default:
                        _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Subscriber version " + subscriberVersion + " not implemented."), LogLevelType.Error);
                        return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented."));
                }

                if (results == null || !results.Any())
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { results }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the subscriber services by default phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("subscriber/services/phone/{phoneNumber}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveSubscriberServicesByDefaultPhoneNumber(string phoneNumber, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object>() { phoneNumber, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var subscriberVersion = serviceVersions.Subscriber;

                List<ServiceInfoType> results;

                switch (subscriberVersion)
                {
                    //Not available in vs 3.

                    case 4:
                        var subscriberV4Service = new SubscriberV4Service(equipmentConnectionString, serviceVersions);
                        var subscriberV4 = subscriberV4Service.RetrieveSubscriberServicesByPhoneNumber(phoneNumber);
                        results = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.SubscriberV4.ServiceInfoType>, IEnumerable<ServiceInfoType>>(_commonMapper, subscriberV4).ToList();
                        break;
                    default:
                        _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Subscriber version " + subscriberVersion + " not implemented."), LogLevelType.Error);
                        return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented."));
                }

                if (results == null || !results.Any())
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { results }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the subscriber services by billing.
        /// </summary>
        /// <param name="billingAccount">The billing account.</param>
        /// <param name="billingServiceId">The billing service identifier.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("subscriber/services/billingaccount/{billingAccount}/billingserviceid/{billingserviceid}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveSubscriberServicesByBilling(string billingAccount, string billingServiceId, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object>() { billingAccount, billingServiceId, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var subscriberVersion = serviceVersions.Subscriber;

                List<ServiceInfoType> results;

                switch (subscriberVersion)
                {
                    //Not available in vs 3.

                    case 4:
                        var subscriberV4Service = new SubscriberV4Service(equipmentConnectionString, serviceVersions);
                        var subscriberV4 = subscriberV4Service.RetrieveSubscriberServicesByBillingAccount(billingAccount, billingServiceId);
                        results = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.SubscriberV4.ServiceInfoType>, IEnumerable<ServiceInfoType>>(_commonMapper, subscriberV4).ToList();
                        break;
                    default:
                        _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Subscriber version " + subscriberVersion + " not implemented."), LogLevelType.Error);
                        return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented."));
                }

                if (results == null || !results.Any())
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { results }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }



        /// <summary>
        /// Audits the type of the subscriber for service.
        /// </summary>
        /// <param name="offSet">The off set.</param>
        /// <param name="numberOfAccounts">The number of accounts.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Could not determine service type.</exception>
        [Route("subscriber/offSet/{offSet}/numberofaccountstoreturn/{numberOfAccounts}/servicetype/{serviceType}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage AuditSubscriberForServiceType(int offSet, int numberOfAccounts, string serviceType, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { numberOfAccounts, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var subscriberVersion = serviceVersions.Subscriber;
                
                List<string> results;

                Common.SubscriberV4.ServiceType_e serviceTypeEnum;
                if (!Enum.TryParse(serviceType, true, out serviceTypeEnum))
                    throw new Exception("Could not determine service type.");

                switch (subscriberVersion)
                {
                    //Not available in vs 3.

                    case 4:
                        var subscriberV4Service = new SubscriberV4Service(equipmentConnectionString, serviceVersions);
                        results = subscriberV4Service.RetrieveServiceGuidsByServiceTypeForAudit(offSet, numberOfAccounts, serviceTypeEnum);
                        break;
                    default:
                        _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Subscriber version " + subscriberVersion + " not implemented."), LogLevelType.Error);
                        return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Subscriber version " + subscriberVersion + " not implemented."));
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { results }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Creates the subscriber.
        /// </summary>
        /// <param name="subscriber"></param>
        /// <returns></returns>
        [Route("subscriber/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage CreateOrUpdateSubscriber([FromBody]ProvisioningAddUpdateSubscriber subscriber)
        {
            try
            {
                 _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { subscriber }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(subscriber.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Subscriber;
                
                switch (version)
                {
                    case 3:
                        var voicemailServiceV3 = new SubscriberV3Service(equipmentConnectionString, serviceVersions);
                        var apmaxSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<SubscriberType, Common.SubscriberV3.SubscriberType>(_commonMapper, subscriber.SubscriberType);
                        voicemailServiceV3.AddOrUpdateSubscriberProv(apmaxSubscriberTypeV3);
                        break;
                    case 4:
                        var subscriberV4Service = new SubscriberV4Service(equipmentConnectionString, serviceVersions);
                        var apmaxSubscriberTypeV4 = ObjectFactory.CreateInstanceAndMap<SubscriberType, Common.SubscriberV4.SubscriberType>(_commonMapper, subscriber.SubscriberType);
                        subscriberV4Service.AddOrUpdateSubscriberProv(apmaxSubscriberTypeV4);
                        break;
                    default:
                    {
                        _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Subscriber version " + version + " not implemented."), LogLevelType.Error);
                        return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Subscriber version " + version + " not implemented"));
                    }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.Created), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the subscriber.
        /// </summary>
        /// <param name="phoneNumber">Subscriber phone number.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("subscriber/phone/{phoneNumber}/equipment/{equipmentId}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeleteSubscriberByPhoneNumber(string phoneNumber, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object>() { phoneNumber, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Subscriber;

                switch (version)
                {
                    case 3:
                        var subscriberV3Service = new SubscriberV3Service(equipmentConnectionString, serviceVersions);
                        subscriberV3Service.DeleteSubscriberByDefaultPhoneNumber(phoneNumber);
                        break;
                    case 4:
                        var subscriberV4Service = new SubscriberV4Service(equipmentConnectionString, serviceVersions);
                        subscriberV4Service.DeleteSubscriberByDefaultPhoneNumber(phoneNumber);
                        break;
                    default:
                    {
                        _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Subscriber version " + version + " not implemented."), LogLevelType.Error);
                        return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Subscriber version " + version + " not implemented"));
                    }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response"), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the subscriber.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("subscriber/guid/{guid}/equipment/{equipmentId}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeleteSubscriberByGuid(string guid, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { guid, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                switch (version)
                {
                    case 3:
                        var subscriberV3Service = new SubscriberV3Service(equipmentConnectionString, serviceVersions);
                        subscriberV3Service.DeleteSubscriberByGuid(Guid.Parse(guid));
                        break;
                    case 4:
                        var subscriberV4Service = new SubscriberV4Service(equipmentConnectionString, serviceVersions);
                        subscriberV4Service.DeleteSubscriberByGuid(Guid.Parse(guid));
                        break;
                    default:
                    {
                        _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Subscriber version " + version + " not implemented."), LogLevelType.Error);
                        return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Subscriber version " + version + " not implemented"));
                    }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        #endregion

        #region ************************** Voicemail info *******************************

        /// <summary>
        /// Retrieves all voicemail packages.
        /// </summary>
        /// <param name="equipmentId">The company identifier.</param>
        /// <returns></returns>
        [Route("voicemail/packages/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveAllVoicemailPackages(int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object>() { equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                List<PackageType> results;

                switch (version)
                {
                    case 3:
                        var voicemailServiceV3 = new VoicemailV3Service(equipmentConnectionString, serviceVersions);
                        IEnumerable<Common.VoicemailV3.PackageType> voicemailBoxTypeV3 = voicemailServiceV3.RetrieveAllPackages();
                        results = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.VoicemailV3.PackageType>, IEnumerable<PackageType>>(_commonMapper, voicemailBoxTypeV3).ToList();
                        break;
                    default:
                        if (version >= 5)
                        {
                            var voicemailServiceV5 = new VoicemailV5Service(equipmentConnectionString, serviceVersions);
                            IEnumerable<Common.VoicemailV5.PackageType> voicemailBoxTypeV5 = voicemailServiceV5.RetrieveAllPackages();
                            results = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.VoicemailV5.PackageType>, IEnumerable<PackageType>>(_commonMapper, voicemailBoxTypeV5).ToList();
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemail version " + version + " not implemented"));
                        }
                }

                if (results == null || !results.Any())
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { results }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all the notification centers.
        /// </summary>
        /// <param name="equipmentId">The company identifier.</param>
        /// <returns></returns>
        [Route("voicemail/notificationcenters/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveAllNotificationCenters(int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object>() { equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;
                
                List<NotificationCenterInfoType> results;

                switch (version)
                {
                    case 3:
                        var voicemailServiceV3 = new VoicemailV3Service(equipmentConnectionString, serviceVersions);
                        IEnumerable<Common.VoicemailV3.NotificationCenterInfoType> voicemailBoxTypeV3 = voicemailServiceV3.RetrieveAllNotifcationCenters();
                        results = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.VoicemailV3.NotificationCenterInfoType>, IEnumerable<NotificationCenterInfoType>>(_commonMapper, voicemailBoxTypeV3).ToList();
                        break;
                    default:
                        if (version >= 5)
                        {
                            var voicemailServiceV5 = new VoicemailV5Service(equipmentConnectionString, serviceVersions);
                            IEnumerable<Common.VoicemailV5.NotificationCenterInfoType> voicemailBoxTypeV5 = voicemailServiceV5.RetrieveAllNotifcationCenters();
                            results = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.VoicemailV5.NotificationCenterInfoType>,IEnumerable<NotificationCenterInfoType>>(_commonMapper,voicemailBoxTypeV5).ToList();
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemail version " + version + " not implemented"));
                        }
                }

                if (results == null || !results.Any())
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { results }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the version by service.
        /// </summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="equipmentId">The company identifier.</param>
        /// <returns></returns>
        [Route("voicemail/phone/{phoneNumber}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveVoicemail(string phoneNumber, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object>() { phoneNumber, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var voicemailVersion = serviceVersions.Voicemail;

                List<VoiceMailBoxType> results;

                switch (voicemailVersion)
                {
                    case 3:
                        var voicemailServiceV3 = new VoicemailV3Service(equipmentConnectionString, serviceVersions);
                        var voicemailBoxTypeV3 = voicemailServiceV3.RetrieveVoicemail(phoneNumber);
                        results = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.VoicemailV3.VoiceMailBoxType>, IEnumerable<VoiceMailBoxType>>(_commonMapper, voicemailBoxTypeV3).ToList();
                        break;
                    default:
                        if (voicemailVersion >= 5)
                        {
                            var voicemailServiceV5 = new VoicemailV5Service(equipmentConnectionString, serviceVersions);
                            var voicemailBoxTypeV5 = voicemailServiceV5.RetrieveVoicemail(phoneNumber);
                            results = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.VoicemailV5.VoiceMailBoxType>, IEnumerable<VoiceMailBoxType>>(_commonMapper, voicemailBoxTypeV5).ToList();
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + voicemailVersion + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemail version " + voicemailVersion + " not implemented"));
                        }
                        
                }

                if (results == null || !results.Any())
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { results }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null,string.Format(MethodBase.GetCurrentMethod().Name +" in ProvisioningAPI. Response."),LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Disable or Enable the voicemail box
        /// </summary>ram>
        /// <returns></returns>
        [Route("voicemail/disable/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage SuspendVoicemail([FromBody]ProvisioningDisableVoicemail provisioningDisableVoicemail)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { provisioningDisableVoicemail }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(provisioningDisableVoicemail.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                switch (version)
                {
                    default:
                        if (version >= 5)
                        {
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).SuspendVoicemail(
                                provisioningDisableVoicemail.PhoneNumber, provisioningDisableVoicemail.Disable);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemail version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Creates voicemail.
        /// </summary>
        /// <returns></returns>
        [Route("voicemail/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage CreateVoicemail([FromBody]ProvisioningCreateVoicemail voicemailType)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object>{voicemailType}, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(voicemailType.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                switch (version)
                {
                    case 3:
                        new VoicemailV3Service(equipmentConnectionString, serviceVersions).CreateVoicemail(voicemailType.PhoneNumber, voicemailType.Description, voicemailType.VoiceMailPackageName,
                            voicemailType.SubscriberName, voicemailType.SubscriberBillingAccountNumber, (Common.VoicemailV3.MailboxType) voicemailType.MailBoxType, voicemailType.NotificationCenterDescription);
                        break;
                    default:
                        if (version >= 5)
                        {
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).CreateVoicemail(voicemailType.PhoneNumber, voicemailType.Description, voicemailType.VoiceMailPackageName,voicemailType.SubscriberName, voicemailType.SubscriberBillingAccountNumber,(Common.VoicemailV5.MailboxType) voicemailType.MailBoxType,voicemailType.NotificationCenterDescription);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemail version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.Created), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Creates the voicemail sub mailbox.
        /// </summary>
        /// <returns></returns>
        [Route("voicemail/submailbox/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage CreateVoiceMailSubMailbox([FromBody]ProvisioningCreateVoicemailSubMailbox subMailbox)        
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { subMailbox }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(subMailbox.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                switch (version)
                {
                    case 3:
                        new VoicemailV3Service(equipmentConnectionString, serviceVersions).CreateVoiceMailSubMailbox(subMailbox.PhoneNumber, subMailbox.Digit);
                        break;
                    default:
                        if (version >= 5)
                        {
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).CreateVoiceMailSubMailbox(subMailbox.PhoneNumber, subMailbox.Digit);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemail version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.Created), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Adds out dial.
        /// </summary>
        /// <param name="outDialInfo"></param>
        /// <returns></returns>
        [Route("voicemail/outdial/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage AddOutDialNumber([FromBody]ProvisioningAddOutDialNumber outDialInfo)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { outDialInfo }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(outDialInfo.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                switch (version)
                {
                    case 3:
                        new VoicemailV3Service(equipmentConnectionString, serviceVersions).AddOutDialNumber(outDialInfo.PhoneNumber, outDialInfo.OutDialPhoneNumber, outDialInfo.OutDialRoutingNumber);
                        break;
                    default:
                        if (version >= 5)
                        {
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).AddOutDialNumber(outDialInfo.PhoneNumber,
                                outDialInfo.OutDialPhoneNumber, outDialInfo.OutDialRoutingNumber);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemail version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.Created), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes out dial.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="outDialPhoneNumber"></param>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        [Route("voicemail/phone/{phoneNumber}/outdialphonenumber/{outDialPhoneNumber}/equipment/{equipmentId}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeleteOutDialNumber(string phoneNumber, string outDialPhoneNumber, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { phoneNumber, outDialPhoneNumber, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                switch (version)
                {
                    case 3:
                        new VoicemailV3Service(equipmentConnectionString, serviceVersions).DeleteOutDialNumber(phoneNumber, outDialPhoneNumber);
                        break;
                    default:
                        if (version >= 5)
                        {
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).DeleteOutDialNumber(phoneNumber,outDialPhoneNumber);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemail version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the voice mail box with max mail box time and max messages.
        /// </summary>
        /// <returns></returns>
        [Route("voicemail/simple/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateVoicemailSimple([FromBody]ProvisioningUpdateVoicemailSimple voiceMailInfo)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { voiceMailInfo }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(voiceMailInfo.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                switch (version)
                {
                    case 3:
                        new VoicemailV3Service(equipmentConnectionString, serviceVersions).UpdateVoiceMailBox(voiceMailInfo.PhoneNumber, voiceMailInfo.MaxMailBoxtime, voiceMailInfo.MaxMessages);
                        break;
                    default:
                        if (version >= 5)
                        {
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).UpdateVoiceMailBox(
                                voiceMailInfo.PhoneNumber, voiceMailInfo.MaxMailBoxtime, voiceMailInfo.MaxMessages);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemail version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Updates voice mail with all available properties.
        /// </summary>
        /// <param name="updateVoicemail"></param>
        /// <returns></returns>
        [Route("voicemail/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateVoicemailFull([FromBody]ProvisioningUpdateVoicemail updateVoicemail)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { updateVoicemail }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(updateVoicemail.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                switch (version)
                {
                    case 3:
                        var apmaxVoiceMailBoxTypeV3 = ObjectFactory.CreateInstanceAndMap<Models.ApMax.VoiceMailBoxType, Common.VoicemailV3.VoiceMailBoxType>(_commonMapper, updateVoicemail.VoiceMailBoxType);
                        var apmaxInternetAccessTypeV3 = ObjectFactory.CreateInstanceAndMap<Models.ApMax.InternetAccessType, Common.VoicemailV3.InternetAccessType>(_commonMapper, updateVoicemail.InternetAccessType);
                        new VoicemailV3Service(equipmentConnectionString, serviceVersions).UpdateVoiceMailBoxFull(updateVoicemail.PhoneNumber, apmaxVoiceMailBoxTypeV3, apmaxInternetAccessTypeV3); 
                        break;
                    default:
                        if (version >= 5)
                        {
                            var apmaxVoiceMailBoxTypeV5 = ObjectFactory.CreateInstanceAndMap<Models.ApMax.VoiceMailBoxType, Common.VoicemailV5.VoiceMailBoxType>(_commonMapper,updateVoicemail.VoiceMailBoxType);
                            var apmaxInternetAccessTypeV5 = ObjectFactory.CreateInstanceAndMap<Models.ApMax.InternetAccessType, Common.VoicemailV5.InternetAccessType>(_commonMapper, updateVoicemail.InternetAccessType);
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).UpdateVoiceMailBoxFull(updateVoicemail.PhoneNumber, apmaxVoiceMailBoxTypeV5, apmaxInternetAccessTypeV5);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemal version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Updates sub mailbox.
        /// </summary>ram>
        /// <returns></returns>
        [Route("voicemail/submailbox/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateVoiceSubMailbox([FromBody]ProvisioningUpdateVoicemailSubMailBox subMailBox)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { subMailBox }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(subMailBox.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                switch (version)
                {
                    case 3:
                        new VoicemailV3Service(equipmentConnectionString, serviceVersions).UpdateVoiceSubMailbox(subMailBox.PhoneNumber, subMailBox.NumberOfSubMailBoxes, subMailBox.MaxNumberOfSubMailBoxesAllowed);
                        break;
                    default:
                        if (version >= 5)
                        {
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).UpdateVoiceSubMailbox(subMailBox.PhoneNumber, subMailBox.NumberOfSubMailBoxes,subMailBox.MaxNumberOfSubMailBoxesAllowed);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemal version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Updates voicemail box package.
        /// </summary>
        /// <returns></returns>
        [Route("voicemail/boxpackage/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateVoiceMailBoxPackage([FromBody]ProvisioningUpdateVoicemailBoxPackage voiceMailBoxPackageInfo)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { voiceMailBoxPackageInfo }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(voiceMailBoxPackageInfo.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;
                
                switch (version)
                {
                    case 3:
                        new VoicemailV3Service(equipmentConnectionString, serviceVersions).UpdateVoiceMailBoxPackage(voiceMailBoxPackageInfo.PhoneNumber, voiceMailBoxPackageInfo.VoiceMailPackageName);
                        break;
                    default:
                        if (version >= 5)
                        {
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).UpdateVoiceMailBoxPackage(voiceMailBoxPackageInfo.PhoneNumber, voiceMailBoxPackageInfo.VoiceMailPackageName);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemal version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Updates voicemail box type.
        /// </summary>
        /// <returns></returns>
        [Route("voicemail/boxtype/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateVoiceMailBoxType([FromBody]ProvisioningUpdateVoicemailBoxType voiceMailBoxTypeInfo)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { voiceMailBoxTypeInfo }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(voiceMailBoxTypeInfo.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                switch (version)
                {
                    case 3:
                        new VoicemailV3Service(equipmentConnectionString, serviceVersions).UpdateVoiceMailBoxType(voiceMailBoxTypeInfo.PhoneNumber, (Common.VoicemailV3.MailboxType)voiceMailBoxTypeInfo.MailboxType, voiceMailBoxTypeInfo.InternetPassword, voiceMailBoxTypeInfo.InternetUserName, voiceMailBoxTypeInfo.InternetAccess);
                        break;
                    default:
                        if (version >= 5)
                        {
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).UpdateVoiceMailBoxType(voiceMailBoxTypeInfo.PhoneNumber,(Common.VoicemailV5.MailboxType) voiceMailBoxTypeInfo.MailboxType,voiceMailBoxTypeInfo.InternetPassword, voiceMailBoxTypeInfo.InternetUserName,voiceMailBoxTypeInfo.InternetAccess);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemal version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the voicemail..
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="deleteSubscriber"></param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("voicemail/phone/{phoneNumber}/deletesubscriber/{deleteSubscriber}/equipment/{equipmentId}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeleteVoicemail(string phoneNumber, bool deleteSubscriber, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { phoneNumber, deleteSubscriber, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                switch (version)
                {
                    case 3:
                        new VoicemailV3Service(equipmentConnectionString, serviceVersions).DeleteVoiceMailBox(phoneNumber, deleteSubscriber);
                        break;
                    default:
                        if (version >= 5)
                        {
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).DeleteVoiceMailBox(phoneNumber, deleteSubscriber);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemal version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary/>
        /// Adds voicemail box internet access.
        /// <returns></returns>
        [Route("voicemail/internetaccess/")]
        [HttpPost]
        public HttpResponseMessage AddVoiceMailBoxInternetAccess([FromBody]ProvisioningAddVoiceMailBoxInternetAccess voiceMailBoxInternetAccessInfo)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { voiceMailBoxInternetAccessInfo }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(voiceMailBoxInternetAccessInfo.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                switch (version)
                {
                    case 3:
                        new VoicemailV3Service(equipmentConnectionString, serviceVersions).AddVoiceMailBoxInternetAccess(voiceMailBoxInternetAccessInfo.PhoneNumber, voiceMailBoxInternetAccessInfo.EmailAddress, voiceMailBoxInternetAccessInfo.InternetPassword, voiceMailBoxInternetAccessInfo.InternetUserName);
                        break;
                    default:
                        if (version >= 5)
                        {
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).AddVoiceMailBoxInternetAccess(voiceMailBoxInternetAccessInfo.PhoneNumber, voiceMailBoxInternetAccessInfo.EmailAddress,voiceMailBoxInternetAccessInfo.InternetPassword,voiceMailBoxInternetAccessInfo.InternetUserName);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemal version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.Created), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary/>
        /// Deletes the voicemail box internet access.
        /// <param name="phoneNumber"></param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("voicemail/internetaccess/phone/{phoneNumber}/equipment/{equipmentId}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeleteVoiceMailBoxInternetAccess(string phoneNumber, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { phoneNumber, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);
                
                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                switch (version)
                {
                    case 3:
                        new VoicemailV3Service(equipmentConnectionString, serviceVersions).DeleteVoiceMailBoxInternetAccess(phoneNumber);
                        break;
                    default:
                        if (version >= 5)
                        {
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).DeleteVoiceMailBoxInternetAccess(
                                phoneNumber);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemal version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Reassigns voicemail box number.
        /// </summary>
        /// <param name="provisioningReassignVoicemail"></param>
        /// <returns></returns>
        [Route("voicemail/reassign/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage ReassignVmBoxNumber([FromBody]ProvisioningReassignVoicemail provisioningReassignVoicemail)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { provisioningReassignVoicemail }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(provisioningReassignVoicemail.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;
                
                switch (version)
                {
                    case 3:
                        new VoicemailV3Service(equipmentConnectionString, serviceVersions).ReassignVmBoxNumber(provisioningReassignVoicemail.OldPhoneNumber, provisioningReassignVoicemail.NewPhoneNumber, provisioningReassignVoicemail.MailBoxDescription, 
                            provisioningReassignVoicemail.SubscriberName, provisioningReassignVoicemail.InternetPassword, provisioningReassignVoicemail.InternetUserName, provisioningReassignVoicemail.InternetAccess, provisioningReassignVoicemail.DeleteOldSubscriber);
                        break;
                    default:
                        if (version >= 5)
                        {
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).ReassignVmBoxNumber(
                                provisioningReassignVoicemail.OldPhoneNumber,
                                provisioningReassignVoicemail.NewPhoneNumber,
                                provisioningReassignVoicemail.MailBoxDescription,
                                provisioningReassignVoicemail.SubscriberName,
                                provisioningReassignVoicemail.InternetPassword,
                                provisioningReassignVoicemail.InternetUserName,
                                provisioningReassignVoicemail.InternetAccess,
                                provisioningReassignVoicemail.DeleteOldSubscriber);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemal version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI. Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Resets the voicemail pin to the new pin specified.
        /// </summary>
        /// <returns></returns>
        [Route("voicemail/passwordreset/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage VmPasswordReset([FromBody]ProvisioningVoicemailPasswordReset passwordReset)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { passwordReset }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(passwordReset.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Voicemail;

                switch (version)
                {
                    case 3:
                        new VoicemailV3Service(equipmentConnectionString, serviceVersions).VmPasswordReset(passwordReset.PhoneNumber, passwordReset.NewPin);
                        break;
                    default:
                        if (version >= 5)
                        {
                            new VoicemailV5Service(equipmentConnectionString, serviceVersions).VmPasswordReset(
                                passwordReset.PhoneNumber, passwordReset.NewPin);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Voicemail version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Voicemail version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }
        #endregion

        #region ************************** IPTV info *******************************


        /// <summary>
        /// Retrieves all the channel line ups (aka packages
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("iptv/channellineups/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveAllChannelLineups(int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                List<ChannelLineupType> channelLineupTypes;

                switch (version)
                {
                    case 3:
                       var channelLineUpTypesV3 = new IptvV3Service(equipmentConnectionString, serviceVersions).RetrieveAllChannelLineups();
                       channelLineupTypes = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.IPTVServiceV3.ChannelLineupType>, IEnumerable<ChannelLineupType>>(_commonMapper, channelLineUpTypesV3).ToList();
                        break;
                    default:
                        if (version >= 7)
                        {
                            var channelLineUpTypesV7 = new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).RetrieveAllChannelLineups();
                            channelLineupTypes = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.IPTVServiceV7.ChannelLineupType>, IEnumerable<ChannelLineupType>>(_commonMapper, channelLineUpTypesV7).ToList();
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented"));
                        }
                }

                if (channelLineupTypes == null || !channelLineupTypes.Any())
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { channelLineupTypes }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, channelLineupTypes);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the iptv subscriber by mac address
        /// </summary>
        /// <param name="subscriberMac"></param>
        /// <param name="equipmentId">The company identifier.</param>
        /// <returns></returns>
        [Route("iptv/subscribermac/{subscriberMac}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveIptvAccountTypesByMac(string subscriberMac, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { subscriberMac, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                List<IptvAccountType> iptvAccountTypes;

                switch (version)
                {
                    case 3:
                        var apMaxIptvAccountTypesV3 = new IptvV3Service(equipmentConnectionString, serviceVersions).RetrieveIptvAccountTypesByMac(subscriberMac);
                        iptvAccountTypes = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.IPTVServiceV3.IPTVAccountType>, IEnumerable<IptvAccountType>>(_commonMapper, apMaxIptvAccountTypesV3).ToList();
                        break;
                    default:
                        if (version >= 7)
                        {
                            var apMaxIptvAccountTypesV7 = new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).RetrieveIptvAccountTypesByMac(subscriberMac);
                            iptvAccountTypes = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.IPTVServiceV7.IPTVAccountType>, IEnumerable<IptvAccountType>>(_commonMapper, apMaxIptvAccountTypesV7).ToList();
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented"));
                        }
                }

                if (iptvAccountTypes == null || !iptvAccountTypes.Any())
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { iptvAccountTypes }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, iptvAccountTypes);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the iptv subscriber by serial number
        /// </summary>
        /// <param name="subscriberSerialNumber"></param>
        /// <param name="equipmentId">The company identifier.</param>
        /// <returns></returns>
        [Route("iptv/subscriberserialnumber/{subscriberSerialNumber}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveIptvAccountTypesBySerialNumber(string subscriberSerialNumber, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { subscriberSerialNumber, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                List<IptvAccountType> iptvAccountTypes;

                switch (version)
                {
                    case 3:
                        var apMaxIptvAccountTypeV3S = new IptvV3Service(equipmentConnectionString, serviceVersions).RetrieveIptvAccountTypesBySerialNumber(subscriberSerialNumber);
                        iptvAccountTypes = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.IPTVServiceV3.IPTVAccountType>, IEnumerable<IptvAccountType>>(_commonMapper, apMaxIptvAccountTypeV3S).ToList();
                        break;
                    default:
                        if (version >= 7)
                        {
                            var apMaxIptvAccountTypeV7S = new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).RetrieveIptvAccountTypesBySerialNumber(subscriberSerialNumber);
                            iptvAccountTypes = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.IPTVServiceV7.IPTVAccountType>, IEnumerable<IptvAccountType>>(_commonMapper, apMaxIptvAccountTypeV7S).ToList();
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);                            
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented"));
                        }
                }

                if (iptvAccountTypes == null || !iptvAccountTypes.Any())
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { iptvAccountTypes }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, iptvAccountTypes);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }
        
        /// <summary>
        /// Gets the iptv settops by billing account number.
        /// </summary>
        /// <param name="billingAccountNumber">The billing account number.</param>
        /// <param name="equipmentId">The company identifier.</param>
        /// <returns></returns>
        [Route("iptv/settop/billingaccountnumber/{billingAccountNumber}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveIptvSettopsByBillingAccountNumber(string billingAccountNumber, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { billingAccountNumber, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                var settops = new List<SetTopBoxType>();

                switch (version)
                {
                    case 3:
                        var apMaxIptvAccountTypeV3S = new IptvV3Service(equipmentConnectionString, serviceVersions).RetrieveIptvAccountTypesByBillingAccountNumber(billingAccountNumber).ToList();
                        foreach (var account in apMaxIptvAccountTypeV3S)
                        {
                            settops.AddRange(ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.IPTVServiceV3.SetTopBoxType>, IEnumerable<SetTopBoxType>>(_commonMapper, account.SetTopBoxList));
                        }
                        break;
                    default:
                        if (version >= 7)
                        {
                            var apMaxIptvAccountTypeV7S = new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).RetrieveIptvAccountTypesByBillingAccountNumber(billingAccountNumber).ToList();
                            foreach (var account in apMaxIptvAccountTypeV7S)
                            {
                                settops.AddRange(ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.IPTVServiceV7.SetTopBoxType>, IEnumerable<SetTopBoxType>>(_commonMapper, account.SetTopBoxList));
                            }
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { settops }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, settops);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the iptv subscriber by billing account number.
        /// </summary>
        /// <param name="billingAccountNumber">The billing account number.</param>
        /// <param name="equipmentId">The company identifier.</param>
        /// <returns></returns>
        [Route("iptv/billingaccountnumber/{billingAccountNumber}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveIptvAccountTypesByBillingAccountNumber(string billingAccountNumber, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { billingAccountNumber, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                List<IptvAccountType> iptvAccountTypes;

                switch (version)
                {
                    case 3:
                        List<Common.IPTVServiceV3.IPTVAccountType> apMaxIptvAccountTypeV3S = new IptvV3Service(equipmentConnectionString, serviceVersions).RetrieveIptvAccountTypesByBillingAccountNumber(billingAccountNumber).ToList();
                        iptvAccountTypes = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.IPTVServiceV3.IPTVAccountType>, IEnumerable<IptvAccountType>>(_commonMapper, apMaxIptvAccountTypeV3S).ToList();
                        break;
                    default:
                        if (version >= 7)
                        {
                            var apMaxIptvAccountTypeV7S = new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).RetrieveIptvAccountTypesByBillingAccountNumber(billingAccountNumber).ToList();
                            iptvAccountTypes = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.IPTVServiceV7.IPTVAccountType>, IEnumerable<IptvAccountType>>(_commonMapper, apMaxIptvAccountTypeV7S).ToList();
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented"));
                        }
                }

                if (iptvAccountTypes == null || !iptvAccountTypes.Any())
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { iptvAccountTypes }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, iptvAccountTypes);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves IPTV Account by service reference.
        /// </summary>
        /// <param name="serviceReference"></param>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        [Route("iptv/servicereference/{serviceReference}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveIptvAccountByServiceReference(string serviceReference, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { serviceReference, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                IptvAccountType iptvAccountType;

                switch (version)
                {
                    case 3:
                        var apMaxIptvAccountTypeV3S = new IptvV3Service(equipmentConnectionString, serviceVersions).RetrieveIptvAccountByServiceReference(serviceReference);
                        iptvAccountType = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV3.IPTVAccountType, IptvAccountType>(_commonMapper, apMaxIptvAccountTypeV3S);
                        break;
                    default:
                        if (version >= 7)
                        {
                            var apMaxIptvAccountTypeV7S = new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).RetrieveIptvAccountByServiceReference(serviceReference);
                            iptvAccountType = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV7.IPTVAccountType, IptvAccountType>(_commonMapper, apMaxIptvAccountTypeV7S);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented"));
                        }
                }

                if (iptvAccountType == null)
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { iptvAccountType }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, iptvAccountType);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the iptv account by sub address.
        /// </summary>
        /// <param name="subAddress"></param>
        /// <param name="equipmentId">The company identifier.</param>
        /// <returns></returns>
        [Route("iptv/subaddress/{subAddress}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveIptvAccountBySubAddress(string subAddress, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { subAddress, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                List<IptvAccountType> iptvAccountTypes;

                switch (version)
                {
                    case 3:
                        var apMaxIptvAccountTypeV3S = new IptvV3Service(equipmentConnectionString, serviceVersions).RetrieveIptvAccountsBySubAddress(subAddress);
                        iptvAccountTypes = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.IPTVServiceV3.IPTVAccountType>, IEnumerable<IptvAccountType>>(_commonMapper, apMaxIptvAccountTypeV3S).ToList();
                        break;
                    default:
                        if (version >= 7)
                        {
                            var apMaxIptvAccountTypeV7S = new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).RetrieveIptvAccountsBySubAddress(subAddress);
                            iptvAccountTypes = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.IPTVServiceV7.IPTVAccountType>, IEnumerable<IptvAccountType>>(_commonMapper, apMaxIptvAccountTypeV7S).ToList();
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented"));
                        }
                }

                if (iptvAccountTypes == null || !iptvAccountTypes.Any())
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { iptvAccountTypes }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, iptvAccountTypes);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the iptv account by sub address and service reference.
        /// </summary>
        /// <param name="subAddress">the sub address.</param>
        /// <param name="serviceReference">the service reference.</param>
        /// <param name="equipmentId">The company identifier.</param>
        /// <returns></returns>
        [Route("iptv/subaddress/{subAddress}/servicereference/{serviceReference}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveIptvAccountBySubAddressAndServiceRef(string subAddress, string serviceReference, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { subAddress, serviceReference, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                IptvAccountType iptvAccountType;

                switch (version)
                {
                    case 3:
                        var apMaxIptvAccountTypeV3 = new IptvV3Service(equipmentConnectionString, serviceVersions).RetrieveIptvAccountBySubAddressAndServiceRef(subAddress, serviceReference);
                        iptvAccountType = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV3.IPTVAccountType, IptvAccountType>(_commonMapper, apMaxIptvAccountTypeV3);
                        break;
                    default:
                        if (version >= 7)
                        {
                            var apMaxIptvAccountTypeV7 = new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).RetrieveIptvAccountBySubAddressAndServiceRef(subAddress, serviceReference);
                            iptvAccountType = ObjectFactory.CreateInstanceAndMap<Common.IPTVServiceV7.IPTVAccountType, IptvAccountType>(_commonMapper, apMaxIptvAccountTypeV7);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented"));
                        }
                }

                if (iptvAccountType == null)
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { iptvAccountType }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, iptvAccountType);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Creates or Updates the IPTV Account
        /// </summary>
        /// <returns></returns>
        [Route("iptv/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage SetIptvAccount([FromBody]ProvisioningSetIptvAccount provisioningSetIptvAccount)
        {
            try
            {
                if (provisioningSetIptvAccount == null)
                {
                    throw new Exception(MethodBase.GetCurrentMethod().Name + ": ProvisioningSetIptvAccount is required and was not supplied.");
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { provisioningSetIptvAccount }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(provisioningSetIptvAccount.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                switch (version)
                {
                    case 3:
                        var apMaxIptvAccountTypeV3 = ObjectFactory.CreateInstanceAndMap<IptvAccountType, Common.IPTVServiceV3.IPTVAccountType>(_commonMapper, provisioningSetIptvAccount.IptvAccountType);
                        var apMaxIptvSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<SubscriberType, Common.IPTVServiceV3.SubscriberType>(_commonMapper, provisioningSetIptvAccount.SubscriberType);
                        new IptvV3Service(equipmentConnectionString, serviceVersions).SetIptvAccount(apMaxIptvAccountTypeV3, apMaxIptvSubscriberTypeV3);
                        break;
                    default:
                        if (version >= 7)
                        {
                            var apMaxIptvAccountTypeV7 = ObjectFactory.CreateInstanceAndMap<IptvAccountType, Common.IPTVServiceV7.IPTVAccountType>(_commonMapper, provisioningSetIptvAccount.IptvAccountType);
                            var apMaxIptvSubscriberTypeV7 = ObjectFactory.CreateInstanceAndMap<SubscriberType, Common.IPTVServiceV7.SubscriberType>(_commonMapper, provisioningSetIptvAccount.SubscriberType);
                            new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).SetIptvAccount(apMaxIptvAccountTypeV7,apMaxIptvSubscriberTypeV7);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented."));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Creates or Updates the IPTV Account
        /// </summary>
        /// <returns></returns>
        [Route("iptv/existingsubscriber/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage SetIptvAccountExisting([FromBody]ProvisioningSetIptvAccountExistingSubscriber provisioningSetIptvAccount)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { provisioningSetIptvAccount }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(provisioningSetIptvAccount.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;
                
                switch (version)
                {
                    case 3:
                        var apMaxIptvAccountTypeV3 = ObjectFactory.CreateInstanceAndMap<IptvAccountType, Common.IPTVServiceV3.IPTVAccountType>(_commonMapper, provisioningSetIptvAccount.IptvAccountType);
                        new IptvV3Service(equipmentConnectionString, serviceVersions).SetIptvAccountExistingSubscriber(apMaxIptvAccountTypeV3, provisioningSetIptvAccount.DefaultPhoneNumber);
                        break;
                    default:
                        if (version >= 7)
                        {
                            var apMaxIptvAccountTypeV7 = ObjectFactory.CreateInstanceAndMap<IptvAccountType, Common.IPTVServiceV7.IPTVAccountType>(_commonMapper, provisioningSetIptvAccount.IptvAccountType);
                            new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).SetIptvAccountExistingSubscriber(apMaxIptvAccountTypeV7, provisioningSetIptvAccount.DefaultPhoneNumber);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented."));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Creates or Updates the IPTV Account
        /// </summary>
        /// <returns></returns>
        [Route("iptv/ChannelPackageList/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateIptvChannelPackageList([FromBody]ProvisioningSetIptvChannelPackageList provisioningSetIptvChannelPackageList)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { provisioningSetIptvChannelPackageList }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(provisioningSetIptvChannelPackageList.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                switch (version)
                {
                    case 3:
                        var apMaxIptvChannelPackageTypesV3 = ObjectFactory.CreateInstanceAndMap<IEnumerable<ChannelPackageType>, IEnumerable<Common.IPTVServiceV3.ChannelPackageType>>(_commonMapper, provisioningSetIptvChannelPackageList.ChannelPackageTypes).ToList();
                        new IptvV3Service(equipmentConnectionString, serviceVersions).UpdateIptvChannelPackageList(provisioningSetIptvChannelPackageList.ServiceReference, apMaxIptvChannelPackageTypesV3);
                        break;
                    default:
                        if (version >= 7)
                        {
                            var apMaxIptvChannelPackageTypesV7 = ObjectFactory.CreateInstanceAndMap<IEnumerable<ChannelPackageType>,IEnumerable<Common.IPTVServiceV7.ChannelPackageType>>(_commonMapper,provisioningSetIptvChannelPackageList.ChannelPackageTypes).ToList();
                            new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).UpdateIptvChannelPackageList(provisioningSetIptvChannelPackageList.ServiceReference, apMaxIptvChannelPackageTypesV7);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented."));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the iptv service reference.
        /// </summary>
        /// <param name="provisioningUpdateIptvServiceReference">The provisioning update iptv service reference.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [Route("iptv/servicereference/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateIptvServiceReference([FromBody]ProvisioningUpdateIptvServiceReference provisioningUpdateIptvServiceReference)
        {
            try
            {
                if (provisioningUpdateIptvServiceReference == null)
                {
                    throw new Exception(MethodBase.GetCurrentMethod().Name + ": provisioningUpdateIptvServiceReference is required and was not supplied.");
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { provisioningUpdateIptvServiceReference }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(provisioningUpdateIptvServiceReference.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                switch (version)
                {
                    case 3:
                        new IptvV3Service(equipmentConnectionString, serviceVersions).UpdateIptvServiceReference(
                            provisioningUpdateIptvServiceReference.OldServiceReference,
                            provisioningUpdateIptvServiceReference.NewServiceReference);
                        break;
                    default:
                        if (version >= 7)
                        {
                            new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).UpdateIptvServiceReference(
                                provisioningUpdateIptvServiceReference.OldServiceReference,
                                provisioningUpdateIptvServiceReference.NewServiceReference);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented."));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Enables the IPTV Account
        /// </summary>
        /// <param name="provisioningEnableIptvAccount to db testing"></param>
        /// <returns></returns>
        [Route("iptv/enable/")]
        [HttpPut]
        [ClaimsAuthorize]
        [Obsolete("Method is deprecated, please use SuspendIptvAccount instead.")]
        public HttpResponseMessage EnableIptvAccount([FromBody]ProvisioningEnableIptvAccount provisioningEnableIptvAccount)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { provisioningEnableIptvAccount }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(provisioningEnableIptvAccount.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                switch (version)
                {
                    case 3:
                        if (!string.IsNullOrEmpty(provisioningEnableIptvAccount.SubAddress))
                        {
                            new IptvV3Service(equipmentConnectionString, serviceVersions).EnableIptvAccountBySubAddressAndServiceReference(provisioningEnableIptvAccount.SubAddress, provisioningEnableIptvAccount.ServiceReference);
                        }
                        else
                        {
                            new IptvV3Service(equipmentConnectionString, serviceVersions).EnableIptvAccountByServiceReference(provisioningEnableIptvAccount.ServiceReference);
                        }

                        break;
                    default:
                        if (version >= 7)
                        {
                            if (!string.IsNullOrEmpty(provisioningEnableIptvAccount.SubAddress))
                            {
                                new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).EnableIptvAccountBySubAddressAndServiceReference(provisioningEnableIptvAccount.SubAddress,provisioningEnableIptvAccount.ServiceReference);
                            }
                            else
                            {
                                new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).EnableIptvAccountByServiceReference(provisioningEnableIptvAccount.ServiceReference);
                            }

                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented."));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response." + HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Disables the IPTV Account
        /// </summary>
        /// <param name="provisioningDisableIptvAccount"></param>
        /// <returns></returns>
        [Route("iptv/disable/")]
        [HttpPut]
        [ClaimsAuthorize]
        [Obsolete("Method is deprecated, please use SuspendIptvAccount instead.")]
        public HttpResponseMessage DisableIptvAccount([FromBody]ProvisioningDisableIptvAccount provisioningDisableIptvAccount)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { provisioningDisableIptvAccount }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(provisioningDisableIptvAccount.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                switch (version)
                {
                    case 3:
                    {
                        if (!string.IsNullOrEmpty(provisioningDisableIptvAccount.SubAddress))
                        {
                            new IptvV3Service(equipmentConnectionString, serviceVersions).DisableIptvAccountBySubAddressAndServiceReference(provisioningDisableIptvAccount.SubAddress, provisioningDisableIptvAccount.ServiceReference);
                        }
                        else
                        {
                            new IptvV3Service(equipmentConnectionString, serviceVersions).DisableIptvAccountByServiceReference(provisioningDisableIptvAccount.ServiceReference);
                        }

                        break;
                    }
                    default:
                        if (version >= 7)
                        {
                            if (!string.IsNullOrEmpty(provisioningDisableIptvAccount.SubAddress))
                            {
                                new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).DisableIptvAccountBySubAddressAndServiceReference(provisioningDisableIptvAccount.SubAddress,provisioningDisableIptvAccount.ServiceReference);
                            }
                            else
                            {
                                new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).DisableIptvAccountByServiceReference(provisioningDisableIptvAccount.ServiceReference);
                            }
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented."));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response." + HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Suspend the IPTV Account
        /// </summary>
        /// <param name="provisioningSuspendIptvAccount"></param>
        /// <returns></returns>
        [Route("iptv/suspend/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage SuspendIptvAccount([FromBody]ProvisioningSuspendIptvAccount provisioningSuspendIptvAccount)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { provisioningSuspendIptvAccount }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);
                
                var equipmentConnectionString = Setup(provisioningSuspendIptvAccount.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                switch (version)
                {
                    case 3:
                        if (!string.IsNullOrEmpty(provisioningSuspendIptvAccount.SubscriberId))
                        {
                            new IptvV3Service(equipmentConnectionString, serviceVersions).SuspendIptvAccountBySubscriberId(
                                Guid.Parse(provisioningSuspendIptvAccount.SubscriberId), provisioningSuspendIptvAccount.Suspend, provisioningSuspendIptvAccount.SuspendReason);
                        }
                        else
                        {
                            new IptvV3Service(equipmentConnectionString, serviceVersions).SuspendIptvAccountByServiceReference(
                                provisioningSuspendIptvAccount.ServiceReference, provisioningSuspendIptvAccount.Suspend, provisioningSuspendIptvAccount.SuspendReason);
                        }

                        break;
                    default:
                        if (version >= 7)
                        {
                            if (!string.IsNullOrEmpty(provisioningSuspendIptvAccount.SubscriberId))
                            {
                                new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).SuspendIptvAccountBySubscriberId(
                                    Guid.Parse(provisioningSuspendIptvAccount.SubscriberId),
                                    provisioningSuspendIptvAccount.Suspend, provisioningSuspendIptvAccount.SuspendReason);
                            }
                            else
                            {
                                new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).SuspendIptvAccountByServiceReference(provisioningSuspendIptvAccount.ServiceReference,provisioningSuspendIptvAccount.Suspend, provisioningSuspendIptvAccount.SuspendReason);
                            }

                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented."));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response." + HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the IPTV Account
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <param name="subscriberguid"></param>
        /// <param name="serviceReference"></param>
        /// <returns></returns>
        [Route("iptv/subscriberguid/{subscriberguid}/servicereference/{serviceReference}/equipment/{equipmentId}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeleteIptvAccountBySubscriberGuidAndServiceReference(string subscriberguid, string serviceReference, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { subscriberguid, serviceReference, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var myGuid = Guid.Parse(subscriberguid);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                switch (version)
                {
                    case 3:
                        new IptvV3Service(equipmentConnectionString, serviceVersions).DeleteIptvAccountBySubscriberGuidAndServiceReference(myGuid, serviceReference);
                            break;
                    default:
                        if (version >= 7)
                        {
                            new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).DeleteIptvAccountBySubscriberGuidAndServiceReference(myGuid, serviceReference);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented."));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response." + HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the IPTV Account
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <param name="serviceReference"></param>
        /// <param name="force">Indicate if you want to force the delete</param>
        /// <returns></returns>
        [Route("iptv/servicereference/{serviceReference}/force/{force}/equipment/{equipmentId}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeleteIptvAccountByServiceReference(string serviceReference, int equipmentId, bool force)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { serviceReference, equipmentId, force }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                switch (version)
                {
                    case 3:
                        new IptvV3Service(equipmentConnectionString, serviceVersions).DeleteIptvByServiceReference(serviceReference);
                        break;
                    default:
                        if (version >= 7)
                        {
                            new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).DeleteIptvByServiceReference(serviceReference, force);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented."));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response." + HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the IPTV Account
        /// </summary>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <param name="billingAccount"></param>
        /// <param name="serviceReference"></param>
        /// <returns></returns>
        [Route("iptv/BillingAccount/{billingAccount}/servicereference/{serviceReference}/equipment/{equipmentId}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeleteIptvAccountByBillingAccountAndServiceReference(string billingAccount, string serviceReference, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { billingAccount, serviceReference, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                switch (version)
                {
                    case 3:
                        new IptvV3Service(equipmentConnectionString, serviceVersions).DeleteIptvByBillingAccountAndServiceReference(billingAccount, serviceReference);
                            break;
                    default:
                        if (version >= 7)
                        {
                            new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).DeleteIptvByBillingAccountAndServiceReference(billingAccount, serviceReference);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);                            
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented."));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response." + HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the IPTV Account
        /// </summary>
        /// <param name="deleteSubscriber">True to delete subscriber. False to keep subscriber.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <param name="subAddress"></param>
        /// <param name="serviceReference"></param>
        /// <returns></returns>
        [Route("iptv/subaddress/{subAddress}/servicereference/{serviceReference}/deletesubscriber/{deleteSubscriber}/equipment/{equipmentId}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeleteIptvAccountBySubAddressAndServiceReference(string subAddress, string serviceReference, bool deleteSubscriber, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { subAddress, serviceReference, deleteSubscriber, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                switch (version)
                {
                    case 3:
                        new IptvV3Service(equipmentConnectionString, serviceVersions).DeleteIptvAccountBySubAddressAndServiceReference(subAddress, serviceReference, deleteSubscriber);
                        break;
                    default:
                        if (version >= 7)
                        {
                            new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).DeleteIptvAccountBySubAddressAndServiceReference(subAddress, serviceReference,deleteSubscriber);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented."));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes the IPTV Account
        /// </summary>
        /// <param name="deleteSubscriber">True to delete subscriber. False to keep subscriber.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <param name="subAddress"></param>
        /// <param name="serviceReference"></param>
        /// <returns></returns>
        [Route("iptv/fourcedelete/subaddress/{subAddress}/servicereference/{serviceReference}/deletesubscriber/{deleteSubscriber}/equipment/{equipmentId}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage ForceDeleteIptvAccount(string subAddress, string serviceReference, bool deleteSubscriber, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { subAddress, serviceReference, deleteSubscriber, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                switch (version)
                {
                    case 3:
                        new IptvV3Service(equipmentConnectionString, serviceVersions).ForceDeleteIptvAccount(subAddress, serviceReference, deleteSubscriber);
                        break;
                    default:
                        if (version >= 7)
                        {
                            new ApmaxIptvV7Service(equipmentConnectionString, serviceVersions).ForceDeleteIptvAccount(subAddress,serviceReference, deleteSubscriber);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  IPTV version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("IPTV version " + version + " not implemented."));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).",  HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }
        #endregion

        #region ************************** Calling Name info *******************************


        /// <summary>
        /// Retrieves all the Caller Name entries for a given phone number.
        /// </summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("callingname/phone/{phoneNumber}/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveCallerName(string phoneNumber, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { phoneNumber, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.CallingName;

                List<CallingNameType> callingNameTypes;

                switch (version)
                {
                    case 3:
                        var callingNameTypesV3 = new CallingNameV3Service(equipmentConnectionString).RetrieveCallerName(phoneNumber);
                        callingNameTypes = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.CallingNameV3.CallingNameType>, IEnumerable<CallingNameType>>(_commonMapper, callingNameTypesV3).ToList();
                        break;
                    default:
                        if (version >= 4)
                        {
                            var callingNameTypesV4 = new CallingNameV4Service(equipmentConnectionString).RetrieveCallerName(phoneNumber);
                            callingNameTypes = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.CallingNameV4.CallingNameType>, IEnumerable<CallingNameType>>(_commonMapper, callingNameTypesV4).ToList();
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Calling name version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Calling name version " + version + " not implemented"));
                        }
                }

                if (callingNameTypes == null || !callingNameTypes.Any())
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { callingNameTypes }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, callingNameTypes);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Adds Caller Name.
        /// </summary>
        /// <param name="provisioningAddCallerName"></param>
        /// <returns></returns>
        [Route("callingname/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage AddCallerName([FromBody]ProvisioningAddCallerName provisioningAddCallerName)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { provisioningAddCallerName }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(provisioningAddCallerName.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.CallingName;

                switch (version)
                {
                    case 3:
                        new CallingNameV4Service(equipmentConnectionString).AddCallerName(provisioningAddCallerName.PhoneNumber, provisioningAddCallerName.CallerName, provisioningAddCallerName.Presentation);
                        break;
                    default:
                        if (version >= 4)
                        {
                            new CallingNameV4Service(equipmentConnectionString).AddCallerName(provisioningAddCallerName.PhoneNumber, provisioningAddCallerName.CallerName, provisioningAddCallerName.Presentation);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Calling name version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Calling name version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.Created), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes Caller Name.
        /// </summary>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="equipmentId">The equipment identifier.</param>
        /// <returns></returns>
        [Route("callingname/phone/{phoneNumber}/equipment/{equipmentId}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeleteCallerName(string phoneNumber, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { phoneNumber, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.CallingName;

                switch (version)
                {
                    case 3:
                        new CallingNameV3Service(equipmentConnectionString).DeleteCallerName(phoneNumber);
                        break;
                    default:
                        if (version >= 4)
                        {
                            new CallingNameV4Service(equipmentConnectionString).DeleteCallerName(phoneNumber);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Calling name version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Calling name version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Modifies Caller Name.
        /// </summary>
        /// <param name="provisioningModifyCallerName"></param>
        /// <returns></returns>
        [Route("callingname/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage ModifyCallerName([FromBody]ProvisioningAddCallerName provisioningModifyCallerName)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { provisioningModifyCallerName }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(provisioningModifyCallerName.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.CallingName;

                switch (version)
                {
                    case 3:
                        new CallingNameV3Service(equipmentConnectionString).ModifyCallerName(provisioningModifyCallerName.PhoneNumber, provisioningModifyCallerName.CallerName, provisioningModifyCallerName.Presentation);
                        break;
                    default:
                        if (version >= 4)
                        {
                            new CallingNameV4Service(equipmentConnectionString).ModifyCallerName(provisioningModifyCallerName.PhoneNumber, provisioningModifyCallerName.CallerName,provisioningModifyCallerName.Presentation);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Calling name version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Calling name version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }


        /// <summary>
        /// Reassigns Caller Name.
        /// </summary>
        /// <param name="provisioningReassignCallerName"></param>
        /// <returns></returns>
        [Route("callingname/reassign/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage ReassignCallerName([FromBody]ProvisioningReassignCallerName provisioningReassignCallerName)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { provisioningReassignCallerName }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(provisioningReassignCallerName.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.CallingName;

                switch (version)
                {
                    case 3:
                        new CallingNameV3Service(equipmentConnectionString).ReassignCallerName(provisioningReassignCallerName.NewPhoneNumber, provisioningReassignCallerName.OldPhoneNumber);
                        break;
                    default:
                        if (version >= 4)
                        {
                            new CallingNameV4Service(equipmentConnectionString).ReassignCallerName(provisioningReassignCallerName.NewPhoneNumber,provisioningReassignCallerName.OldPhoneNumber);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Calling name version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Calling name version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Creates Caller Name Screen Pop.
        /// </summary>
        /// <param name="provisioningCreateScreenPop"></param>
        /// <returns></returns>
        [Route("callingname/screenpop/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage AddScreenPop([FromBody]ProvisioningCreateScreenPop provisioningCreateScreenPop)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { provisioningCreateScreenPop }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(provisioningCreateScreenPop.EquipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.CallingName;

                switch (version)
                {
                    //case 3:
                        //ApMax changed how the create screen pop worked from 3 to 4. Right now coding towards version 4 since that's the newest version and we don' thave a client
                        //on version 3.
                        //var screenPopSubscriberTypeV3 = ObjectFactory.CreateInstanceAndMap<ScreenPopSubscriberType, Common.CallingNameV3.ScreenPopSubscriberType>(_commonMapper, provisioningCreateScreenPop.ScreenPopSubscriberType);
                        //new ApMaxCallingNameV3Service(equipmentConnectionString).AddScreenPop(screenPopSubscriberTypeV3, provisioningCreateScreenPop.NpaNxx, provisioningCreateScreenPop.Description);
                        //throw new NotImplementedException("IPTV version " + version + " not implemented"); 
                        //break;
                    default:
                        if (version >= 4)
                        {
                            var screenPopSubscriberTypeV4 = ObjectFactory.CreateInstanceAndMap<ScreenPopSubscriberType, Common.CallingNameV4.ScreenPopSubscriberType>(_commonMapper, provisioningCreateScreenPop.ScreenPopSubscriberType);
                            new CallingNameV4Service(equipmentConnectionString).AddScreenPop(screenPopSubscriberTypeV4, provisioningCreateScreenPop.NpaNxx, provisioningCreateScreenPop.Description);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Calling name version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Calling name version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0}).", HttpStatusCode.Created), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Deletes Calling Name Screen Pop.
        /// </summary>
        /// <param name="calledNumber"></param>
        /// <param name="npaNxx"></param>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        [Route("callingname/screenpop/callednumber/{calledNumber}/npanxx/{NpaNxx}/equipment/{equipmentId}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeleteScreenPop(string calledNumber, string npaNxx, int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { calledNumber, npaNxx, equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.CallingName;

                switch (version)
                {
                    case 3:
                            new CallingNameV3Service(equipmentConnectionString).DeleteScreenPop(calledNumber, npaNxx);
                            break;
                    default:
                        if (version >= 4)
                        {
                            new CallingNameV4Service(equipmentConnectionString).DeleteScreenPop(calledNumber, npaNxx);
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Calling name version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Calling name version " + version + " not implemented"));
                        }
                }

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves all Caller Name Screen Pop Entries.
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        [Route("callingname/screenpop/equipment/{equipmentId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveAllScreenPopEntries(int equipmentId)
        {
            try
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { equipmentId }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI was called."), LogLevelType.Info);

                var equipmentConnectionString = Setup(equipmentId);
                var serviceVersions = new ApMaxCore(equipmentConnectionString).GetVersions();
                var version = serviceVersions.Iptv;

                List<ScreenPopType> screenPopTypes = null;

                switch (version)
                {
                    case 3:
                        IEnumerable<Common.CallingNameV3.ScreenPopType> screenPopTypesV3 = new CallingNameV3Service(equipmentConnectionString).RetrieveAllScreenPopEntries();
                        screenPopTypes = ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.CallingNameV3.ScreenPopType>, IEnumerable<ScreenPopType>>(_commonMapper, screenPopTypesV3).ToList();
                        break;
                    default:
                        if (version >= 4)
                        {
                            IEnumerable<Common.CallingNameV4.ScreenPopType> screenPopTypesV4 = new CallingNameV4Service(equipmentConnectionString).RetrieveAllScreenPopEntries();
                            screenPopTypes =ObjectFactory.CreateInstanceAndMap<IEnumerable<Common.CallingNameV4.ScreenPopType>, IEnumerable<ScreenPopType>>(_commonMapper, screenPopTypesV4).ToList();
                            break;
                        }
                        else
                        {
                            _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Calling name version " + version + " not implemented."), LogLevelType.Error);
                            return this.Request.CreateResponse(HttpStatusCode.NotImplemented, new NotImplementedException("Calling name version " + version + " not implemented"));
                        }
                }

                if (screenPopTypes == null || !screenPopTypes.Any())
                {
                    _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.NoContent), LogLevelType.Info);
                    return this.Request.CreateResponse(HttpStatusCode.NoContent);
                }

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { screenPopTypes }, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response({0})", HttpStatusCode.OK), LogLevelType.Info);
                return this.Request.CreateResponse(HttpStatusCode.OK, screenPopTypes);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in ProvisioningAPI.  Response."), LogLevelType.Error, ex);
                throw;
            }
        }
        #endregion
    }
}
