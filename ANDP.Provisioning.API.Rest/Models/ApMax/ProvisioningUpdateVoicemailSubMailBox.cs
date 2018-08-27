namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ProvisioningUpdateVoicemailSubMailBox
    {
        public int EquipmentId { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfSubMailBoxes { get; set; }
        public int MaxNumberOfSubMailBoxesAllowed { get; set; }


    }
}