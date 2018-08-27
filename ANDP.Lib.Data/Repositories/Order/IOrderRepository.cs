using System;
using System.Collections.Generic;

namespace ANDP.Lib.Data.Repositories.Order
{

    public interface IOrderRepository : IDisposable
    {
        Order CreateOrUpdateOrder(Order order, string updatingUserId);
        Order UpdateOrder(Order order, string updatingUserId);
        void UpdateOrderSentResponseFlag(int orderId,  bool sent, string updatingUserId);
        Order UpdateOrderStatus(int orderId, StatusTypeEnum statusType, int priority, string resultMessage, string log, DateTime? startDate, DateTime? compleDateTime, string updatingUserId);
        Order DeleteOrder(Order order, string updatingUserId);
        Order DeleteOrderById(int id, string updatingUserId);
        Order DeleteOrderByExtId(string extId, string updatingUserId);
        Order UndoOrder(int orderId, string updatingUserId);

        IEnumerable<Order> RetrieveCompletedOrders(int companyId);
        Order RetrieveOrderById(int orderId);
        int RetrieveOrderIdByExtIdAndCompanyId(string extOrderId, int companyId);
        IEnumerable<Order> RetrieveOrdersByProvisionDateAndCompanyId(DateTime date, int companyId);
        IEnumerable<Order> RetrieveOrdersByStatusTypeAndCompanyId(StatusTypeEnum statusType, int companyId);
        IEnumerable<Order> RetrieveOrdersByCreateByUserAndCompanyId(string userName, int companyId);
        IEnumerable<Order> RetrieveOrdersByActionTypeAndCompanyId(ActionTypeEnum actionType, int companyId);
        IEnumerable<Order> RetrieveOrdersByPriorityAndCompanyId(int priority, int companyId);
        IEnumerable<Order> RetrieveProcessingOrders(int companyId);
        Service RetrieveServiceByOrderIdAndExtServiceIdAndCompanyId(int orderId, string extServiceId, int companyId);
        Service RetrieveServiceById(int serviceId);

        Order RetrieveNextOrderToProvisionByCompanyIdAndActionType(int companyId, List<ActionTypeEnum> provisionableActionTypes);
        Service RetrieveNextServiceToProvisionByCompanyIdAndActionType(int companyId, List<ActionTypeEnum> provisionableActionTypes);
        Item RetrieveNextItemToProvisionByCompanyIdAndActionTypesAndItemType(int companyId, Dictionary<ItemTypeEnum, List<ActionTypeEnum>> provisionableItemActionTypes);
        IEnumerable<Order> RetrieveNextNOrdersToProvisionByCompanyId(int numberOfOrders, int companyId);
        IEnumerable<Service> AnyServicesLeftOnOrder(int orderId);

        IEnumerable<Service> RetrieveRecentlyProvisionedServices(int numberReturned);

        Service UpdateServiceStatus(int serviceId, StatusTypeEnum statusType, int priority, string resultMessage, string log, DateTime? startDate, DateTime? compleDateTime, string updatingUserId);
        int RetrieveServiceIdByOrderIdAndExtServiceIdAndCompanyId(int orderId, string extServiceId, int companyId);

        Item RetrieveItemById(int itemId);
        Item UpdateItemStatus(int itemId, StatusTypeEnum statusType, int priority, string resultMessage, string log, DateTime? startDate, DateTime? compleDateTime, string updatingUserId);

        Equipment UpdateEquipmentStatus(int itemId, int equipmentId, int equipmentItemId, StatusTypeEnum statusType, string resultMessage, string log, string name, DateTime? startDate, DateTime? compleDateTime, string updatingUserId);

        string RetrieveCustomValidation(int companyId, string typeName);
    }
}
