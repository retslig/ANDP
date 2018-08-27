namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ProvisioningAddCallerName
    {
        public int EquipmentId { get; set; }

        public string PhoneNumber { get; set; }
        public string CallerName { get; set; }
        public string Presentation { get; set; }
    }
}