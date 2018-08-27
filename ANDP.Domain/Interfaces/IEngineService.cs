
using System.Collections.Generic;
using ANDP.Lib.Domain.Models;

namespace ANDP.Lib.Domain.Interfaces
{
    public interface IEngineService
    {
        EngineSetting RetrieveProvisioningEngineSetting(int companyId);
        void UnPauseProvisioning(int companyId, string updatingUserId);
        void PauseProvisioning(int companyId, string updatingUserId);
    }
}
