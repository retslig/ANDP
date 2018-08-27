namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ProvisioningReassignCallerName
    {
        public int EquipmentId { get; set; }

        public string OldPhoneNumber { get; set; }
        public string NewPhoneNumber { get; set; }
    }
}