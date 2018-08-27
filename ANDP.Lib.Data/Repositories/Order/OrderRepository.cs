
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace ANDP.Lib.Data.Repositories.Order
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IANDP_Order_Entities _iandpOrderEntities;

        public OrderRepository(IANDP_Order_Entities iandpOrderEntities)
        {
            _iandpOrderEntities = iandpOrderEntities;
        }

        public Order CreateOrUpdateOrder(Order order, string updatingUserId)
        {
            var daoOrder = _iandpOrderEntities.Orders.AsNoTracking().FirstOrDefault(p => p.ExternalOrderId == order.ExternalOrderId);
            var services = order.Services.ToList();
            var items = new List<Item>();
            foreach (var service in services)
            {
                items.AddRange(service.Items.ToList());
            }
            
            if (daoOrder != null && daoOrder.Id > 0)
            {
                //If the order is not pending or errored we do not want to accept the order.
                var orderStatusTypes = new[] { StatusTypeEnum.Error, StatusTypeEnum.Pending };
                if (!orderStatusTypes.Contains((StatusTypeEnum)daoOrder.StatusTypeId))
                    throw new Exception("This order cannot be accepted because it has a status of " + ((StatusTypeEnum)daoOrder.StatusTypeId).ToString());

                order.CreatedByUser = daoOrder.CreatedByUser;
                order.ModifiedByUser = updatingUserId;
                order.Id = daoOrder.Id;
                order.DateCreated = daoOrder.DateCreated;
                order.DateModified = DateTime.Now;
                order.Version = daoOrder.Version;
                foreach (var service in services)
                {
                    var daoService = daoOrder.Services.FirstOrDefault(p => p.ExternalServiceId == service.ExternalServiceId);
                    if (daoService != null && daoService.Id > 0)
                    {
                        service.CreatedByUser = daoService.CreatedByUser;
                        service.ModifiedByUser = updatingUserId;
                        service.OrderId = daoOrder.Id;
                        service.Id = daoService.Id;
                        service.DateCreated = daoService.DateCreated;
                        service.DateModified = DateTime.Now;
                        service.Version = daoService.Version;
                        service.Items = null;
                        _iandpOrderEntities.AttachEntity(null, service, new[] { "Id" }, updatingUserId);
                        _iandpOrderEntities.SaveChanges();
                        _iandpOrderEntities.ClearContextEntries();
                        foreach (var item in items)
                        {
                            var daoItem = _iandpOrderEntities.Items.AsNoTracking().FirstOrDefault(p => p.ExternalItemId == item.ExternalItemId && p.ServiceId == daoService.Id);
                            if (daoItem != null && daoItem.Id > 0)
                            {
                                item.CreatedByUser = daoItem.CreatedByUser;
                                item.ModifiedByUser = updatingUserId;
                                item.Id = daoItem.Id;
                                item.ServiceId = daoService.Id;
                                item.DateCreated = daoItem.DateCreated;
                                item.DateModified = DateTime.Now;
                                item.Version = daoItem.Version;
                                _iandpOrderEntities.AttachEntity(null, item, new[] { "Id" }, updatingUserId);
                            }
                        }

                        _iandpOrderEntities.SaveChanges();
                        _iandpOrderEntities.RefreshAll();
                    }
                }
            }
            else
            {
                order.CreatedByUser = updatingUserId;
                order.ModifiedByUser = updatingUserId;
                order.DateCreated = DateTime.Now;
                order.DateModified = DateTime.Now;
                order.Version = 0;

                foreach (var service in order.Services)
                {
                    service.CreatedByUser = updatingUserId;
                    service.ModifiedByUser = updatingUserId;
                    service.DateCreated = DateTime.Now;
                    service.DateModified = DateTime.Now;
                    service.Version = 0;

                    foreach (var item in service.Items)
                    {
                        item.CreatedByUser = updatingUserId;
                        item.ModifiedByUser = updatingUserId;
                        item.DateCreated = DateTime.Now;
                        item.DateModified = DateTime.Now;
                        item.Version = 0;
                    }
                }
            }

            _iandpOrderEntities.AttachEntity(null, order, new[] { "Id" }, updatingUserId);
            _iandpOrderEntities.SaveChanges();
            _iandpOrderEntities.RefreshEntity(order);
            return order;
        }

        public Order UpdateOrder(Order order, string updatingUserId)
        {
            var daoOrder = _iandpOrderEntities.Orders.FirstOrDefault(p => p.ExternalOrderId == order.ExternalOrderId);
            if (daoOrder == null || daoOrder.Id < 1)
                throw new Exception("Order not found.");

            order.ModifiedByUser = updatingUserId;
            order.DateModified = DateTime.Now;

            _iandpOrderEntities.ClearContextEntries();
            _iandpOrderEntities.AttachEntity(null, order, new[] { "Id" }, updatingUserId);
            _iandpOrderEntities.SaveChanges();
            _iandpOrderEntities.RefreshEntity(order);
            return order;
        }

        public void UpdateOrderSentResponseFlag(int orderId, bool sent, string updatingUserId)
        {
            var daoOrder = _iandpOrderEntities.Orders.AsNoTracking().FirstOrDefault(x => x.Id == orderId);

            if (daoOrder != null)
            {
                daoOrder.ResponseSent = sent;
                _iandpOrderEntities.AttachEntity(null, daoOrder, new[] { "Id" }, updatingUserId);
                _iandpOrderEntities.SaveChanges();
            }
        }

        public Order DeleteOrder(Order order, string updatingUserId)
        {
            _iandpOrderEntities.Orders.Remove(order);
            _iandpOrderEntities.SaveChanges();
            _iandpOrderEntities.RefreshEntity(order);
            return order;
        }

        public Order DeleteOrderByExtId(string extId, string updatingUserId)
        {
            var daoOrder = _iandpOrderEntities.Orders.FirstOrDefault(x => x.ExternalOrderId == extId);

            if (daoOrder != null)
            {
                foreach (var service in daoOrder.Services.ToList())
                {
                    DeleteServiceById(service.Id);
                }

                _iandpOrderEntities.Orders.Remove(daoOrder);
                _iandpOrderEntities.SaveChanges();
            }

            return _iandpOrderEntities.Orders.FirstOrDefault(x => x.ExternalOrderId == extId);
        }

        public Order DeleteOrderById(int id, string updatingUserId)
        {
            var daoOrder = _iandpOrderEntities.Orders.FirstOrDefault(x => x.Id == id);

            if (daoOrder != null)
            {
                foreach (var service in daoOrder.Services.ToList())
                {
                    DeleteServiceById(service.Id);
                }

                _iandpOrderEntities.Orders.Remove(daoOrder);
                _iandpOrderEntities.SaveChanges();
            }

            return _iandpOrderEntities.Orders.FirstOrDefault(x => x.Id == id);
        }
        
        public Order RetrieveOrderById(int orderId)
        {
            //Have to no 'AsNoTracking()' so that entities doesn't track the entity and throw an error 
            //if we decided to push back changes after we've mapped it back and forth from domain to dao.

            return _iandpOrderEntities.Orders.AsNoTracking().FirstOrDefault(x => x.Id == orderId);
        }

        public int RetrieveOrderIdByExtIdAndCompanyId(string extOrderId, int companyId)
        {
            var orderId = (from o in _iandpOrderEntities.Orders
                           join com in _iandpOrderEntities.Companies on o.ExternalCompanyId equals com.ExternalCompanyId
                           where o.ExternalOrderId == extOrderId
                                && com.Id == companyId
                           select o.Id).FirstOrDefault();

            return orderId;
        }

        public IEnumerable<Order> RetrieveCompletedOrders(int companyId)
        {
            var orderStatus = new List<int> { (int)StatusTypeEnum.Success, (int)StatusTypeEnum.Error };
            return _iandpOrderEntities.Orders.Where(p => p.ResponseSent == false && orderStatus.Contains(p.StatusTypeId)).ToList();
        }

        public IEnumerable<Order> RetrieveOrdersByProvisionDateAndCompanyId(DateTime date, int companyId)
        {
            var daoOrders = (from o in _iandpOrderEntities.Orders
                             join c in _iandpOrderEntities.Companies on o.ExternalCompanyId equals c.ExternalCompanyId
                             //where o.ProvisionDate.Date == date.Date  Can't do this in linq...
                             //where DateTime.Compare(o.ProvisionDate.Date, date.Date) == 0 Can't do this in linq...
                             where o.ProvisionDate.Year == date.Year
                                && o.ProvisionDate.Month == date.Month
                                && o.ProvisionDate.Day == date.Day
                                && c.Id == companyId
                             select o).ToList();

            return daoOrders;
        }

        public IEnumerable<Order> RetrieveOrdersByStatusTypeAndCompanyId(StatusTypeEnum statusType, int companyId)
        {
            //Have to no 'AsNoTracking()' so that entities doesn't track the entity and throw an error 
            //if we decided to push back changes after we've mapped it back and forth from domain to dao.

            var daoOrders = (from o in _iandpOrderEntities.Orders
                             join comp in _iandpOrderEntities.Companies on o.ExternalCompanyId equals comp.ExternalCompanyId
                             where o.StatusTypeId == (int)statusType
                                && comp.Id == companyId
                             select o).ToList();

            return daoOrders;
        }

        public IEnumerable<Order> RetrieveOrdersByCreateByUserAndCompanyId(string userName, int companyId)
        {
            var daoOrders = (from o in _iandpOrderEntities.Orders
                             join c in _iandpOrderEntities.Companies on o.ExternalCompanyId equals c.ExternalCompanyId
                             where o.CreatedByUser == userName
                                && c.Id == companyId
                             select o).ToList();

            return daoOrders;
        }

        public IEnumerable<Order> RetrieveOrdersByActionTypeAndCompanyId(ActionTypeEnum actionType, int companyId)
        {
            var daoOrders = (from o in _iandpOrderEntities.Orders
                             join c in _iandpOrderEntities.Companies on o.ExternalCompanyId equals c.ExternalCompanyId
                             where o.ActionTypeId == (int) actionType
                                  && c.Id == companyId
                             select o).ToList();

            return daoOrders;
        }

        public IEnumerable<Order> RetrieveOrdersByPriorityAndCompanyId(int priority, int companyId)
        {
            var daoOrders = (from o in _iandpOrderEntities.Orders
                             join c in _iandpOrderEntities.Companies on o.ExternalCompanyId equals c.ExternalCompanyId
                             where o.Priority == priority && c.Id == companyId
                             select o).ToList();

            return daoOrders;
        }

        public IEnumerable<Order> RetrieveProcessingOrders(int companyId)
        {
            var daoOrders = (from o in _iandpOrderEntities.Orders
                             join c in _iandpOrderEntities.Companies on o.ExternalCompanyId equals c.ExternalCompanyId
                             where c.Id == companyId && o.StatusTypeId == (int)StatusTypeEnum.Processing
                             select o).ToList();

            return daoOrders;
        }

        public Order UndoOrder(int orderId, string updatingUserId)
        {
            //No need to implement yet Brent
            
            throw new NotImplementedException();
        }

        public Service RetrieveServiceById(int serviceId)
        {
            var daoService = _iandpOrderEntities.Services.FirstOrDefault(p => p.Id == serviceId);
            return daoService;
        }

        public Order RetrieveNextOrderToProvisionByCompanyIdAndActionType(int companyId,
             List<ActionTypeEnum> provisionableActionTypes)
        {
            //By default if no types are given then assume all.
            var actionStatuses = (provisionableActionTypes == null || !provisionableActionTypes.Any()) ?
                Enum.GetValues(typeof(ActionTypeEnum)).Cast<int>().ToList() :
                provisionableActionTypes.Select(actionType => (int)actionType).ToList();

            //If we are provisionByOrder then we only want pending orders. 
            var orderStatus = new List<int> { (int)StatusTypeEnum.Pending };

            //Rules for getting next order companyid 1, status 2, priority 3, provision date 4, fifo 5
            var daoOrder = (from o in _iandpOrderEntities.Orders
                join comp in _iandpOrderEntities.Companies on o.ExternalCompanyId equals comp.ExternalCompanyId
                where orderStatus.Contains(o.StatusTypeId)
                      && actionStatuses.Contains(o.ActionTypeId)
                      && o.ProvisionDate <= DateTime.Now
                      && comp.Id == companyId
                select o).OrderBy(o => o.Priority)
                .ThenBy(o => o.ProvisionDate)
                .ThenBy(o => o.DateCreated);

            return daoOrder.FirstOrDefault();
        }

        public Service RetrieveNextServiceToProvisionByCompanyIdAndActionType(int companyId,
             List<ActionTypeEnum> provisionableActionTypes)
        {
            //By default if no types are given then assume all.
            var actionStatuses = (provisionableActionTypes == null || !provisionableActionTypes.Any()) ?
                Enum.GetValues(typeof(ActionTypeEnum)).Cast<int>().ToList() :
                provisionableActionTypes.Select(actionType => (int)actionType).ToList();

            var serviceStatus = new List<int> { (int)StatusTypeEnum.Pending };

            //Rules for getting next order companyid 1, status 2, priority 3, provision date 4, fifo 5
            var daoOrder = (from service in _iandpOrderEntities.Services
                            join order in _iandpOrderEntities.Orders on service.OrderId equals order.Id
                            join comp in _iandpOrderEntities.Companies on order.ExternalCompanyId equals comp.ExternalCompanyId
                            where actionStatuses.Contains(service.ActionTypeId)
                              && serviceStatus.Contains(service.StatusTypeId)
                              && service.ProvisionDate <= DateTime.Now
                              && comp.Id == companyId
                        select service).OrderBy(o => o.Priority)
                .ThenBy(o => o.ProvisionDate)
                .ThenBy(o => o.DateCreated)
                .FirstOrDefault();

            return daoOrder;
        }

        public Item RetrieveNextItemToProvisionByCompanyIdAndActionTypesAndItemType(int companyId, 
            Dictionary<ItemTypeEnum,List<ActionTypeEnum>> provisionableItemActionTypes)
        {
            var videoActionTypes = new List<int>();
            var internetActionTypes = new List<int>();
            var phoneActionTypes = new List<int>();

            if (provisionableItemActionTypes == null || !provisionableItemActionTypes.Any())
                throw new Exception("You must provide at least item and one action type.");

            foreach (var provisionableItemActionType in provisionableItemActionTypes)
            {
                switch (provisionableItemActionType.Key)
                {
                    case ItemTypeEnum.Video:
                        if (provisionableItemActionType.Value == null || !provisionableItemActionType.Value.Any())
                            throw new Exception("You must provide at least one action type for video items.");

                        videoActionTypes = provisionableItemActionType.Value.Select(actionType => (int)actionType).ToList();
                        break;
                    case ItemTypeEnum.Internet:
                        if (provisionableItemActionType.Value == null || !provisionableItemActionType.Value.Any())
                            throw new Exception("You must provide at least one action type for internet items.");

                        internetActionTypes = provisionableItemActionType.Value.Select(actionType => (int)actionType).ToList();
                        break;
                    case ItemTypeEnum.Phone:
                        if (provisionableItemActionType.Value == null || !provisionableItemActionType.Value.Any())
                            throw new Exception("You must provide at least one action type for phone items.");

                        phoneActionTypes = provisionableItemActionType.Value.Select(actionType => (int)actionType).ToList();
                        break;
                    default:
                        throw new NotImplementedException("This item type is not implemented. Enum number:" + (int)provisionableItemActionType.Key);
                }
            }

            var itemStatus = new List<int> { (int)StatusTypeEnum.Pending };

            //If we are provisionByOrder then we only want pending orders. 
            //Otherwise if we are by service we the order might be processing but the services still might need provisioning.
            //Rules for getting next order companyid 1, status 2, priority 3, provision date 4, itemtype 5, fifo 6
            var daoOrder = (from item in _iandpOrderEntities.Items
                            join service in _iandpOrderEntities.Services on item.ServiceId equals service.Id
                            join order in _iandpOrderEntities.Orders on service.OrderId equals order.Id
                            join comp in _iandpOrderEntities.Companies on order.ExternalCompanyId equals comp.ExternalCompanyId
                            where (
                                (item.ItemTypeId == (int)ItemTypeEnum.Video && videoActionTypes.Contains(item.ActionTypeId))
                                || (item.ItemTypeId == (int)ItemTypeEnum.Internet && internetActionTypes.Contains(item.ActionTypeId))
                                || (item.ItemTypeId == (int)ItemTypeEnum.Phone && phoneActionTypes.Contains(item.ActionTypeId))
                            )
                                && itemStatus.Contains(item.StatusTypeId)
                                && item.ProvisionDate <= DateTime.Now
                                && comp.Id == companyId
                            select item).OrderBy(o => o.Priority)
                .ThenBy(o => o.ProvisionDate)
                .ThenBy(item => item.ItemTypeId)
                .ThenBy(o => o.DateCreated)
                .FirstOrDefault();

            return daoOrder;
        }

        //public Item RetrieveNextItemToProvisionByCompanyIdAndActionTypesAndItemType(int companyId,
        //    ProvisionByMethodTypeEnum provisionByMethodType, List<ActionTypeEnum> provisionableActionTypes, ItemTypeEnum provisionableItemType)
        //{
        //    if (provisionableActionTypes == null || !provisionableActionTypes.Any())
        //        throw new Exception("You must provide at least one action type.");

        //    var actionTypes = provisionableActionTypes.Select(actionType => (int)actionType).ToList();
        //    var itemStatus = new List<int> { (int)StatusTypeEnum.Pending };

        //    //If we are provisionByOrder then we only want pending orders. 
        //    //Otherwise if we are by service we the order might be processing but the services still might need provisioning.
        //    //Rules for getting next order companyid 1, status 2, priority 3, provision date 4, itemtype 5, fifo 6
        //    var daoOrder = (from item in _iandpOrderEntities.Items
        //                join service in _iandpOrderEntities.Services on item.ServiceId equals service.Id
        //                join order in _iandpOrderEntities.Orders on service.OrderId equals order.Id
        //                join comp in _iandpOrderEntities.Companies on order.ExternalCompanyId equals comp.ExternalCompanyId
        //                    where item.ItemTypeId == (int)provisionableItemType
        //                        && actionTypes.Contains(item.ActionTypeId)
        //                        && itemStatus.Contains(item.StatusTypeId)
        //                        && item.ProvisionDate <= DateTime.Now
        //                        && comp.Id == companyId
        //                    select item).OrderBy(o => o.Priority)
        //        .ThenBy(o => o.ProvisionDate)
        //        .ThenBy(item => item.ItemTypeId)
        //        .ThenBy(o => o.DateCreated)
        //        .FirstOrDefault();
            
        //    return daoOrder;
        //}

        public IEnumerable<Order> RetrieveNextNOrdersToProvisionByCompanyId(int numberOfOrders, int companyId)
        {
            //Ruls for getting next order -companyid 1, status 2, priority 3, provision date 4, fifo 5
            var statuses = new List<int> { (int)StatusTypeEnum.Pending };

            var daoOrders = (from o in _iandpOrderEntities.Orders
                            join comp in _iandpOrderEntities.Companies on o.ExternalCompanyId equals comp.ExternalCompanyId
                            where statuses.Contains(o.StatusTypeId)
                                && o.ProvisionDate <= DateTime.Now
                            select o).OrderBy(o => o.Priority).ThenBy(o => o.ProvisionDate).Take(numberOfOrders);

            return daoOrders;
        }
        
        public IEnumerable<Service> AnyServicesLeftOnOrder(int orderId)
        {

            var daoServices =
                _iandpOrderEntities.Services.Where(
                    x =>
                        x.OrderId == orderId &&
                        (x.StatusTypeId == (int) StatusTypeEnum.Pending))
                    .ToList();

            return daoServices;
        }

        public IEnumerable<Service> RetrieveRecentlyProvisionedServices(int numberReturned)
        {
            var statusTypes = new[] { (int)StatusTypeEnum.Error, (int)StatusTypeEnum.Success };
            return _iandpOrderEntities.Services.Where(p => statusTypes.Contains(p.StatusTypeId)).OrderByDescending(p => p.CompletionDate).Take(numberReturned).ToList();
        }

        public int RetrieveServiceIdByOrderIdAndExtServiceIdAndCompanyId(int orderId, string extServiceId, int companyId)
        {
            var daoServiceId = (from o in _iandpOrderEntities.Orders //had to join back to the order in order to get over to the company id since the services don't have a company id
                                join s in _iandpOrderEntities.Services on o.Id equals s.OrderId
                                join c in _iandpOrderEntities.Companies on o.ExternalCompanyId equals c.ExternalCompanyId
                                where o.Id == orderId
                                    && s.ExternalServiceId == extServiceId
                                    && c.Id == companyId
                                select s.Id).FirstOrDefault();

            return daoServiceId;
        }

        public Order UpdateOrderStatus(int orderId, StatusTypeEnum statusType, int priority, string resultMessage, string log, DateTime? startDate, DateTime? compleDateTime, string updatingUserId)
        {
            var daoOrder = _iandpOrderEntities.Orders.FirstOrDefault(x => x.Id == orderId);

            try
            {
                if (daoOrder == null)
                    throw new Exception("Order not found.");

                daoOrder.StatusTypeId = (int) statusType;
                daoOrder.ResultMessage = resultMessage;
                if (priority > 0)
                    daoOrder.Priority = priority;
                daoOrder.Log = log;
                daoOrder.StartDate = startDate;
                daoOrder.CompletionDate = compleDateTime;
                daoOrder.ModifiedByUser = updatingUserId;
                daoOrder.DateModified = DateTime.Now;

                _iandpOrderEntities.AttachEntity(null, daoOrder, new[] {"Id"}, updatingUserId);
                _iandpOrderEntities.SaveChanges();
                _iandpOrderEntities.RefreshEntity(daoOrder);
                
            }
            catch (DbUpdateConcurrencyException)
            {
                //Due to multiply threads running at the same time for each service, and the service is responsible for updating the orders status
                //we must handle the scenarios were a different thread has changed the status in a different entity context 
                //and the current context on a different thread is not aware of the changes.
                //So in layman's terms:
                //We are trying to update a row when the version has changed and we do not have the most update to date information.
                //So we just need to retry updating. But why?
                //Because when we call this again we will get an updated version of the data and then apply our new changes to this.
                //Eventually this will go through.
                UpdateOrderStatus(orderId, statusType, priority, resultMessage, log, startDate, compleDateTime, updatingUserId);
            }

            return daoOrder;
        }

        public Service UpdateServiceStatus(int serviceId, StatusTypeEnum statusType, int priority, string resultMessage, string log, DateTime? startDate, DateTime? compleDateTime, string updatingUserId)
        {
            var daoService = _iandpOrderEntities.Services.FirstOrDefault(x => x.Id == serviceId);

            try
            {
                if (daoService == null)
                    throw new Exception("Service not found.");

                daoService.StatusTypeId = (int)statusType;
                daoService.ResultMessage = resultMessage;
                daoService.Log = log;
                if (priority > 0)
                    daoService.Priority = priority;
                daoService.StartDate = startDate;
                daoService.CompletionDate = compleDateTime;
                daoService.ModifiedByUser = updatingUserId;
                daoService.DateModified = DateTime.Now;

                _iandpOrderEntities.AttachEntity(null, daoService, new[] { "Id" }, updatingUserId);
                _iandpOrderEntities.SaveChanges();
                _iandpOrderEntities.RefreshEntity(daoService);
            }
            catch (DbUpdateConcurrencyException)
            {
                //Due to multiply threads running at the same time for each service, and the service is responsible for updating the orders status
                //we must handle the scenarios were a different thread has changed the status in a different entity context 
                //and the current context on a different thread is not aware of the changes.
                //So in layman's terms:
                //We are trying to update a row when the version has changed and we do not have the most update to date information.
                //So we just need to retry updating. But why?
                //Because when we call this again we will get an updated version of the data and then apply our new changes to this.
                //Eventually this will go through.
                UpdateServiceStatus(serviceId, statusType, priority, resultMessage, log, startDate, compleDateTime, updatingUserId);
            }

            return daoService;
        }

        public Item RetrieveItemById(int itemId)
        {
            var daoItem = _iandpOrderEntities.Items.FirstOrDefault(p => p.Id == itemId);
            return daoItem;
        }

        public Item UpdateItemStatus(int itemId, StatusTypeEnum statusType, int priority, string resultMessage, string log, DateTime? startDate,
            DateTime? compleDateTime, string updatingUserId)
        {
            var daoItem = _iandpOrderEntities.Items.FirstOrDefault(x => x.Id == itemId);
            try
            {
                if (daoItem == null)
                    throw new Exception("Item not found.");

                daoItem.StatusTypeId = (int)statusType;
                daoItem.ResultMessage = resultMessage;
                daoItem.Log = log;
                if (priority > 0)
                    daoItem.Priority = priority;
                daoItem.StartDate = startDate;
                daoItem.CompletionDate = compleDateTime;
                daoItem.ModifiedByUser = updatingUserId;
                daoItem.DateModified = DateTime.Now;

                _iandpOrderEntities.AttachEntity(null, daoItem, new[] { "Id" }, updatingUserId);
                _iandpOrderEntities.SaveChanges();
                _iandpOrderEntities.RefreshEntity(daoItem);
            }
            catch (DbUpdateConcurrencyException)
            {
                //Due to multiply threads running at the same time for each service, and the service is responsible for updating the orders status
                //we must handle the scenarios were a different thread has changed the status in a different entity context 
                //and the current context on a different thread is not aware of the changes.
                //So in layman's terms:
                //We are trying to update a row when the version has changed and we do not have the most update to date information.
                //So we just need to retry updating. But why?
                //Because when we call this again we will get an updated version of the data and then apply our new changes to this.
                //Eventually this will go through.
                UpdateItemStatus(itemId, statusType, priority, resultMessage, log, startDate, compleDateTime, updatingUserId);
            }

            return daoItem;
        }

        public Equipment UpdateEquipmentStatus(int itemId, int equipmentId, int equipmentItemId, StatusTypeEnum statusType, 
            string resultMessage, string log, string name, DateTime? startDate, DateTime? compleDateTime, string updatingUserId)
        {
            var daoEquipment = _iandpOrderEntities.Equipments.FirstOrDefault(x => x.EquipmentSetupId == equipmentId && x.ItemId == itemId && x.EquipmentItemId == equipmentItemId);

            try
            {
                //If we ever do equipment level provisioning this will need to be implemented differently.
                if (daoEquipment != null)
                {
                    daoEquipment.StatusTypeId = (int) statusType;
                    daoEquipment.ResultMessage = resultMessage;
                    daoEquipment.Log = log;
                    daoEquipment.EquipmentItemDescription = name;
                    daoEquipment.StartDate = startDate;
                    daoEquipment.CompletionDate = compleDateTime;
                    daoEquipment.ModifiedByUser = updatingUserId;
                    daoEquipment.DateModified = DateTime.Now;

                    _iandpOrderEntities.AttachEntity(null, daoEquipment, new[] {"Id"}, updatingUserId);
                    _iandpOrderEntities.SaveChanges();
                    _iandpOrderEntities.RefreshEntity(daoEquipment);
                }
                else
                {
                    var daoItem = _iandpOrderEntities.Items.FirstOrDefault(x => x.Id == itemId);
                    if (daoItem != null)
                    {
                        daoEquipment = new Equipment
                        {
                            ExternalEquipmentId = "0",
                            EquipmentSetupId = equipmentId,
                            EquipmentItemId = equipmentItemId,
                            ActionTypeId = daoItem.ActionTypeId,
                            CreatedByUser = updatingUserId,
                            ModifiedByUser = updatingUserId,
                            ItemId = itemId,
                            ProvisionDate = daoItem.ProvisionDate,
                            StatusTypeId = (int) statusType,
                            ResultMessage = resultMessage,
                            EquipmentItemDescription = name,
                            Log = log,
                            Xml = "",
                            StartDate = startDate,
                            CompletionDate = compleDateTime
                        };

                        _iandpOrderEntities.AttachEntity(null, daoEquipment, new[] { "Id" }, updatingUserId);
                        _iandpOrderEntities.SaveChanges();
                        _iandpOrderEntities.RefreshEntity(daoEquipment);
                    }
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                //Due to multiply threads running at the same time for each service, and the service is responsible for updating the orders status
                //we must handle the scenarios were a different thread has changed the status in a different entity context 
                //and the current context on a different thread is not aware of the changes.
                //So in layman's terms:
                //We are trying to update a row when the version has changed and we do not have the most update to date information.
                //So we just need to retry updating. But why?
                //Because when we call this again we will get an updated version of the data and then apply our new changes to this.
                //Eventually this will go through.
                UpdateEquipmentStatus(itemId, equipmentId, equipmentItemId, statusType, resultMessage, log, name, startDate, compleDateTime, updatingUserId);
            }

            return daoEquipment;
        }

        public Service RetrieveServiceByOrderIdAndExtServiceIdAndCompanyId(int orderId, string extServiceId, int companyId)
        {
            var daoService = (from o in _iandpOrderEntities.Orders
                                join s in _iandpOrderEntities.Services on o.Id equals s.OrderId
                                join c in _iandpOrderEntities.Companies on o.ExternalCompanyId equals c.ExternalCompanyId
                                where o.Id == orderId
                                    && s.ExternalServiceId == extServiceId
                                    && c.Id == companyId
                                select s).FirstOrDefault();

            return daoService;
        }

        private void DeleteServiceById(int id)
        {
            var daoService = _iandpOrderEntities.Services.FirstOrDefault(x => x.Id == id);

            if (daoService != null)
            {
                foreach (var item in daoService.Items.ToList())
                {
                    DeleteItemById(item.Id);
                }

                _iandpOrderEntities.Services.Remove(daoService);
                _iandpOrderEntities.SaveChanges();
            }
        }

        private void DeleteItemById(int id)
        {
            var daoItem = _iandpOrderEntities.Items.FirstOrDefault(x => x.Id == id);

            if (daoItem != null)
            {
                _iandpOrderEntities.Items.Remove(daoItem);
                _iandpOrderEntities.SaveChanges();
            }
        }

        public string RetrieveCustomValidation(int companyId, string typeName)
        {

            return "";
        }

        public void Dispose()
        {
            _iandpOrderEntities.Dispose();
        }
    }
}
