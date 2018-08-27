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
using ANDP.Lib.Domain.Interfaces;
using Common.Lib.Common.Enums;
using Common.Lib.Extensions;
using Common.Lib.Interfaces;
using Common.Lib.Security;
using Common.Udp.Factories;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace ANDP.API.Rest.Controllers
{
    /// <summary>
    /// This controller handles all orders.
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/udpbilling")]
    public class UdpBillingController : ApiController
    {
        private readonly ILogger _logger;
        private IBillingService _iBillingService;
        private Guid _tenantId;
        private string _user;
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpBillingController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public UdpBillingController(ILogger logger)
        {
            _name = Assembly.GetAssembly(this.GetType()).GetName().Name + " " +
                    Assembly.GetAssembly(this.GetType()).GetName().Version;
            _logger = logger;
        }

        /// <summary>
        /// The reason this is not in the constructor is solely for the purpose of exception handling.
        /// If you leave this in the controller and someone who is not authenticated calls the API you will not get a tenantId not found error.
        /// The error will be ugly and be hard to figure out you are not authorized.
        /// This way if the all methods have the ClaimsAuthorize attribute on them they will first be authenticated if not get a nice error message of not authorized.
        /// </summary>
        /// <exception cref="System.Exception">No Tenant Id Found.</exception>
        private void Setup()
        {
            //var isAllowed = ClaimsAuthorization.CheckAccess("Get", "CustomerId", "00");
            //isAllowed = true;
            //Get the current claims principal
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var tenant = identity.Claims.Where(c => c.Type == ClaimsConstants.TenantIdClaimType).Select(c => c.Value).SingleOrDefault();

            if (string.IsNullOrEmpty(tenant))
                throw new Exception("No Tenant Id Found.");

            _tenantId = Guid.Parse(tenant);
            _user = identity.Identity.Name;

            _iBillingService = UdpBillingServiceFactory.Create(_tenantId);
        }

        /// <summary>
        /// Retrieves the billing records.
        /// </summary>
        /// <param name="externalCompanyId">The external company identifier.</param>
        /// <param name="startExternalAccountId">The start external account identifier.</param>
        /// <param name="endExternalAccountId">The end external account identifier.</param>
        /// <returns></returns>
        [Route("externalCompanyId/{externalCompanyId}/startExternalAccountId/{startExternalAccountId}/endExternalAccountId/{endExternalAccountId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveBillingRecordsByAccountNumberRange(string externalCompanyId, string startExternalAccountId, string endExternalAccountId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> {string.Format("Ext AccountId = {0} and Ext AccountId = {1} and Ext CompanyId: {2}", startExternalAccountId, endExternalAccountId, externalCompanyId)},
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Info);

                var result = _iBillingService.RetrieveBillingRecordsByAccountNumberRange(externalCompanyId, startExternalAccountId, endExternalAccountId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> { string.Format("Ext AccountId = {0} and Ext AccountId = {1} and Ext CompanyId: {2}", startExternalAccountId, endExternalAccountId, externalCompanyId) },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Error,
                    ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Retrieves the billing records.
        /// </summary>
        /// <param name="externalCompanyId">The external company identifier.</param>
        /// <param name="startPhoneNumber">The start phone number.</param>
        /// <param name="endPhoneNumber">The end phone number.</param>
        /// <returns></returns>
        [Route("externalCompanyId/{externalCompanyId}/startPhoneNumber/{startPhoneNumber}/endPhoneNumber/{endPhoneNumber}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveBillingRecordsByPhoneRange(string externalCompanyId, string startPhoneNumber, string endPhoneNumber)
        {
            try
            {
                Setup();

                if (startPhoneNumber.Length != 10 || endPhoneNumber.Length != 10)
                    throw new Exception("Phone numbers must be 9 digits in length");

                int startNpa = int.Parse(startPhoneNumber.Substring(0, 3));
                int startNxx = int.Parse(startPhoneNumber.Substring(3, 3));
                int startStation = int.Parse(startPhoneNumber.Substring(6));

                int endNpa = int.Parse(endPhoneNumber.Substring(0, 3));
                int endNxx = int.Parse(endPhoneNumber.Substring(3, 3));
                int endStation = int.Parse(endPhoneNumber.Substring(6));

                if (endNpa != startNpa && startNxx != endNxx)
                    throw new Exception("Phone numbers must have same npa and nxx.");

                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object>
                    {
                        string.Format("StartPhoneNumber = {0} and EndPhoneNumber = {1} and Ext CompanyId: {2}", startPhoneNumber, endPhoneNumber, externalCompanyId)
                    },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Info);
                var result = _iBillingService.RetrieveBillingRecordsByPhoneRange(externalCompanyId, startNpa, startNxx, startStation, endStation);
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(),
                    new List<object> { string.Format("StartPhoneNumber = {0} and EndPhoneNumber = {1} and Ext CompanyId: {2}", startPhoneNumber, endPhoneNumber, externalCompanyId) },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name),
                    LogLevelType.Error,
                    ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }
    }
}
