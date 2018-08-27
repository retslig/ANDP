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
using ANDP.Lib.Factories;
using BrockAllen.MembershipReboot;
using Common.Lib.Common.Enums;
using Common.Lib.Data.Repositories.Common;
using Common.Lib.Extensions;
using Common.Lib.Interfaces;
using Common.Lib.Security;

namespace ANDP.API.Rest.Controllers
{
    /// <summary>
    /// This controller handles all Auth for Provisioning API.
    /// </summary>
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/authentication")]
    [Authorize]
    public class AuthController : ApiController
    {
        private readonly UserAccountService _userAccountService;
        private readonly ICommonRepository _commonRepository;
        private readonly ILogger _logger;
        private Guid _tenantId;
        private string _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        public AuthController(ICommonRepository commonRepository, ILogger logger)
        {
            //_userAccountService = userAccountService;
            _userAccountService = UserAccountServiceFactory.Create();
            _commonRepository = commonRepository;
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
        }

        /// <summary>
        /// Retrieves the claims for user.
        /// </summary>
        /// <returns></returns>
        [Route("claims/")]
        [HttpGet]
        public HttpResponseMessage RetrieveClaimsForUser()
        {
            try
            {
                Setup();

                var tenant = _commonRepository.RetrieveTenantById(_tenantId);

                //Get tenant name from id
                string tenantName = tenant.Schema;

                var user = _userAccountService.GetByUsername(tenantName, _user);
                var claims = user.GetAllClaims();

                return this.Request.CreateResponse(HttpStatusCode.OK, claims);
            }
            catch (Exception ex)
            {
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <returns></returns>
        [Route("authenticate/")]
        [HttpGet]
        public HttpResponseMessage AuthenticateUser()
        {
            try
            {
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw ex.AddEntityValidationInfo();
            }
        }
    }
}