using System;
using System.Collections.Generic;
using Common.Lib.Infastructure;
using Common.Lib.Utility;
using CustomValidationService = ANDP.Lib.Domain.Services.CustomValidationService;

namespace ANDP.Lib.Domain.Models
{
    public class Service
    {
        public List<Location> Locations { get; set; }
        public int Id { get; set; } // Id (Primary key)
        public string ExternalServiceId { get; set; } // ExternalServiceId
        public int OrderId { get; set; } // OrderId
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

        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version


        //The setter has to be public for nlog to be able to serialize this.
        //Dictionary isn't serializable and I need it to be for nlog usage. That's why I'm using the SerializableDictionary I built.
        public SerializableDictionary<string, string> ValidationErrors { get; set; }

        public bool Validate(CustomValidationService service)
        {
            ValidationErrors = new SerializableDictionary<string, string>();

            if (string.IsNullOrWhiteSpace(ExternalServiceId))
            {
                ValidationErrors.Add(LambdaHelper<Service>.GetPropertyName(x => x.ExternalServiceId), "Service.ExternalServiceId is a mandatory field.");
            }

            if (Priority == null || Priority < 1)
            {
                ValidationErrors.Add(LambdaHelper<Service>.GetPropertyName(x => x.Priority), "Service.Priority is a mandatory field.");
            }

            if (ProvisionSequence == null || ProvisionSequence < 1)
            {
                ValidationErrors.Add(LambdaHelper<Service>.GetPropertyName(x => x.ProvisionSequence), "Service.ProvisionSequence is a mandatory field and must be greater then 0.");
            }

            if (ProvisionDate == null || ProvisionDate == DateTime.MinValue)
            {
                ValidationErrors.Add(LambdaHelper<Service>.GetPropertyName(x => x.ProvisionDate), "Service.ProvisionDate is a mandatory field.");
            }

            //Note: Validation the child class as well.

            if (Locations == null)
                ValidationErrors.Add(LambdaHelper<Service>.GetPropertyName(x => x.Locations), "Service.Locations is a mandatory field.");
            else
            {
                foreach (var location in Locations)
                {
                    if (location.Validate(service))
                    {
                        foreach (var validationError in location.ValidationErrors)
                        {
                            if (!ValidationErrors.ContainsKey(validationError.Key))
                            {
                                ValidationErrors.Add(validationError.Key, validationError.Value);
                            }
                        }
                    }
                }
            }

            return ValidationErrors.Count > 0;
        }
    }
}
