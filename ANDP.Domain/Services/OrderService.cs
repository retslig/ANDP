
using System;
using System.Collections.Generic;
using System.Linq;
using ANDP.Lib.Data.Repositories.Equipment;
using ANDP.Lib.Domain.Models;
using ANDP.Lib.Data.Repositories.Order;
using ANDP.Lib.Domain.Interfaces;
using ANDP.Lib.Domain.MappingProfiles;
using Common.Lib.Infastructure;
using Common.Lib.Mapping;
using Company = ANDP.Lib.Domain.Models.Company;
using DaoCompany = ANDP.Lib.Data.Repositories.Equipment.Company;
using DaoOrder = ANDP.Lib.Data.Repositories.Order.Order;
using DaoService = ANDP.Lib.Data.Repositories.Order.Service;
using DaoEquipment = ANDP.Lib.Data.Repositories.Order.Equipment;
using DaoItem = ANDP.Lib.Data.Repositories.Order.Item;
using Order = ANDP.Lib.Domain.Models.Order;
using Service = ANDP.Lib.Domain.Models.Service;

namespace ANDP.Lib.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _iOrderRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly ICommonMapper _iCommonMapper;

        public OrderService(IOrderRepository iOrderRepository, IEquipmentRepository equipmentRepository, ICommonMapper iCommonMapper)
        {
            _iOrderRepository = iOrderRepository;
            _equipmentRepository = equipmentRepository;
            _iCommonMapper = iCommonMapper;
        }

        public Order RetrieveOrderByExtIdAndExtCompanyId(string extOrderId, string externalCompanyId)
        {
            int companyId = _equipmentRepository.RetrieveCompanyIdByExternalCompanyId(externalCompanyId);
            int orderId = _iOrderRepository.RetrieveOrderIdByExtIdAndCompanyId(extOrderId, companyId);
            var daoOrder = _iOrderRepository.RetrieveOrderById(orderId);
            return ObjectFactory.CreateInstanceAndMap<DaoOrder, Order>(_iCommonMapper, daoOrder);
        }

        public IEnumerable<Order> RetrieveProcessingOrders(string externalCompanyId)
        {
            int companyId = _equipmentRepository.RetrieveCompanyIdByExternalCompanyId(externalCompanyId);
            var daoOrders = _iOrderRepository.RetrieveProcessingOrders(companyId);
            return ObjectFactory.CreateInstanceAndMap<IEnumerable<DaoOrder>, IEnumerable<Order>>(_iCommonMapper, daoOrders);
        }

        public Order RetrieveOrderById(int orderId)
        {
            var daoOrder = _iOrderRepository.RetrieveOrderById(orderId);
            return ObjectFactory.CreateInstanceAndMap<DaoOrder, Order>(_iCommonMapper, daoOrder);
        }

        public int RetrieveOrderIdByExtIdAndExtCompanyId(string extOrderId, string externalCompanyId)
        {
            int companyId = _equipmentRepository.RetrieveCompanyIdByExternalCompanyId(externalCompanyId);
            return _iOrderRepository.RetrieveOrderIdByExtIdAndCompanyId(extOrderId, companyId);
        }

        public IEnumerable<Order> RetrieveOrdersByProvisionDateAndExtCompanyId(DateTime date, string externalCompanyId)
        {
            int companyId = _equipmentRepository.RetrieveCompanyIdByExternalCompanyId(externalCompanyId);
            var daoOrders = _iOrderRepository.RetrieveOrdersByProvisionDateAndCompanyId(date, companyId);
            return ObjectFactory.CreateInstanceAndMap<IEnumerable<DaoOrder>, IEnumerable<Order>>(_iCommonMapper, daoOrders);
        }

        public IEnumerable<Order> RetrieveOrdersByStatusTypeAndExtCompanyId(StatusType statusType, string externalCompanyId)
        {
            int companyId = _equipmentRepository.RetrieveCompanyIdByExternalCompanyId(externalCompanyId);
            var daoOrders = _iOrderRepository.RetrieveOrdersByStatusTypeAndCompanyId(statusType.ToStatusTypeEnum(), companyId);
            return ObjectFactory.CreateInstanceAndMap<IEnumerable<DaoOrder>, IEnumerable<Order>>(_iCommonMapper, daoOrders);
        }

        public IEnumerable<Order> RetrieveOrdersByCreateByUserAndExtCompanyId(string userName, string externalCompanyId)
        {
            int companyId = _equipmentRepository.RetrieveCompanyIdByExternalCompanyId(externalCompanyId);
            var daoOrders = _iOrderRepository.RetrieveOrdersByCreateByUserAndCompanyId(userName, companyId);
            return ObjectFactory.CreateInstanceAndMap<IEnumerable<DaoOrder>, IEnumerable<Order>>(_iCommonMapper, daoOrders);
        }

        public IEnumerable<Order> RetrieveOrdersByActionTypeAndExtCompanyId(ActionType actionType, string externalCompanyId)
        {
            int companyId = _equipmentRepository.RetrieveCompanyIdByExternalCompanyId(externalCompanyId);
            var daoOrders = _iOrderRepository.RetrieveOrdersByActionTypeAndCompanyId(actionType.ToActionTypeEnum(), companyId);
            return ObjectFactory.CreateInstanceAndMap<IEnumerable<DaoOrder>, IEnumerable<Order>>(_iCommonMapper, daoOrders);
        }

        public IEnumerable<Order> RetrieveOrdersByPriorityAndExtCompanyId(int priority, string externalCompanyId)
        {
            int companyId = _equipmentRepository.RetrieveCompanyIdByExternalCompanyId(externalCompanyId);
            var daoOrders = _iOrderRepository.RetrieveOrdersByPriorityAndCompanyId(priority, companyId);
            return ObjectFactory.CreateInstanceAndMap<IEnumerable<DaoOrder>, IEnumerable<Order>>(_iCommonMapper, daoOrders);
        }

        public OrderStatus UndoOrderByOrderId(int orderId, string updatingUserId)
        {
            throw new NotImplementedException();
        }

        public OrderStatus UndoOrderByExtIdAndExtCompanyId(string externalOrderId, string externalCompanyId, string updatingUserId)
        {
            throw new NotImplementedException();
        }

        public Order RetrieveNextOrderToProvisionByCompanyIdAndActionType(int companyId, List<ActionType> provisionableActionTypes)
        {
            var actionTypes = provisionableActionTypes.Select(type => (ActionTypeEnum)(int)type).ToList();

            var daoOrder = _iOrderRepository.RetrieveNextOrderToProvisionByCompanyIdAndActionType(companyId, actionTypes);
            return ObjectFactory.CreateInstanceAndMap<DaoOrder, Order>(_iCommonMapper, daoOrder);
        }

        public Service RetrieveNextServiceToProvisionByCompanyIdAndActionType(int companyId, List<ActionType> provisionableOrderTypes)
        {
            var actionTypes = provisionableOrderTypes.Select(type => (ActionTypeEnum)(int)type).ToList();

            var daoService = _iOrderRepository.RetrieveNextServiceToProvisionByCompanyIdAndActionType(companyId, actionTypes);
            return ObjectFactory.CreateInstanceAndMap<DaoService, Service>(_iCommonMapper, daoService);
        }

        public object RetrieveNextItemToProvisionByCompanyIdAndActionTypeAndItemType(int companyId, List<ItemActionType> provisionableItemTypes)
        {
            var provisionableItemActionTypes = provisionableItemTypes.ToDictionary(provisionableItemActionType =>
                (ItemTypeEnum) (int) provisionableItemActionType.ItemType,
                provisionableItemActionType =>
                    provisionableItemActionType.ActionTypes.Select(type => (ActionTypeEnum) (int) type).ToList());

            var daoItem = _iOrderRepository.RetrieveNextItemToProvisionByCompanyIdAndActionTypesAndItemType(companyId, provisionableItemActionTypes);

            if (daoItem == null)
                return null;

            if (daoItem.Xml.Contains("<PhoneItem>"))
                return ObjectFactory.CreateInstanceAndMap<DaoItem, PhoneItem>(_iCommonMapper, daoItem);

            if (daoItem.Xml.Contains("<VideoItem>"))
                return ObjectFactory.CreateInstanceAndMap<DaoItem, VideoItem>(_iCommonMapper, daoItem);
                
            if (daoItem.Xml.Contains("<InternetItem>"))
                return ObjectFactory.CreateInstanceAndMap<DaoItem, InternetItem>(_iCommonMapper, daoItem);

            return null;
        }

        public OrderStatus DeleteOrderByExtIdAndExtCompanyId(string externalOrderId, string externalCompanyId, string updatingUserId)
        {
            throw new NotImplementedException();
        }

        public OrderStatus RetrieveOrderStatusById(int orderId)
        {
            var daoOrder = _iOrderRepository.RetrieveOrderById(orderId);
            return ObjectFactory.CreateInstanceAndMap<DaoOrder, OrderStatus>(_iCommonMapper, daoOrder);
        }

        public IEnumerable<Order> RetrieveCompletedOrders(int companyId)
        {
            var daoOrder = _iOrderRepository.RetrieveCompletedOrders(companyId);
            return ObjectFactory.CreateInstanceAndMap<IEnumerable<DaoOrder>, IEnumerable<Order>>(_iCommonMapper, daoOrder);
        }

        public OrderStatus RetrieveOrderStatusByExtIdAndExtCompanyId(string extOrderId, string externalCompanyId)
        {
            throw new NotImplementedException();
        }


        public ServiceStatus UpdateServiceStatus(int serviceId, StatusType statusType, int priority, string resultMessage, string log,
            DateTime? startDate, DateTime? compleDateTime, string updatingUserId)
        {
            var service = _iOrderRepository.UpdateServiceStatus(serviceId, statusType.ToStatusTypeEnum(), 0, resultMessage, log, startDate, compleDateTime, updatingUserId);
            return ObjectFactory.CreateInstanceAndMap<DaoService, ServiceStatus>(_iCommonMapper, service);
        }

        public IEnumerable<Service> RetrieveRecentlyProvisionedServices(int numberReturned)
        {
            var daoServices = _iOrderRepository.RetrieveRecentlyProvisionedServices(numberReturned);
            return ObjectFactory.CreateInstanceAndMap<IEnumerable<DaoService>, IEnumerable<Service>>(_iCommonMapper, daoServices);
        }

        public int RetrieveServiceIdByExtOrderIdAndExtIdAndExtCompanyId(string externalOrderId, string extServiceId, string externalCompanyId)
        {
            int companyId = _equipmentRepository.RetrieveCompanyIdByExternalCompanyId(externalCompanyId);
            int orderId = _iOrderRepository.RetrieveOrderIdByExtIdAndCompanyId(externalOrderId, companyId);
            return _iOrderRepository.RetrieveServiceIdByOrderIdAndExtServiceIdAndCompanyId(orderId, extServiceId, companyId);
        }

        public Service RetrieveServiceByExtOrderIdAndExtIdAndExtCompanyId(string externalOrderId, string externalServiceId, string externalCompanyId)
        {
            int companyId = _equipmentRepository.RetrieveCompanyIdByExternalCompanyId(externalCompanyId);
            int orderId = _iOrderRepository.RetrieveOrderIdByExtIdAndCompanyId(externalOrderId, companyId);
            var daoService = _iOrderRepository.RetrieveServiceByOrderIdAndExtServiceIdAndCompanyId(orderId, externalServiceId, companyId);
            return ObjectFactory.CreateInstanceAndMap<DaoService, Service>(_iCommonMapper, daoService);
        }

        public Service RetrieveServiceById(int serviceId)
        {
            var daoService = _iOrderRepository.RetrieveServiceById(serviceId);
            return ObjectFactory.CreateInstanceAndMap<DaoService, Service>(_iCommonMapper, daoService);
        }

        public ServiceStatus RetrieveServiceStatusById(int serviceId)
        {
            var daoService = _iOrderRepository.RetrieveServiceById(serviceId);
            return ObjectFactory.CreateInstanceAndMap<DaoService, ServiceStatus>(_iCommonMapper, daoService);
        }

        public int RetrieveCompanyIdByExtCompanyId(string externalCompanyId)
        {
            return _equipmentRepository.RetrieveCompanyIdByExternalCompanyId(externalCompanyId);
        }

        public IEnumerable<Company> RetrieveAllCompanies()
        {
            var daoCompanies = _equipmentRepository.RetrieveAllCompanies();
            return ObjectFactory.CreateInstanceAndMap < IEnumerable<DaoCompany>, IEnumerable<Company>>(_iCommonMapper, daoCompanies);
        }

        public object RetrieveItemById( int itemId)
        {
            var daoItem = _iOrderRepository.RetrieveItemById(itemId);

            if (daoItem == null)
                return null;

            if (daoItem.Xml.Contains("<PhoneItem>"))
                return ObjectFactory.CreateInstanceAndMap<DaoItem, PhoneItem>(_iCommonMapper, daoItem);

            if (daoItem.Xml.Contains("<VideoItem>"))
                return ObjectFactory.CreateInstanceAndMap<DaoItem, VideoItem>(_iCommonMapper, daoItem);
                
            if (daoItem.Xml.Contains("<InternetItem>"))
                return ObjectFactory.CreateInstanceAndMap<DaoItem, InternetItem>(_iCommonMapper, daoItem);

            return null;
        }

        public ItemStatus RetrieveItemStatusById(int itemId)
        {
            var daoItem = _iOrderRepository.RetrieveItemById(itemId);
            return ObjectFactory.CreateInstanceAndMap<DaoItem, ItemStatus>(_iCommonMapper, daoItem);
        }

        public ItemStatus UpdateItemStatus(int itemId, StatusType statusType, int priority, string resultMessage, string log, DateTime? startDate,
            DateTime? compleDateTime, string updatingUserId)
        {
            var item = _iOrderRepository.UpdateItemStatus(itemId, statusType.ToStatusTypeEnum(), 0, resultMessage, log, startDate, compleDateTime, updatingUserId);
            return ObjectFactory.CreateInstanceAndMap<DaoItem, ItemStatus>(_iCommonMapper, item);
        }

        public EquipmentStatus UpdateEquipmentStatus(int itemId, int equipmentId, int equipmentItemId, StatusType statusType, string resultMessage,
            string log, string name, DateTime? startDate, DateTime? compleDateTime, string updatingUserId)
        {
            var equipment = _iOrderRepository.UpdateEquipmentStatus(itemId, equipmentId, equipmentItemId, statusType.ToStatusTypeEnum(), resultMessage, log, name, startDate, compleDateTime, updatingUserId);
            return ObjectFactory.CreateInstanceAndMap<DaoEquipment, EquipmentStatus>(_iCommonMapper, equipment);
        }

        public ServiceStatus UpdateServiceResult(ServiceProvisioningResult serviceProvisioningResult, string updatingUserId)
        {
            if (serviceProvisioningResult.Id < 1)
                throw new Exception("ServiceId is a mandatory field and you are missing this.");

            UpdateServiceStatus(serviceProvisioningResult.Id, serviceProvisioningResult.StatusType, 0, serviceProvisioningResult.ErrorMessage,
                   serviceProvisioningResult.Log, serviceProvisioningResult.StartDate, serviceProvisioningResult.CompletionDate, updatingUserId);

            var statusTypes = new[] { StatusType.Pending, StatusType.Processing };

            //Get current service so we have the orderId.
            var service = RetrieveServiceById(serviceProvisioningResult.Id);

            //Now that all status have been updated on services re-query db and see if all services have now been provisioned.
            var order = RetrieveOrderById(service.OrderId);

            string serviceErrors = order.Services.Where(p => p.StatusType == StatusType.Error).Aggregate("", (current, error) => current + (error.ResultMessage + Environment.NewLine));
            string serviceLogs = order.Services.Aggregate("", (current, error) => current + (error.Log + Environment.NewLine));

            if (!order.Services.Any(p => statusTypes.Contains(p.StatusType)))
            {
                //must fetch each service and make none errored.
                var statusType = StatusType.Success;
                if (order.Services.Any(p => p.StatusType == StatusType.Error))
                    statusType = StatusType.Error;

                UpdateOrderStatus(
                    order.Id,
                    statusType,
                    0,
                    serviceErrors,
                    serviceLogs,
                    null,
                    null,
                    updatingUserId);
            }
            else
            {
                //If all services are not complete then set back to pending to get picked up again.
                UpdateOrderStatus(
                    order.Id, StatusType.Pending,
                    0,
                    "",
                    "",
                    null,
                    null,
                    updatingUserId);
            }

            //Now get the orderstatus
            return RetrieveServiceStatusById(serviceProvisioningResult.Id);
        }

        public ItemStatus UpdateItemResult(ItemProvisioningResult itemProvisioningResult, string updatingUserId)
        {
            if (itemProvisioningResult.Id < 1)
                throw new Exception("ItemId is a mandatory field and you are missing this.");

            int count = 1;
            foreach (var equipment in itemProvisioningResult.EquipmentResults)
            {
                if (equipment.Id < 1)
                    throw new Exception("EquipmentId is a mandatory field and you are missing this.");

                UpdateEquipmentStatus(itemProvisioningResult.Id, equipment.Id, count, equipment.StatusType, equipment.ErrorMessage,
                    equipment.Log, equipment.Name, equipment.StartDate, equipment.CompletionDate, updatingUserId);
                count++;
            }

            //Set item status to results.
            UpdateItemStatus(itemProvisioningResult.Id, itemProvisioningResult.StatusType, 0, itemProvisioningResult.ErrorMessage,
                itemProvisioningResult.Log, itemProvisioningResult.StartDate, itemProvisioningResult.CompletionDate, updatingUserId);

            //Get current service so we have the orderId.
            var domainItem = RetrieveItemById(itemProvisioningResult.Id);
            int serviceId = 0;

            if (domainItem is PhoneItem)
            {
                var phoneItem = domainItem as PhoneItem;
                serviceId = phoneItem.ServiceId;
            }

            if (domainItem is VideoItem)
            {
                var videoItem = domainItem as VideoItem;
                serviceId = videoItem.ServiceId;
            }

            if (domainItem is InternetItem)
            {
                var internetItem = domainItem as InternetItem;
                serviceId = internetItem.ServiceId;
            }

            bool phoneItemsPending = false;
            bool videoItemsPending = false;
            bool internetItemsPending = false;

            bool itemError = false;
            string phoneItemErrors = string.Empty;
            string videoItemsErrors = string.Empty;
            string internetItemsErrors = string.Empty;
            string phoneItemLogs = string.Empty;
            string videoItemsLogs = string.Empty;
            string internetItemsLogs = string.Empty;

            var statusTypes = new[] { StatusType.Pending, StatusType.Processing };

            //Get current service so we have the orderId.
            var service = RetrieveServiceById(serviceId);
            foreach (var location in service.Locations)
            {
                itemError = location.PhoneItems.Any(p => p.StatusType == StatusType.Error) ||
                    location.VideoItems.Any(p => p.StatusType == StatusType.Error) ||
                    location.InternetItems.Any(p => p.StatusType == StatusType.Error);

                phoneItemErrors = location.PhoneItems.Where(p => p.StatusType == StatusType.Error).Aggregate("", (current, error) => current + (error.ResultMessage + Environment.NewLine));
                videoItemsErrors = location.VideoItems.Where(p => p.StatusType == StatusType.Error).Aggregate("", (current, error) => current + (error.ResultMessage + Environment.NewLine));
                internetItemsErrors = location.InternetItems.Where(p => p.StatusType == StatusType.Error).Aggregate("", (current, error) => current + (error.ResultMessage + Environment.NewLine));

                phoneItemLogs = location.PhoneItems.Aggregate("", (current, error) => current + (error.Log + Environment.NewLine));
                videoItemsLogs = location.VideoItems.Aggregate("", (current, error) => current + (error.Log + Environment.NewLine));
                internetItemsLogs = location.InternetItems.Aggregate("", (current, error) => current + (error.Log + Environment.NewLine));

                phoneItemsPending = location.PhoneItems.Any(p => statusTypes.Contains(p.StatusType));
                videoItemsPending = location.VideoItems.Any(p => statusTypes.Contains(p.StatusType));
                internetItemsPending = location.InternetItems.Any(p => statusTypes.Contains(p.StatusType));
            }

            if (!(phoneItemsPending || videoItemsPending || internetItemsPending))
            {
                //must fetch each service and make sure none errored.
                var statusType = StatusType.Success;
                if (itemError)
                    statusType = StatusType.Error;

                UpdateServiceStatus(
                    service.Id,
                    statusType,
                    0,
                    phoneItemErrors + videoItemsErrors + internetItemsErrors,
                    phoneItemLogs + videoItemsLogs + internetItemsLogs,
                    null,
                    null,
                    updatingUserId);
            }

            //Now that all status have been updated on services re-query db and see if all services have now been provisioned.
            var order = RetrieveOrderById(service.OrderId);

            string serviceErrors = order.Services.Where(p => p.StatusType == StatusType.Error).Aggregate("", (current, error) => current + (error.ResultMessage + Environment.NewLine));
            string serviceLogs = order.Services.Aggregate("", (current, error) => current + (error.Log + Environment.NewLine));

            if (!order.Services.Any(p => statusTypes.Contains(p.StatusType)))
            {
                //must fetch each service and make none errored.
                var statusType = StatusType.Success;
                if (order.Services.Any(p => p.StatusType == StatusType.Error))
                    statusType = StatusType.Error;

                UpdateOrderStatus(
                    order.Id,
                    statusType,
                    0,
                    serviceErrors,
                    serviceLogs,
                    null,
                    null,
                    updatingUserId);
            }
            else
            {
                //If all services are not complete then set back to pending to get picked up again.
                UpdateOrderStatus(
                    order.Id, StatusType.Pending,
                    0,
                    "",
                    "",
                    null,
                    null,
                    updatingUserId);
            }

            //Now get the orderstatus
            return RetrieveItemStatusById(itemProvisioningResult.Id);
        }

        public OrderStatus UpdateEquipmentResult(EquipmentProvisioningResult equipmentProvisioningResult, string updatingUserId)
        {
            throw new NotImplementedException();
        }

        public OrderStatus CreateOrUpdateOrder(Order order, string updatingUserId)
        {
            int companyId = RetrieveCompanyIdByExtCompanyId(order.ExternalCompanyId);

            //On a update or create we want to make sure a few columns do have data.
            order.Log = "";
            order.ResultMessage = "";
            order.ResponseSent = false;
            order.StartDate = null;
            order.CompletionDate = null;


            order.Validate(new CustomValidationService(_iOrderRepository, companyId));
            if (order.ValidationErrors.Any())
            {
                string errors = order.ValidationErrors.Aggregate("", (current, error) => current + (error + Environment.NewLine));
                throw new Exception("Order Creation Failed:" + Environment.NewLine + errors); 
            }

            var daoOrder = ObjectFactory.CreateInstanceAndMap<Order, DaoOrder>(_iCommonMapper, order);
            daoOrder = _iOrderRepository.CreateOrUpdateOrder(daoOrder, updatingUserId);
            return ObjectFactory.CreateInstanceAndMap<DaoOrder, OrderStatus>(_iCommonMapper, daoOrder);
        }

        public void UpdateOrderSentResponseFlag(int orderId, bool sent, string updatingUserId)
        {
            _iOrderRepository.UpdateOrderSentResponseFlag(orderId, sent, updatingUserId);
        }

        public OrderStatus DeleteOrder(Order order, string updatingUserId)
        {
            var daoOrder = ObjectFactory.CreateInstanceAndMap<Order, DaoOrder>(_iCommonMapper, order);
            daoOrder = _iOrderRepository.DeleteOrder(daoOrder, updatingUserId);
            return ObjectFactory.CreateInstanceAndMap<DaoOrder, OrderStatus>(_iCommonMapper, daoOrder);
        }

        public OrderStatus DeleteOrderById(int id, string updatingUserId)
        {
            throw new NotImplementedException();
        }

        public Service AnyServicesLeftOnOrder(int serviceId)
        {
            throw new NotImplementedException();
        }

        public OrderStatus UpdateOrderStatus(int orderId, StatusType statusType, int priority, string resultMessage, string log, DateTime? startDate,
            DateTime? compleDateTime, string updatingUserId)
        {
            if (orderId < 1)
                throw new Exception("OrderId is a mandatory field and you are missing this.");

            var order = _iOrderRepository.UpdateOrderStatus(orderId, statusType.ToStatusTypeEnum(), priority, resultMessage, log, startDate, compleDateTime, updatingUserId);
            return ObjectFactory.CreateInstanceAndMap<DaoOrder, OrderStatus>(_iCommonMapper, order);
        }

        public OrderStatus UpdateOrder(Order order, string updatingUserId)
        {
            int companyId = RetrieveCompanyIdByExtCompanyId(order.ExternalCompanyId);

            order.Validate(new CustomValidationService(_iOrderRepository, companyId));
            if (order.ValidationErrors.Any())
            {
                string errors = order.ValidationErrors.Aggregate("", (current, error) => current + (error + Environment.NewLine));
                throw new Exception("Update Failed:" + Environment.NewLine + errors); 
            }

            //todo: quick fix to set these properties as when a order comes in end user doesn't want to have to set all this.
            //the attach should look for child objects.
            foreach (var service in order.Services)
            {
                service.CreatedByUser = updatingUserId;
                service.ModifiedByUser = updatingUserId;
                service.DateCreated = DateTime.Now;
                service.DateModified = DateTime.Now;
                //service.Version = 0;

                foreach (var location in service.Locations)
                {
                    if (location.InternetItems != null)
                    {
                        foreach (var item in location.InternetItems)
                        {
                            item.CreatedByUser = updatingUserId;
                            item.ModifiedByUser = updatingUserId;
                            item.DateCreated = DateTime.Now;
                            item.DateModified = DateTime.Now;
                            //item.Version = 0;
                        }
                    }

                    if (location.PhoneItems != null)
                    {
                        foreach (var item in location.PhoneItems)
                        {
                            item.CreatedByUser = updatingUserId;
                            item.ModifiedByUser = updatingUserId;
                            item.DateCreated = DateTime.Now;
                            item.DateModified = DateTime.Now;
                            //item.Version = 0;
                        }
                    }

                    if (location.VideoItems != null)
                    {
                        foreach (var item in location.VideoItems)
                        {
                            item.CreatedByUser = updatingUserId;
                            item.ModifiedByUser = updatingUserId;
                            item.DateCreated = DateTime.Now;
                            item.DateModified = DateTime.Now;
                            //item.Version = 0;
                        }
                    }
                }
            }

            var daoOrder = ObjectFactory.CreateInstanceAndMap<Order, DaoOrder>(_iCommonMapper, order);
            daoOrder = _iOrderRepository.UpdateOrder(daoOrder, updatingUserId);
            return ObjectFactory.CreateInstanceAndMap<DaoOrder, OrderStatus>(_iCommonMapper, daoOrder);
        }
    }
}
