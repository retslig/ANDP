
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using ANDP.Lib.Domain.Models;
using ANDP.Lib.Domain.Interfaces;
using Common.Lib.Common.Enums;
using Common.Lib.Interfaces;
using Common.Lib.Services;
using Common.Lib.Utility;

namespace ANDP.Lib.Domain.Services
{
    public class ProvisioningEngineService : IProvisioningEngineService
    {
        private readonly IEngineService _iEngineService;
        private readonly IOrderService _iOrderService;
        private readonly ILogger _logger;
        private readonly Guid _tenantId;

        public ProvisioningEngineService(IEngineService iEngineService, IOrderService iOrderService, ILogger logger, Guid tenantId)
        {
            _iEngineService = iEngineService;
            _iOrderService = iOrderService;
            _logger = logger;
            _tenantId = tenantId;
        }

        public void StartProvisioning(int companyId, string serviceName)
        {
            WindowsServiceHelperService.StartService(serviceName, 5000);
        }

        public void StopProvisioning(int companyId, string serviceName)
        {
            WindowsServiceHelperService.StopService(serviceName, 5000);
        }

        public void PauseProvisioning(int companyId, string updatingUserId)
        {
            _iEngineService.PauseProvisioning(companyId, updatingUserId);
        }

        public void UnPauseProvisioning(int companyId, string updatingUserId)
        {
            _iEngineService.UnPauseProvisioning(companyId, updatingUserId);
        }

        public EngineSetting RetrieveProvisioningEngineSetting(int companyId)
        {
            return _iEngineService.RetrieveProvisioningEngineSetting(companyId);
        }

        public EngineStatus RetrieveProvisioningEngineStatus(int companyId, string serviceName)
        {
            var settings = _iEngineService.RetrieveProvisioningEngineSetting(companyId);

            switch (WindowsServiceHelperService.RetrieveServiceStatus(serviceName))
            {
                case ServiceControllerStatus.Running:
                    if (settings.ProvisioningPaused)
                        return EngineStatus.Paused;

                    return EngineStatus.Running;
                case ServiceControllerStatus.Stopped:
                    return EngineStatus.Stopped;
                case ServiceControllerStatus.Paused:
                    return EngineStatus.Paused;
                //case ServiceControllerStatus.StopPending:
                //    return "Stopping";
                //case ServiceControllerStatus.StartPending:
                //    return "Starting";
                default:
                    throw new Exception("Cannot Determine Status.");
            }
        }

        /// <summary>
        /// Provisions the order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="testMode">if set to <c>true</c> [test mode].</param>
        /// <param name="sendResponse">if set to <c>true</c> [send response].</param>
        /// <param name="forceRun">if set to <c>true</c> [force run].</param>
        /// <param name="updatingUserId">The updating user identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">The order has been deleted.
        /// or
        /// The order has been run and errored.
        /// or
        /// The order is currently Processing
        /// or
        /// The order has already been completed.
        /// or
        /// Status not implemented.
        /// or
        /// or
        /// Script Error: The scripts did not return the correct object. orderEngineResult is empty.</exception>
        public async Task<OrderStatus >ProvisionOrder(int orderId, int companyId, bool testMode, bool sendResponse, bool forceRun, string updatingUserId)
        {
            //get order status
            var order = _iOrderService.RetrieveOrderById(orderId);

            //Check make sure order is not in some other status.
            //If in testmode allow to get past validation because should not be live anyway.
            if (!testMode)
            {
                switch (order.StatusType)
                {
                    case StatusType.Deleted:
                        throw new Exception("The order has been deleted.");
                    case StatusType.Error:
                        if (!forceRun)
                            throw new Exception("The order has been run and errored.");

                        break;
                    case StatusType.Pending:
                        //Do nothing as you should process as expected.
                        break;
                    case StatusType.Processing:
                        throw new Exception("The order is currently Processing");
                    case StatusType.Success:
                        if (!forceRun)
                            throw new Exception("The order has already been completed.");

                        break;
                    default:
                        throw new Exception("Status not implemented.");
                }

                //Set Order status to processing.
                _iOrderService.UpdateOrderStatus(orderId, StatusType.Processing, 0, "", "", null, null, updatingUserId);

                foreach (var service in order.Services)
                {
                    //Set Service status to processing.
                    _iOrderService.UpdateServiceStatus(service.Id, StatusType.Processing, 0, "", "", null, null, updatingUserId);
                    foreach (var location in service.Locations)
                    {
                        foreach (var item in location.PhoneItems)
                        {
                            //Set Item status to processing.
                            _iOrderService.UpdateItemStatus(item.Id, StatusType.Processing, 0, "", "", null, null, updatingUserId);
                        }
                        foreach (var item in location.VideoItems)
                        {
                            //Set Item status to processing.
                            _iOrderService.UpdateItemStatus(item.Id, StatusType.Processing, 0, "", "", null, null, updatingUserId);
                        }
                        foreach (var item in location.InternetItems)
                        {
                            //Set Item status to processing.
                            _iOrderService.UpdateItemStatus(item.Id, StatusType.Processing, 0, "", "", null, null, updatingUserId);
                        }
                    }
                }
            }

            InvokeOrderJavascript(orderId, companyId, testMode, sendResponse);

            return _iOrderService.RetrieveOrderStatusById(orderId);
        }

