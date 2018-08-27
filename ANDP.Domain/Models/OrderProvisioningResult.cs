using System;
using System.Collections.Generic;

namespace ANDP.Lib.Domain.Models
{
    public class OrderProvisioningResult
    {
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public StatusType StatusType { get; set; }
        public string ErrorMessage { get; set; } 
        public string Log { get; set; }
        public List<ServiceProvisioningResult> ServiceResults { get; set; }
    }
}
