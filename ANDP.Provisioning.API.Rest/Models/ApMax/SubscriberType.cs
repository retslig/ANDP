using System;
using System.Collections.Generic;

namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class SubscriberType
    {
        public List<AddressInfoType> Addresses { get; set; }
        public InternetAccessType InternetAccess { get; set; }
        public PlacementType PlacementType { get; set; }
        public ServiceInfoType ServiceInformation { get; set; }
        public Timezone SubscriberTimezone { get; set; }
        public string BillingAccountNumber { get; set; }
        public string BillingEnvironmentCode { get; set; }
        public string BillingServiceAddress { get; set; }
        public string DialByNameDigits { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUpdatedBy { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ParentSubscriberId { get; set; }
        public string SubscriberDefaultPhoneNumber { get; set; }
        public string SubscriberEmail { get; set; }
        public string SubscriberGuid { get; set; }
        public string SubscriberName { get; set; }
    }
}