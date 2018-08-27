
using System;
using System.Collections.Generic;

namespace ANDP.Lib.Domain.Models
{
    public class EngineSetting
    {
        public int Id { get; set; } // Id (Primary key)
        public int CompanyId { get; set; } // CompanyId
        public string ScriptName { get; set; } // ScriptName
        public bool LoadBalancingActive { get; set; } // LoadBalancingActive
        public bool FailOverActive { get; set; } // FailOverActive
        public int ProvisioningInterval { get; set; } // ProvisioningInterval
        public int MaxThreadsPerDispatcher { get; set; } // MaxThreadsPerDispatcher
        public ProvisionByMethodType ProvisionByMethod { get; set; }
        public List<ActionType> ProvisionableOrderOrServiceActionTypes { get; set; }
        public List<ItemActionType> ProvisionableItemActionTypes { get; set; }
        public bool ProvisioningPaused { get; set; } // ProvisioningPaused
        public List<EngineSchedule> Schedules { get; set; } 
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
    }
}
