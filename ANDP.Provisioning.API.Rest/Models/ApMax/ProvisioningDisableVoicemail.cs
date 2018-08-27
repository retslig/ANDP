
namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ProvisioningDisableVoicemail
    {
        public string PhoneNumber { get; set; }
        public bool Disable { get; set; }

        public int EquipmentId { get; set; }
    }
}