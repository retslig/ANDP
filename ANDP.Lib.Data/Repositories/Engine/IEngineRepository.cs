using System;
using System.Collections.Generic;

namespace ANDP.Lib.Data.Repositories.Engine
{
    public interface IEngineRepository : IDisposable
    {
        ProvisioningEngineSetting RetrieveProvisioningEngineSetting(int companyId);
        ProvisioningEngineSetting UpdateProvisioningEngineSettings(ProvisioningEngineSetting settings, string updatingUserId);
    }
}
