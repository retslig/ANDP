using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANDP.Lib.Domain.Models
{
    public class Equipment
    {
        public int Id { get; set; } // Id (Primary key)
        public string ExternalEquipmentId { get; set; } // ExternalEquipmentId
        public int EquipmentSetupId { get; set; } 
        public int ItemId { get; set; } // ItemId
        public string EquipmentItemDescription { get; set; } // EquipmentItemDescription
        public int Priority { get; set; } // Priority
        public int ProvisionSequence { get; set; } // ProvisionSequence
        public string Xml { get; set; } // Xml
        public DateTime ProvisionDate { get; set; } // ProvisionDate
        public StatusType StatusType { get; set; } // StatusTypeId
        public ActionType ActionType { get; set; } // ActionTypeId
        public DateTime? StartDate { get; set; } // StartDate
        public DateTime? CompletionDate { get; set; } // CompletionDate
        public string ResultMessage { get; set; } // ResultMessage
        public string Log { get; set; } // Log
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
    }
}
