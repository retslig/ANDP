

using System;

namespace ANDP.Lib.Domain.Models
{
    public class DataDictionary
    {
        public int Id { get; set; } // Id (Primary key)
        public int? CompanyId { get; set; } // CompanyId
        public int? EquipmentId { get; set; } // EquipmentId
        public string Key1 { get; set; } // Key1
        public string Key2 { get; set; } // Key2
        public string Key3 { get; set; } // Key3
        public string Key4 { get; set; } // Key4
        public string Key5 { get; set; } // Key5
        public string Key6 { get; set; } // Key6
        public string Key7 { get; set; } // Key7
        public string Key8 { get; set; } // Key8
        public string Key9 { get; set; } // Key9
        public string Value1 { get; set; } // Value1
        public string Value2 { get; set; } // Value2
        public string Value3 { get; set; } // Value3
        public string Value4 { get; set; } // Value4
        public string Value5 { get; set; } // Value5
        public string Value6 { get; set; } // Value6
        public string Value7 { get; set; } // Value7
        public string Value8 { get; set; } // Value8
        public string Value9 { get; set; } // Value9
        public bool? Active { get; set; } // Active
        public string CreatedByUser { get; set; } // CreatedById
        public string ModifiedByUser { get; set; } // ModifiedById
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version
    }
}
