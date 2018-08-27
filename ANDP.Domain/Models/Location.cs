using System;
using System.Collections.Generic;
using System.Linq;
using Common.Lib.Utility;
using CustomValidationService = ANDP.Lib.Domain.Services.CustomValidationService;

namespace ANDP.Lib.Domain.Models
{
    public class Location
    {
        public string ExternalLocationId { get; set; }

        public Address Address { get; set; }
        public Address OldAddress { get; set; }
        public LocationInfo LocationInfo { get; set; }
        public List<PhoneItem> PhoneItems { get; set; }
        public List<InternetItem> InternetItems { get; set; }
        public List<VideoItem> VideoItems { get; set; }

        public string TaxArea { get; set; }
        public string CountyJurisdiction { get; set; }
        public string DistrictJurisdiction { get; set; }
        public string MsagExchange { get; set; }
        public int MsaGesn { get; set; }

        //The setter has to be public for nlog to be able to serialize this.
        //Dictionary isn't serializable and I need it to be for nlog usage. That's why I'm using the SerializableDictionary I built.
        public SerializableDictionary<string, string> ValidationErrors { get; set; }

        public bool Validate(CustomValidationService service)
        {
            ValidationErrors = new SerializableDictionary<string, string>();

            if ((PhoneItems == null || PhoneItems.Count < 1)
                 && (InternetItems == null || InternetItems.Count < 1)
                 && (VideoItems == null || VideoItems.Count < 1))
            {
                ValidationErrors.Add(LambdaHelper<Location>.GetPropertyName(x => x.PhoneItems), "Location.PhoneItems is a mandatory field.");
                ValidationErrors.Add(LambdaHelper<Location>.GetPropertyName(x => x.InternetItems), "Location.InternetItems is a mandatory field.");
                ValidationErrors.Add(LambdaHelper<Location>.GetPropertyName(x => x.VideoItems), "Location.VideoItems is a mandatory field.");
            }

            if (PhoneItems != null && PhoneItems.Any())
            {
                foreach (var item in PhoneItems)
                {
                    if (item.Validate(service))
                    {
                        foreach (var validationError in item.ValidationErrors)
                        {
                            if (!ValidationErrors.ContainsKey(validationError.Key))
                            {
                                ValidationErrors.Add(validationError.Key, validationError.Value);
                            }
                        }
                    }
                }
            }

            if (InternetItems != null)
                if (InternetItems != null && InternetItems.Any())
                {
                    foreach (var item in InternetItems)
                    {
                        if (item.Validate(service))
                        {
                            foreach (var validationError in item.ValidationErrors)
                            {
                                if (!ValidationErrors.ContainsKey(validationError.Key))
                                {
                                    ValidationErrors.Add(validationError.Key, validationError.Value);
                                }
                            }
                        }
                    }
                }

            if (VideoItems != null)
                if (VideoItems != null && VideoItems.Any())
                {
                    foreach (var item in VideoItems)
                    {
                        if (item.Validate(service))
                        {
                            foreach (var validationError in item.ValidationErrors)
                            {
                                if (!ValidationErrors.ContainsKey(validationError.Key))
                                {
                                    ValidationErrors.Add(validationError.Key, validationError.Value);
                                }
                            }
                        }
                    }
                }

            if (Address == null)
                ValidationErrors.Add(LambdaHelper<Location>.GetPropertyName(x => x.Address), "Location.Address is a mandatory field.");
            else
            {
                if (Address.Validate())
                {
                    foreach (var validationError in Address.ValidationErrors)
                    {
                        if (!ValidationErrors.ContainsKey(validationError.Key))
                        {
                            ValidationErrors.Add(validationError.Key, validationError.Value);
                        }
                    }
                }
            }

            return ValidationErrors.Count > 0;
        }
    }
}
