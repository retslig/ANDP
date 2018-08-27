using System;

namespace Common.Lib.Domain.Common.Models
{
    public class Tenant
    {
        public Guid Guid { get; set; } // Guid (Primary key)
        public string Name { get; set; } // Name
        public string Schema { get; set; } // Schema
    }
}
