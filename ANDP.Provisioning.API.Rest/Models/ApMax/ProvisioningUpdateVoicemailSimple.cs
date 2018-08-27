namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ProvisioningUpdateVoicemailSimple
    {
        public string PhoneNumber { get; set; }
        public int EquipmentId { get; set; }
        public int MaxMailBoxtime { get; set; }
        public int MaxMessages { get; set; }
    }
}