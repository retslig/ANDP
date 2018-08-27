
using System.Collections.Generic;

namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ProvisioningSetIptvChannelPackageList
    {
        public string ServiceReference { get; set; }
        public List<ChannelPackageType> ChannelPackageTypes { get; set; } 

        public int EquipmentId { get; set; }
    }
}