        /// <summary>
        /// Undoes the order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="testMode">if set to <c>true</c> [test mode].</param>
        /// <param name="updatingUserId">The updating user identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public OrderStatus UndoOrder(int orderId, int companyId, bool testMode, string updatingUserId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Provisions the service.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="testMode">if set to <c>true</c> [test mode].</param>
        /// <param name="sendResponse">if set to <c>true</c> [send response].</param>
        /// <param name="forceRun">if set to <c>true</c> [force run].</param>
        /// <param name="updatingUserId">The updating user identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">The order has been deleted.
        /// or
        /// The order has been run and errored.
        /// or
        /// The order is currently Processing
        /// or
        /// The order has already been completed.
        /// or
        /// Status not implemented.
        /// or</exception>
        public async Task<ServiceStatus> ProvisionService(int orderId, int serviceId, int companyId, bool testMode, bool sendResponse, bool forceRun, string updatingUserId)
        {
            //get order status
            var order = _iOrderService.RetrieveOrderById(orderId);

            //Check make sure order is not in some other status.
            //If in testmode allow to get past validation because should not be live anyway.
            if (!testMode)
            {
                switch (order.StatusType)
                {
                    case StatusType.Deleted:
                        throw new Exception("The order has been deleted.");
                    case StatusType.Error:
                        if (!forceRun)
                            throw new Exception("The order has been run and errored.");

                        break;
                    case StatusType.Pending:
                        //Do nothing as you should process as expected.
                        break;
                    case StatusType.Processing:
                        //Allow to move on as there may be other services that are still pending.
                        break;
                    case StatusType.Success:
                        if (!forceRun)
                            throw new Exception("The order has already been completed.");

                        break;
                    default:
                        throw new Exception("Status not implemented.");
                }

                //get service status
                var service = _iOrderService.RetrieveServiceById(serviceId);

                switch (service.StatusType)
                {
                    case StatusType.Deleted:
                        throw new Exception("The service has been deleted.");
                    case StatusType.Error:
                        if (!forceRun)
                            throw new Exception("The service has been run and errored.");

                        break;
                    case StatusType.Pending:
                        //Do nothing as you should process as expected.
                        break;
                    case StatusType.Processing:
                        throw new Exception("The service is currently processing");
                    case StatusType.Success:
                        if (!forceRun)
                            throw new Exception("The service has already been completed.");

                        break;
                    default:
                        throw new Exception("This service status not implemented.");
                }
            
                //Set Order status to processing.
                _iOrderService.UpdateOrderStatus(orderId, StatusType.Processing, 0, "", "", null, null, updatingUserId);

                //Set Service status to processing.
                _iOrderService.UpdateServiceStatus(serviceId, StatusType.Processing, 0, "", "", null, null, updatingUserId);

                //Set Service status to processing.
                _iOrderService.UpdateServiceStatus(serviceId, StatusType.Processing, 0, "", "", null, null, updatingUserId);
                foreach (var location in service.Locations)
                {
                    foreach (var item in location.PhoneItems)
                    {
                        //Set Item status to processing.
                        _iOrderService.UpdateItemStatus(item.Id, StatusType.Processing, 0, "", "", null, null, updatingUserId);
                    }
                    foreach (var item in location.VideoItems)
                    {
                        //Set Item status to processing.
                        _iOrderService.UpdateItemStatus(item.Id, StatusType.Processing, 0, "", "", null, null, updatingUserId);
                    }
                    foreach (var item in location.InternetItems)
                    {
                        //Set Item status to processing.
                        _iOrderService.UpdateItemStatus(item.Id, StatusType.Processing, 0, "", "", null, null, updatingUserId);
                    }
                }
            }

            InvokeServiceJavascript(orderId, serviceId, companyId, testMode, sendResponse);
            return _iOrderService.RetrieveServiceStatusById(serviceId);
        }


