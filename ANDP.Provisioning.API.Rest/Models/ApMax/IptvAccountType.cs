using System;
using System.Collections.Generic;

namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class IptvAccountType
    {
        public string AccountDescription { get; set; }
        public bool Active { get; set; }
        public List<ChannelPackageType> ChannelPackageTypes { get; set; }
        public AdultChannelState AdultChannelsState { get; set; }
        public decimal CurrentAmountCharged { get; set; }
        public string DeactivateReason { get; set; }
        public int FipsCountyCode { get; set; }
        public int FipsStateCode { get; set; }
        public int MaxBandwidthKbs { get; set; }
        public int MaxChargingLimit { get; set; }
        public string PurchasePin { get; set; }
        public string RatingPin { get; set; }
        public string ServiceAreaId { get; set; }
        public string ServiceReference { get; set; }
        public string SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public string Comment { get; set; }
        public int? MaxAllowedStbs { get; set; }
        public string RoutePort { get; set; }
        public List<SetTopBoxType> SetTopBoxTypes { get; set; } 
    }
}