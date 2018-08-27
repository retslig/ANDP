using System.Collections.Generic;

namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ChannelLineupType
    {
        public List<ChannelPackageType> ChannelPackageTypes { get; set; }
        public List<CountyType> CountyTypes { get; set; }
        public string ServiceAreaId { get; set; }
        public string ServiceAreaIndex { get; set; }
        public string ServiceAreaName { get; set; }
        public string ServiceAreaTimeZone { get; set; }
    }
}