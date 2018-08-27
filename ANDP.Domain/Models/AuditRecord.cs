
using System;

namespace ANDP.Lib.Domain.Models
{
    public class AuditRecord
    {
        public Guid RunNumber { get; set; } // RunNumber
        public DateTime RunDate { get; set; } // RunDate
        public int CompanyId { get; set; } // CompanyId
        public int EquipmentId { get; set; } // EquipmentSetupId
        public bool BillingOrEquipmentIndicator { get; set; } // BillingOrEquipmentIndicator
        public string ExternalAccountId { get; set; } // ExternalAccountId
        public string ExternalServiceId { get; set; } // ExternalServiceId
        public string ExternalItemId { get; set; }
        public ItemType ItemType { get; set; }
        public string RecordKey { get; set; } // RecordKey
        public string RecordType { get; set; } // RecordType
        public string RecordValue { get; set; } // RecordValue
        public bool? Ignore { get; set; } // Ignore
        public Guid? MatchId { get; set; } // MatchId
        public bool? AddToEquiment { get; set; } // AddToEquiment
        public bool? AddToBilling { get; set; } // AddToBilling
    }
}