        /// <summary>
        /// Undoes the service.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="testMode">if set to <c>true</c> [test mode].</param>
        /// <param name="updatingUserId">The updating user identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ServiceStatus UndoService(int orderId, int serviceId, int companyId, bool testMode, string updatingUserId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Provisions the item.
        /// 
        /// Rules for how dispatcher picks up orders based on Action and Item types:
        /// If both tables are null then by default provisioning is all.
        /// If order or service value is chosen in table ProvisioningEngineSettings column ProvisionByMethodTypeId
        /// Then table ProvisioningEngineOrderOrServiceActionTypesSettings applies and depending on the action chosen it will do ALL orders with that action type.
        ///
        /// If item value is chosen in table ProvisioningEngineSettings column ProvisionByMethodTypeId 
        /// Then table ProvisioningEngineActionItemSettings table is used to see what item type and what actions for that item type are used.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="testMode">if set to <c>true</c> [test mode].</param>
        /// <param name="sendResponse">if set to <c>true</c> [send response].</param>
        /// <param name="forceRun">if set to <c>true</c> [force run].</param>
        /// <param name="updatingUserId">The updating user identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// The order has been deleted.
        /// or
        /// The order has been run and errored.
        /// or
        /// The order has already been completed.
        /// or
        /// Status not implemented.
        /// or
        /// The service has been deleted.
        /// or
        /// The service has been run and errored.
        /// or
        /// The service has already been completed.
        /// or
        /// This service status not implemented.
        /// </exception>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<ItemStatus> ProvisionItem(int orderId, int serviceId, int itemId, int companyId, bool testMode, bool sendResponse, bool forceRun, string updatingUserId)
        {
            //get order status
            var order = _iOrderService.RetrieveOrderById(orderId);

            //Check make sure order is not in some other status.
            //If in testmode allow to get past validation because should not be live anyway.
            if (!testMode)
            {
                switch (order.StatusType)
                {
                    case StatusType.Deleted:
                        throw new Exception("The order has been deleted.");
                    case StatusType.Error:
                        if (!forceRun)
                            throw new Exception("The order has been run and errored.");

                        break;
                    case StatusType.Pending:
                        //Do nothing as you should process as expected.
                        break;
                    case StatusType.Processing:
                        //Allow to move on as there may be other services that are still pending.
                    case StatusType.Success:
                        if (!forceRun)
                            throw new Exception("The order has already been completed.");

                        break;
                    default:
                        throw new Exception("Status not implemented.");
                }

                //get service status
                var service = _iOrderService.RetrieveServiceById(serviceId);

                switch (service.StatusType)
                {
                    case StatusType.Deleted:
                        throw new Exception("The service has been deleted.");
                    case StatusType.Error:
                        if (!forceRun)
                            throw new Exception("The service has been run and errored.");

                        break;
                    case StatusType.Pending:
                        //Do nothing as you should process as expected.
                        break;
                    case StatusType.Processing:
                        //Allow to move on as there may be other services that are still pending.
                    case StatusType.Success:
                        if (!forceRun)
                            throw new Exception("The service has already been completed.");

                        break;
                    default:
                        throw new Exception("This service status not implemented.");
                }


                //Set Order status to processing.
                _iOrderService.UpdateOrderStatus(orderId, StatusType.Processing, 0, "", "", null, null, updatingUserId);

                //Set Service status to processing.
                _iOrderService.UpdateServiceStatus(serviceId, StatusType.Processing, 0, "", "", null, null, updatingUserId);

                //Set Service status to processing.
                _iOrderService.UpdateItemStatus(itemId, StatusType.Processing, 0, "", "", null, null, updatingUserId);
            }

            InvokeItemJavascript(orderId, serviceId, itemId, companyId, testMode, sendResponse);
            return _iOrderService.RetrieveItemStatusById(itemId);

        }

