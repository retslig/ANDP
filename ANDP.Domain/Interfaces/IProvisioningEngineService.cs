
using System.Threading.Tasks;
using ANDP.Lib.Domain.Models;

namespace ANDP.Lib.Domain.Interfaces
{
    public interface IProvisioningEngineService
    {
        void StartProvisioning(int companyId, string serviceName);
        void StopProvisioning(int companyId, string serviceName);
        void PauseProvisioning(int companyId, string updatingUserId);
        void UnPauseProvisioning(int companyId, string updatingUserId);
        EngineSetting RetrieveProvisioningEngineSetting(int companyId);
        EngineStatus RetrieveProvisioningEngineStatus(int companyId, string serviceName);

        Task<OrderStatus> ProvisionOrder(int orderId, int companyId, bool testMode, bool sendResponse, bool forceRun, string updatingUserId);
        OrderStatus UndoOrder(int orderId, int companyId, bool testMode, string updatingUserId);
        Task<ServiceStatus> ProvisionService(int orderId, int serviceId, int companyId, bool testMode, bool sendResponse, bool forceRun, string updatingUserId);
        ServiceStatus UndoService(int orderId, int serviceId, int companyId, bool testMode, string updatingUserId);
        Task<ItemStatus> ProvisionItem(int orderId, int serviceId, int itemId, int companyId, bool testMode, bool sendResponse, bool forceRun, string updatingUserId);
        ItemStatus UndoItem(int orderId, int serviceId, int itemId, int companyId, bool testMode, string updatingUserId);
    }
}
