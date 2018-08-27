using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using ANDP.API.Rest.Models;
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
    [RoutePrefix("api/engine")]
    public class EngineController : ApiController
    {
        private readonly ILogger _logger;
        private Guid _tenantId;
        private IProvisioningEngineService _iProvisioningEngineService;
        private IOrderService _iOrderService;
        private string _user;
        private string _serviceName;
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public EngineController(ILogger logger)
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
            _serviceName = DispatcherServiceFactory.RetrieveServiceName(_tenantId);

            _iProvisioningEngineService = ProvisioningEngineServiceFactory.Create(_tenantId);
            _iOrderService = OrderServiceFactory.Create(_tenantId);
        }


        /// <summary>
        /// Retrieves the provisioning engine status.
        /// </summary>
        /// <param name="externalCompanyId">The external company identifier.</param>
        /// <returns></returns>
        [Route("status/company/{externalCompanyId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveProvisioningEngineStatus(string externalCompanyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                string serviceName = DispatcherServiceFactory.RetrieveServiceName(_tenantId);
                int companyId = _iOrderService.RetrieveCompanyIdByExtCompanyId(externalCompanyId);
                var status = _iProvisioningEngineService.RetrieveProvisioningEngineStatus(companyId, serviceName);

                return this.Request.CreateResponse(HttpStatusCode.OK, status);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw;
            }
        }

        /// <summary>
        /// Provisions the order.
        /// </summary>
        /// <param name="orderDto">The order dto.</param>
        /// <returns></returns>
        [Route("provision/order/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage ProvisionOrder([FromBody]OrderDto orderDto)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { orderDto }, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                int orderId = _iOrderService.RetrieveOrderIdByExtIdAndExtCompanyId(orderDto.ExternalOrderId, orderDto.ExternalCompanyId);
                int companyId = _iOrderService.RetrieveCompanyIdByExtCompanyId(orderDto.ExternalCompanyId);
                var task = _iProvisioningEngineService.ProvisionOrder(orderId, companyId, orderDto.TestMode, orderDto.SendResponse, orderDto.ForceProvision, _user);
                Task.WaitAll(task);
                return this.Request.CreateResponse(HttpStatusCode.OK, task.Result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }
        
        /// <summary>
        /// Provisions the service.
        /// </summary>
        /// <param name="serviceDto">The service dto.</param>
        /// <returns></returns>
        [Route("provision/service/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage ProvisionService([FromBody]ServiceDto serviceDto)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { serviceDto }, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                int orderId = _iOrderService.RetrieveOrderIdByExtIdAndExtCompanyId(serviceDto.ExternalOrderId, serviceDto.ExternalCompanyId);
                int companyId = _iOrderService.RetrieveCompanyIdByExtCompanyId(serviceDto.ExternalCompanyId);
                int serviceId = _iOrderService.RetrieveServiceIdByExtOrderIdAndExtIdAndExtCompanyId(serviceDto.ExternalOrderId, serviceDto.ExternalServiceId, serviceDto.ExternalCompanyId);
                var task = _iProvisioningEngineService.ProvisionService(orderId, serviceId, companyId, serviceDto.TestMode, serviceDto.SendResponse, serviceDto.ForceProvision, _user);
                Task.WaitAll(task);
                return this.Request.CreateResponse(HttpStatusCode.OK, task.Result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Stops the provisioning engine.
        /// </summary>
        /// <returns></returns>
        [Route("stop/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage StopProvisioningEngine([FromBody] string externalCompanyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { externalCompanyId }, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                int companyId = _iOrderService.RetrieveCompanyIdByExtCompanyId(externalCompanyId);
                _iProvisioningEngineService.StopProvisioning(companyId, _serviceName);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Starts the provisioning engine.
        /// </summary>
        /// <param name="externalCompanyId">The external company identifier.</param>
        /// <returns></returns>
        [Route("start/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage StartProvisioningEngine([FromBody] string externalCompanyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { externalCompanyId }, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                int companyId = _iOrderService.RetrieveCompanyIdByExtCompanyId(externalCompanyId);
                _iProvisioningEngineService.StartProvisioning(companyId, _serviceName);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Pauses the provisioning engine.
        /// </summary>
        /// <param name="externalCompanyId">The external company identifier.</param>
        /// <returns></returns>
        [Route("pause/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage PauseProvisioningEngine([FromBody] string externalCompanyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { externalCompanyId }, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                int companyId = _iOrderService.RetrieveCompanyIdByExtCompanyId(externalCompanyId);
                _iProvisioningEngineService.PauseProvisioning(companyId, _user);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Unpause provisioning engine.
        /// </summary>
        /// <param name="externalCompanyId">The external company identifier.</param>
        /// <returns></returns>
        [Route("unpause/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage UnPauseProvisioningEngine([FromBody] string externalCompanyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { externalCompanyId }, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                int companyId = _iOrderService.RetrieveCompanyIdByExtCompanyId(externalCompanyId);
                _iProvisioningEngineService.UnPauseProvisioning(companyId, _user);
                return this.Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }
    }
}
