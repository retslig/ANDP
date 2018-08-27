
namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ProvisioningUpdateVoicemail
    {

        public VoiceMailBoxType VoiceMailBoxType { get; set; }
        public InternetAccessType InternetAccessType { get; set; }
        public string PhoneNumber { get; set; }
        public int EquipmentId { get; set; }
    }
}