        /// <summary>
        /// Undoes the item.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="testMode">if set to <c>true</c> [test mode].</param>
        /// <param name="updatingUserId">The updating user identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ItemStatus UndoItem(int orderId, int serviceId, int itemId, int companyId, bool testMode, string updatingUserId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invokes the item javascript.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="testMode">if set to <c>true</c> [test mode].</param>
        /// <param name="sendResponse">if set to <c>true</c> [send response].</param>
        private void InvokeItemJavascript(int orderId, int serviceId, int itemId, int companyId, bool testMode, bool sendResponse)
        {
            string stdOutput;
            string stdError;
            var engineSetting = _iEngineService.RetrieveProvisioningEngineSetting(companyId);
            var path = Environment.GetEnvironmentVariable("NODE_PATH");

            var list = new List<string>
            {
                path + "scripts\\" + engineSetting.ScriptName,
                orderId.ToString(CultureInfo.InvariantCulture),
                serviceId.ToString(CultureInfo.InvariantCulture),
                itemId.ToString(CultureInfo.InvariantCulture),
                companyId.ToString(CultureInfo.InvariantCulture),
                testMode.ToString(CultureInfo.InvariantCulture),
                sendResponse.ToString(CultureInfo.InvariantCulture),
                "execute"
            };

            _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { list }, list.Aggregate(string.Empty, (current, argument) => current + ("\"" + argument + "\" ")), LogLevelType.Info);

            CommandLineHelper.RunExeWithArguments(
                path + "node",
                list,
                out stdOutput,
                out stdError);

            if (!string.IsNullOrEmpty(stdOutput))
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { list }, "InvokeOrderJavascript Standard Output Logged this: " + stdOutput, LogLevelType.Info);

            if (!string.IsNullOrEmpty(stdError))
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { list }, "InvokeOrderJavascript Standard Error Logged this error: " + stdError, LogLevelType.Error);
        }

        /// <summary>
        /// Invokes the service javascript.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="testMode">if set to <c>true</c> [test mode].</param>
        /// <param name="sendResponse">if set to <c>true</c> [send response].</param>
        private void InvokeServiceJavascript(int orderId, int serviceId, int companyId, bool testMode, bool sendResponse)
        {
            string stdOutput;
            string stdError;
            var engineSetting = _iEngineService.RetrieveProvisioningEngineSetting(companyId);
            var path = Environment.GetEnvironmentVariable("NODE_PATH");

            var list = new List<string>
            {
                path + "scripts\\" + engineSetting.ScriptName,
                orderId.ToString(CultureInfo.InvariantCulture),
                serviceId.ToString(CultureInfo.InvariantCulture),
                "-1", // Item
                companyId.ToString(CultureInfo.InvariantCulture),
                testMode.ToString(CultureInfo.InvariantCulture),
                sendResponse.ToString(CultureInfo.InvariantCulture),
                "execute"
            };

            _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { list }, list.Aggregate(string.Empty, (current, argument) => current + ("\"" + argument + "\" ")), LogLevelType.Info);

            CommandLineHelper.RunExeWithArguments(
                path + "node",
                list,
                out stdOutput,
                out stdError);

            if (!string.IsNullOrEmpty(stdOutput))
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { list }, "InvokeOrderJavascript Standard Output Logged this: " + stdOutput, LogLevelType.Info);

            if (!string.IsNullOrEmpty(stdError))
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { list }, "InvokeOrderJavascript Standard Error Logged this error: " + stdError, LogLevelType.Error);
        }

        /// <summary>
        /// Invokes the order javascript.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="companyId">The company identifier.</param>
        /// <param name="testMode">if set to <c>true</c> [test mode].</param>
        /// <param name="sendResponse">if set to <c>true</c> [send response].</param>
        private void InvokeOrderJavascript(int orderId, int companyId, bool testMode, bool sendResponse)
        {
            string stdOutput;
            string stdError;
            var engineSetting = _iEngineService.RetrieveProvisioningEngineSetting(companyId);
            var path = Environment.GetEnvironmentVariable("NODE_PATH");

            var list = new List<string>
            {
                path + "scripts\\" + engineSetting.ScriptName,
                orderId.ToString(CultureInfo.InvariantCulture),
                "-1", //Service
                "-1", //Item
                companyId.ToString(CultureInfo.InvariantCulture),
                testMode.ToString(CultureInfo.InvariantCulture),
                sendResponse.ToString(CultureInfo.InvariantCulture),
                    "execute"
            };

            _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { list }, list.Aggregate(string.Empty, (current, argument) => current + ("\"" + argument + "\" ")), LogLevelType.Info);

            CommandLineHelper.RunExeWithArguments(
                path + "node",
                list, 
                out stdOutput,
                out stdError);

            if (!string.IsNullOrEmpty(stdOutput))
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { list }, "InvokeOrderJavascript Standard Output Logged this: " + stdOutput, LogLevelType.Info);

            if (!string.IsNullOrEmpty(stdError))
                _logger.WriteLogEntry(_tenantId.ToString(), new List<object> { list }, "InvokeOrderJavascript Standard Error Logged this error: " + stdError, LogLevelType.Error);
        }
    }
}
