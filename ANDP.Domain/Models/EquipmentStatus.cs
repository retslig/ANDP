using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANDP.Lib.Domain.Models
{
    public class EquipmentStatus
    {
        public int Id { get; set; } // Id (Primary key)
        public string ExternalOrderId { get; set; } // ExternalOrderId
        public DateTime ProvisionDate { get; set; } // ProvisionDate
        public string ResultMessage { get; set; } // ResultMessage
        public DateTime? CompletionDate { get; set; } // CompletionDate
        public DateTime? StartDate { get; set; } // StartDate
        public StatusType StatusType { get; set; } // FK_Order_StatusType
    }
}
