using System;

namespace ANDP.Lib.Domain.Models
{
    public class UsocToCommandTranslation
    {
        public int Id { get; set; } // Id (Primary key)
        public int CompanyId { get; set; } // CompanyId
        public int EquipmentId { get; set; } // EquipmentId
        public string UsocName { get; set; } // UsocName
        public string AddCommand { get; set; } // AddCommand
        public string DeleteCommand { get; set; } // DeleteCommand
        public bool Active { get; set; } // Active
        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
    }
}
