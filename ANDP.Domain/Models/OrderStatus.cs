using System;

namespace ANDP.Lib.Domain.Models
{
    public class OrderStatus
    {
        public int Id { get; set; } // Id (Primary key)
        public string ExternalOrderId { get; set; } // ExternalOrderId
        public DateTime ProvisionDate { get; set; } // ProvisionDate
        public string ResultMessage { get; set; } // ResultMessage
        public bool ResponseSent { get; set; } // ResponseSent
        public DateTime? CompletionDate { get; set; } // CompletionDate
        public DateTime? StartDate { get; set; } // StartDate
        public StatusType StatusType { get; set; } // FK_Order_StatusType
    }
}
