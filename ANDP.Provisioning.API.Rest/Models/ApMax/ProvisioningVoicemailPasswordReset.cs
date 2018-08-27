namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ProvisioningVoicemailPasswordReset
    {
        public int EquipmentId { get; set; }
        public string PhoneNumber { get; set; }
        public string NewPin { get; set; }
    }
}