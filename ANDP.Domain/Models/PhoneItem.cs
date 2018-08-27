using System;
using System.Collections.Generic;
using ANDP.Lib.Domain.Services;
using Common.Lib.Utility;

namespace ANDP.Lib.Domain.Models
{
    public class PhoneItem
    {
        public int Id { get; set; } // Id (Primary key)
        public string ExternalItemId { get; set; } // ExternalServiceId
        public int ServiceId { get; set; } // OrderId
        public int Priority { get; set; } // Priority
        public int ProvisionSequence { get; set; } // ProvisionSequence
        public string Xml { get; set; } // Xml
        public DateTime ProvisionDate { get; set; } // ProvisionDate
        public StatusType StatusType { get; set; }
        public ActionType ActionType { get; set; }
        public string ResultMessage { get; set; } // ResultMessage
        public string Log { get; set; }
        public DateTime? CompletionDate { get; set; } // CompletionDate
        public DateTime? StartDate { get; set; } // StartDate
        public string Notes { get; set; }
        public string CustomId { get; set; }

        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        public LineInformation LineInformation { get; set; }
        public List<Feature> Features { get; set; }
        public Directory Directory { get; set; }
        public SerializableDictionary<string, object> Plant { get; set; }
        public SerializableDictionary<string, object> OldPlant { get; set; }
        public List<Equipment> Equipments { get; set; }

        //The setter has to be public for nlog to be able to serialize this.
        //Dictionary isn't serializable and I need it to be for nlog usage. That's why I'm using the SerializableDictionary I built.
        public SerializableDictionary<string, string> ValidationErrors { get; set; }

        public bool Validate(CustomValidationService service)
        {
            ValidationErrors = new SerializableDictionary<string, string>();

            if (string.IsNullOrWhiteSpace(ExternalItemId))
            {
                ValidationErrors.Add(LambdaHelper<PhoneItem>.GetPropertyName(x => x.ExternalItemId), "PhoneItem.ExternalItemId is a mandatory field.");
            }

            if (Priority == null || Priority < 1)
            {
                ValidationErrors.Add(LambdaHelper<PhoneItem>.GetPropertyName(x => x.Priority), "PhoneItem.Priority is a mandatory field.");
            }

            if (ProvisionSequence == null || ProvisionSequence < 1)
            {
                ValidationErrors.Add(LambdaHelper<PhoneItem>.GetPropertyName(x => x.ProvisionSequence), "PhoneItem.ProvisionSequence is a mandatory field and must be greater then 0.");
            }

            if (ProvisionDate == null || ProvisionDate == DateTime.MinValue)
            {
                ValidationErrors.Add(LambdaHelper<PhoneItem>.GetPropertyName(x => x.ProvisionDate), "PhoneItem.ProvisionDate is a mandatory field.");
            }

            return ValidationErrors.Count > 0;
        }
    }
}
