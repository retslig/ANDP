using System;

namespace Common.Lib.Domain.Common.Models
{
    public class User
    {
        public Guid TenantId { get; set; } // Guid (Primary key)
        public string UserName { get; set; } // Name
    }
}
