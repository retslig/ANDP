using System;
using System.Collections.Generic;
using ANDP.Lib.Domain.Models;

namespace ANDP.Lib.Domain.Interfaces
{
    public interface IOrderService
    {
        OrderStatus CreateOrUpdateOrder(Order order, string updatingUserId);
        OrderStatus UpdateOrder(Order order, string updatingUserId);
        void UpdateOrderSentResponseFlag(int orderId, bool sent, string updatingUserId);
        OrderStatus DeleteOrder(Order order, string updatingUserId);
        OrderStatus DeleteOrderById(int id, string updatingUserId);
        OrderStatus DeleteOrderByExtIdAndExtCompanyId(string externalOrderId, string externalCompanyId, string updatingUserId);
        OrderStatus RetrieveOrderStatusById(int orderId);
        IEnumerable<Order> RetrieveCompletedOrders(int companyId);
        OrderStatus RetrieveOrderStatusByExtIdAndExtCompanyId(string extOrderId, string externalCompanyId);
        Order RetrieveOrderByExtIdAndExtCompanyId(string extOrderId, string externalCompanyId);
        IEnumerable<Order> RetrieveProcessingOrders(string externalCompanyId);
        Order RetrieveOrderById(int orderId);
        int RetrieveOrderIdByExtIdAndExtCompanyId(string extOrderId, string externalCompanyId);
        IEnumerable<Order> RetrieveOrdersByProvisionDateAndExtCompanyId(DateTime date, string externalCompanyId);
        IEnumerable<Order> RetrieveOrdersByStatusTypeAndExtCompanyId(StatusType statusType, string externalCompanyId);
        IEnumerable<Order> RetrieveOrdersByCreateByUserAndExtCompanyId(string userName, string externalCompanyId);
        IEnumerable<Order> RetrieveOrdersByActionTypeAndExtCompanyId(ActionType actionType, string externalCompanyId);
        IEnumerable<Order> RetrieveOrdersByPriorityAndExtCompanyId(int priority, string externalCompanyId);
        OrderStatus UndoOrderByOrderId(int orderId, string updatingUserId);
        OrderStatus UndoOrderByExtIdAndExtCompanyId(string externalOrderId, string externalCompanyId, string updatingUserId);

        int RetrieveServiceIdByExtOrderIdAndExtIdAndExtCompanyId(string externalOrderId,string extServiceId, string externalCompanyId);
        Service RetrieveServiceByExtOrderIdAndExtIdAndExtCompanyId(string externalOrderId, string externalServiceId, string externalCompanyId);
        Service RetrieveServiceById(int serviceId);
        ServiceStatus RetrieveServiceStatusById(int serviceId);

        object RetrieveItemById(int itemId);
        ItemStatus RetrieveItemStatusById(int itemId);

        int RetrieveCompanyIdByExtCompanyId(string externalCompanyId);
        IEnumerable<Company> RetrieveAllCompanies();

        Order RetrieveNextOrderToProvisionByCompanyIdAndActionType(int companyId, List<ActionType> provisionableOrderTypes);
        Service RetrieveNextServiceToProvisionByCompanyIdAndActionType(int companyId, List<ActionType> provisionableOrderTypes);
        object RetrieveNextItemToProvisionByCompanyIdAndActionTypeAndItemType(int companyId, List<ItemActionType> provisionableItemTypes);
        Service AnyServicesLeftOnOrder(int serviceId);
        IEnumerable<Service> RetrieveRecentlyProvisionedServices(int numberReturned);

        OrderStatus UpdateOrderStatus(int orderId, StatusType statusType, int priority, string resultMessage, string log, DateTime? startDate, DateTime? compleDateTime, string updatingUserId);
        ServiceStatus UpdateServiceStatus(int serviceId, StatusType statusType, int priority, string resultMessage, string log, DateTime? startDate, DateTime? compleDateTime, string updatingUserId);
        ItemStatus UpdateItemStatus(int itemId, StatusType statusType, int priority, string resultMessage, string log, DateTime? startDate, DateTime? compleDateTime, string updatingUserId);
        EquipmentStatus UpdateEquipmentStatus(int itemId, int equipmentId, int equipmentItemId, StatusType statusType, string resultMessage, string log, string name, DateTime? startDate, DateTime? compleDateTime, string updatingUserId);

        ServiceStatus UpdateServiceResult(ServiceProvisioningResult serviceProvisioningResult, string updatingUserId);
        ItemStatus UpdateItemResult(ItemProvisioningResult itemProvisioningResult, string updatingUserId);
        OrderStatus UpdateEquipmentResult(EquipmentProvisioningResult equipmentProvisioningResult, string updatingUserId);
    }
}
