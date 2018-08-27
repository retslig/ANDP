
namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ProvisioningSetIptvAccount
    {
        public IptvAccountType IptvAccountType { get; set; }
        public SubscriberType SubscriberType { get; set; }

        public int EquipmentId { get; set; }
    }
}