using System;
using System.Collections.Generic;
using Common.Lib.Utility;
using CustomValidationService = ANDP.Lib.Domain.Services.CustomValidationService;

namespace ANDP.Lib.Domain.Models
{
    public class VideoItem
    {
        public int Id { get; set; } // Id (Primary key)
        public string ExternalItemId { get; set; } 
        public int ServiceId { get; set; } // ServiceId
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
        public int MaxPurchaseLimit { get; set; }
        public string PurchasePin { get; set; }
        public string RatingPin { get; set; }
        public string ServiceArea { get; set; }
        public string OldServiceArea { get; set; }
        public string FipsCountyCode { get; set; }
        public string OldFipsCountyCode { get; set; }
        public string FipsStateCode { get; set; }
        public string OldFipsStateCode { get; set; }
        public string ScreenPopPhoneNumber { get; set; }
        public string OldScreenPopPhoneNumber { get; set; }
        public string Notes { get; set; }
        public string CustomId { get; set; }

        public string CreatedByUser { get; set; } // CreatedByUser
        public string ModifiedByUser { get; set; } // ModifiedByUser
        public DateTime DateCreated { get; set; } // DateCreated
        public DateTime DateModified { get; set; } // DateModified
        public int Version { get; set; } // Version

        public SerializableDictionary<string, object> Plant { get; set; }
        public SerializableDictionary<string, object> OldPlant { get; set; }
        public List<Feature> Features { get; set; }
        public List<Equipment> Equipments { get; set; }

        //The setter has to be public for nlog to be able to serialize this.
        //Dictionary isn't serializable and I need it to be for nlog usage. That's why I'm using the SerializableDictionary I built.
        public SerializableDictionary<string, string> ValidationErrors { get; set; }

        public bool Validate(CustomValidationService service)
        {
            ValidationErrors = new SerializableDictionary<string, string>();

            //if (service != null)
            //{
            //    bool valid = service.Validate(this, this.GetType());
            //    if (!valid)
            //        ValidationErrors = service.ValidationErrors;
            //}

            if (string.IsNullOrWhiteSpace(ExternalItemId))
            {
                ValidationErrors.Add(LambdaHelper<VideoItem>.GetPropertyName(x => x.ExternalItemId), "VideoItem.ExternalOrderId is a mandatory field.");
            }

            if (Priority == null || Priority < 1)
            {
                ValidationErrors.Add(LambdaHelper<VideoItem>.GetPropertyName(x => x.Priority), "VideoItem.Priority is a mandatory field.");
            }

            if (ProvisionSequence == null || ProvisionSequence < 1)
            {
                ValidationErrors.Add(LambdaHelper<VideoItem>.GetPropertyName(x => x.ProvisionSequence), "VideoItem.ProvisionSequence is a mandatory field and must be greater then 0.");
            }

            if (ProvisionDate == null || ProvisionDate == DateTime.MinValue)
            {
                ValidationErrors.Add(LambdaHelper<VideoItem>.GetPropertyName(x => x.ProvisionDate), "VideoItem.ProvisionDate is a mandatory field.");
            }

            return ValidationErrors.Count > 0;
        }
    }

    public class ValidationHalper
    {
        public SerializableDictionary<string, string> ValidationErrors { get; set; }

        public bool Validate(object model)
        {
            var videoItem = model as VideoItem;
            if (videoItem == null)
            {
                ValidationErrors.Add(LambdaHelper<VideoItem>.GetPropertyName(x => x.ProvisionDate), "VideoItem cannot be null");
                return false;
            }

            if ((videoItem.ActionType == ActionType.Add || videoItem.ActionType == ActionType.Change) && string.IsNullOrEmpty(videoItem.ServiceArea))
            {
                ValidationErrors.Add(LambdaHelper<VideoItem>.GetPropertyName(x => x.ServiceArea), "VideoItem.ServiceArea cannot be blank on adds or changes.");
            }

            if ((videoItem.ActionType == ActionType.Add || videoItem.ActionType == ActionType.Change) && string.IsNullOrEmpty(videoItem.FipsCountyCode))
            {
                ValidationErrors.Add(LambdaHelper<VideoItem>.GetPropertyName(x => x.FipsCountyCode), "VideoItem.FipsCountyCode cannot be blank on adds or changes.");
            }

            if ((videoItem.ActionType == ActionType.Add || videoItem.ActionType == ActionType.Change) && string.IsNullOrEmpty(videoItem.FipsStateCode))
            {
                ValidationErrors.Add(LambdaHelper<VideoItem>.GetPropertyName(x => x.FipsStateCode), "VideoItem.FipsStateCode cannot be blank on adds or changes.");
            }


            return ValidationErrors.Count > 0;
        }
    }
}
