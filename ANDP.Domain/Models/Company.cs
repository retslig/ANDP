using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANDP.Lib.Domain.Models
{
    public class Company
    {
        public int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public string ExternalCompanyId { get; set; } // ExternalCompanyId
        public string CreatedByUser { get; set; } // CreatedById
        public string ModifiedByUser { get; set; } // ModifiedById
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
    }
}
