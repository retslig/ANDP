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
    [RoutePrefix("api/order")]
    public class OrderController : ApiController
    {
        private readonly ILogger _logger;
        private Guid _tenantId;
        private IOrderService _iOrderService;
        private string _user;
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public OrderController(ILogger logger)
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

            _iOrderService = OrderServiceFactory.Create(_tenantId);
        }

        /// <summary>
        /// Retrieves the pending orders.
        /// </summary>
        /// <param name="statusType">Type of the status.</param>
        /// <param name="externalCompanyId">The external company identifier.</param>
        /// <returns></returns>
        [Route("statustype/{statusType}/company/{externalCompanyId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveOrdersByStatusTypeAndExtCompanyId(StatusType statusType, string externalCompanyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name + " StatusType = {0} and Ext CompanyId: {1}", statusType, externalCompanyId), LogLevelType.Info);
                var result = _iOrderService.RetrieveOrdersByStatusTypeAndExtCompanyId(statusType, externalCompanyId);
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Gets the order by ext identifier.
        /// </summary>
        /// <param name="externalOrderId">The external order identifier.</param>
        /// <param name="externalCompanyId">The external company identifier.</param>
        /// <returns></returns>
        [Route("{externalOrderId}/company/{externalCompanyId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveOrderByExtIdAndExtCompanyId(string externalOrderId, string externalCompanyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name + " Ext Order Id = {0} and Ext CompanyId: {1}", externalOrderId, externalCompanyId), LogLevelType.Info);
                var result = _iOrderService.RetrieveOrderByExtIdAndExtCompanyId(externalOrderId, externalCompanyId);
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Gets the order by identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        [Route("{orderId:int}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveOrderById(int orderId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name + " Order Id = {0}.", orderId), LogLevelType.Info);
                var result = _iOrderService.RetrieveOrderById(orderId);
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Retrieves the provisioning orders.
        /// </summary>
        /// <returns></returns>
        [Route("processing/company/{externalCompanyId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveProcessingOrders(string externalCompanyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name + " Order Id = {0}.", externalCompanyId), LogLevelType.Info);
                var result = _iOrderService.RetrieveProcessingOrders(externalCompanyId);
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Retrieves all companies ext ids.
        /// </summary>
        /// <returns></returns>
        [Route("~/api/company/")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveAllCompaniesExtIds()
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);
                var result = _iOrderService.RetrieveAllCompanies();
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <param name="externalOrderId">The external order identifier.</param>
        /// <param name="externalCompanyId">The external company identifier.</param>
        /// <returns></returns>
        [Route("~/api/orderstatus/{externalOrderId}/company/{externalCompanyId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveOrderStatusByExtIdAndExtCompanyId(string externalOrderId, string externalCompanyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name + " Ext Order Id = {0} and Ext CompanyId: {1}", externalOrderId, externalCompanyId), LogLevelType.Info);
                var result = _iOrderService.RetrieveOrderStatusByExtIdAndExtCompanyId(externalOrderId, externalCompanyId);
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Gets the order by ext identifier.
        /// </summary>
        /// <param name="externalOrderId">The external order identifier.</param>
        /// <param name="externalServiceId">The external service identifier.</param>
        /// <param name="externalCompanyId">The external company identifier.</param>
        /// <returns></returns>
        [Route("{externalOrderId}/service/{externalServiceId}/company/{externalCompanyId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveServiceByExtOrderIdAndExtIdAndExtCompanyId(string externalOrderId, string externalServiceId, string externalCompanyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name + " Ext Order Id = {0} and Ext ServiceId: {1} and Ext CompanyId: {2}", externalOrderId, externalServiceId, externalCompanyId), LogLevelType.Info);
                var result = _iOrderService.RetrieveServiceByExtOrderIdAndExtIdAndExtCompanyId(externalOrderId, externalServiceId, externalCompanyId);
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Retrieves the service by order identifier and identifier.
        /// </summary>
        /// <param name="serviceId">The service identifier.</param>
        /// <returns></returns>
        [Route("~/api/service/{serviceId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveServiceByOrderIdAndId(int serviceId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name + " ServiceId: {0}", serviceId), LogLevelType.Info);
                var result = _iOrderService.RetrieveServiceById(serviceId);
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Retrieves the item.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <returns></returns>
        [Route("~/api/item/{itemId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage RetrieveItem(int itemId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name + " Item Id = {0}", itemId), LogLevelType.Info);
                object result = _iOrderService.RetrieveItemById(itemId);

                if (result is PhoneItem)
                    return this.Request.CreateResponse(HttpStatusCode.OK, result as PhoneItem);

                if (result is VideoItem)
                    return this.Request.CreateResponse(HttpStatusCode.OK, result as VideoItem);

                if (result is InternetItem)
                    return this.Request.CreateResponse(HttpStatusCode.OK, result as InternetItem);

                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Undoes the order.
        /// </summary>
        /// <param name="externalOrderId">The external order identifier.</param>
        /// <param name="externalCompanyId">The external company identifier.</param>
        /// <returns></returns>
        [Route("~/api/undoorder/{externalOrderId}/company/{externalCompanyId}")]
        [HttpGet]
        [ClaimsAuthorize]
        public HttpResponseMessage UndoOrderByExtIdAndExtCompanyId(string externalOrderId, string externalCompanyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name + " Ext Order Id = {0} and Ext CompanyId: {1}", externalOrderId, externalCompanyId), LogLevelType.Info);
                var result = _iOrderService.UndoOrderByExtIdAndExtCompanyId(externalOrderId, externalCompanyId, _user);
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Posts the specified order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage CreateOrder([FromBody]Order order)
        {
            try
            {
                Setup();

                if (order == null)
                    throw new Exception("Order data is empty.  Please make sure you have sent order information in the body of the http post.");

                var status = _iOrderService.CreateOrUpdateOrder(order, _user);
                return this.Request.CreateResponse(HttpStatusCode.Created, status);
            }
            catch (Exception ex)
            {
                
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList(), order.SerializeObjectToJsonString() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Puts the specified order.
        /// </summary>
        /// <param name="externalOrderId">The external order identifier.</param>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        [Route("{externalOrderId}")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateOrder(string externalOrderId, [FromBody] Order order)
        {
            try
            {
                Setup();

                if (order == null)
                    throw new Exception("Order data is empty.  Please make sure you have sent order information in the body of the http put.");

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { order.SerializeObjectToJsonString() }, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                var status = _iOrderService.UpdateOrder(order, _user);
                return this.Request.CreateResponse(HttpStatusCode.OK, status);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList(), order.SerializeObjectToJsonString() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Deletes the specified external order identifier.
        /// </summary>
        /// <param name="externalOrderId">The external order identifier.</param>
        /// <param name="externalCompanyId">The external company identifier.</param>
        /// <returns></returns>
        [Route("{externalOrderId}/company/{externalCompanyId}")]
        [HttpDelete]
        [ClaimsAuthorize]
        public HttpResponseMessage DeleteOrderByExtIdAndExtCompanyId(string externalOrderId, string externalCompanyId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name + " Ext Order Id = {0} and Ext CompanyId: {1}", externalOrderId, externalCompanyId), LogLevelType.Info);
                var status = _iOrderService.DeleteOrderByExtIdAndExtCompanyId(externalOrderId, externalCompanyId, _user);
                return this.Request.CreateResponse(HttpStatusCode.OK, status);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Updates the order to force skip.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        [Route("order/{orderId}/forceskip/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateOrderToForceSkip(int orderId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                var order = _iOrderService.RetrieveOrderById(orderId);
                var priority = order.Priority + 1;
                _iOrderService.UpdateOrderStatus(orderId, StatusType.Pending, priority, "Order force skipped.", "", null, null, _user);
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
        /// Updates the order to pending.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        [Route("order/{orderId}/pending/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateOrderToPending(int orderId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                _iOrderService.UpdateOrderStatus(orderId, StatusType.Pending, 0, "", "", null, null, _user);
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
        /// Updates the sent response flag.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        [Route("responsesent/{orderId}")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateOrderSentResponseFlag(int orderId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                _iOrderService.UpdateOrderSentResponseFlag(orderId, true, _user);
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
        /// Updates the item to force skip.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <returns></returns>
        [Route("item/{itemId}/forceskip/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateItemToForceSkip(int itemId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                object result = _iOrderService.RetrieveItemById(itemId);
                int priority = 0;
                if (result is PhoneItem)
                    priority = (result as PhoneItem).Priority + 1;
                
                if (result is VideoItem)
                    priority = (result as VideoItem).Priority + 1;

                if (result is InternetItem)
                    priority = (result as InternetItem).Priority + 1;

                _iOrderService.UpdateItemStatus(itemId, StatusType.Pending, priority, "Item force skipped.", "", null, null, _user);
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
        /// Updates the item results.
        /// </summary>
        /// <param name="itemProvisioningResult">The item engine result.</param>
        /// <returns>The order status</returns>
        [Route("itemresults/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateItemResults([FromBody] ItemProvisioningResult itemProvisioningResult)
        {
            try
            {
                Setup();

                if (itemProvisioningResult == null)
                    throw new Exception("Must provide itemProvisioningResult info.");

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { itemProvisioningResult }, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                var orderStatus = _iOrderService.UpdateItemResult(itemProvisioningResult, _user);
                return this.Request.CreateResponse(HttpStatusCode.OK, orderStatus);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { itemProvisioningResult, ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Updates the service to force skip.
        /// </summary>
        /// <param name="serviceId">The service identifier.</param>
        /// <returns></returns>
        [Route("service/{serviceId}/forceskip/")]
        [HttpPut]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateServiceToForceSkip(int serviceId)
        {
            try
            {
                Setup();

                _logger.WriteLogEntry(_tenantId.ToString(), null, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                var service = _iOrderService.RetrieveServiceById(serviceId);
                var priority = service.Priority + 1;
                _iOrderService.UpdateServiceStatus(serviceId, StatusType.Pending, priority, "Service force skipped.", "", null, null, _user);
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
        /// Updates the service results.
        /// </summary>
        /// <param name="serviceProvisioningResult">The order engine result.</param>
        /// <returns>The order status</returns>
        [Route("serviceresults/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateServiceResults([FromBody] ServiceProvisioningResult serviceProvisioningResult)
        {
            try
            {
                Setup();

                if (serviceProvisioningResult == null)
                    throw new Exception("Must provide serviceProvisioningResult info.");

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { serviceProvisioningResult }, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                var orderStatus = _iOrderService.UpdateServiceResult(serviceProvisioningResult, _user);
                return this.Request.CreateResponse(HttpStatusCode.OK, orderStatus);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { serviceProvisioningResult, ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }

        /// <summary>
        /// Updates the order results.
        /// </summary>
        /// <param name="orderProvisioningResult">The order engine result.</param>
        /// <returns>The order status</returns>
        /// <exception cref="System.Exception">Error ServiceResults has no entries.</exception>
        [Route("orderresults/")]
        [HttpPost]
        [ClaimsAuthorize]
        public HttpResponseMessage UpdateOrderResults([FromBody] OrderProvisioningResult orderProvisioningResult)
        {
            try
            {
                Setup();

                if (orderProvisioningResult == null)
                    throw new Exception("Must provide orderProvisioningResult info.");

                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> {orderProvisioningResult}, string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Info);

                _iOrderService.UpdateOrderStatus(
                    orderProvisioningResult.Id,
                    orderProvisioningResult.StatusType,
                    0,
                    orderProvisioningResult.ErrorMessage,
                    orderProvisioningResult.Log,
                    orderProvisioningResult.StartDate,
                    orderProvisioningResult.CompletionDate,
                    _user);

                //Now get the orderstatus
                var orderStatus = _iOrderService.RetrieveOrderStatusById(orderProvisioningResult.Id);
                return this.Request.CreateResponse(HttpStatusCode.OK, orderStatus);
            }
            catch (Exception ex)
            {
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { orderProvisioningResult, ex.RetrieveEntityExceptionDataAsObjectList() },
                    string.Format(MethodBase.GetCurrentMethod().Name + " in " + _name), LogLevelType.Error, ex.GetInnerMostException());
                throw ex.AddEntityValidationInfo();
            }
        }
    }
}
