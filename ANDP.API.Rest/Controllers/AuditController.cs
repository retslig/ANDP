using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using ANDP.Lib.Domain.Factories;
using ANDP.Lib.Domain.Interfaces;
using ANDP.Lib.Domain.Models;
using Common.Lib.Common.Enums;
using Common.Lib.Extensions;
using Common.Lib.Interfaces;
using Common.Lib.Security;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace ANDP.API.Rest.Controllers
{
    /// <summary>
    /// This controller handles all orders.
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/audit")]
    public class AuditController : ApiController
    {
        private readonly ILogger _logger;
        private IAuditService _auditService;
        private Guid _tenantId;
        private string _user;
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public AuditController(ILogger logger)
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

            _auditService = AuditServiceFactory.Create(_tenantId);
        }

        /// <summary>
        /// Retrieves the billing audit records.
        /// </summary>
        /// <param name="auditRecord">The audit record.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage CreateAuditRecords([FromBody]AuditRecord auditRecord)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                _auditService.CreateAuditRecords(auditRecord, _user);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Retrieves the audit records.
        /// </summary>
        /// <param name="runNumber">The run number.</param>
        /// <param name="recordKey">The record key.</param>
        /// <param name="equipmentSetupId">The equipment setup identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        [Route("runNumber/{runNumber}/recordKey/{recordKey}/equipment/{equipmentSetupId}/company/{companyId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveAuditRecords(Guid runNumber, string recordKey, int equipmentSetupId, int companyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var auditRecords = _auditService.RetrieveAuditRecords(runNumber, recordKey, equipmentSetupId, companyId);
                return this.Request.CreateResponse(HttpStatusCode.OK, auditRecords);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex);
                throw ex.AddEntityValidationInfo();
            }
        }
    }
}
