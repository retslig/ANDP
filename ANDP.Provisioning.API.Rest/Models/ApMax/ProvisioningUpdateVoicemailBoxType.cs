namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ProvisioningUpdateVoicemailBoxType
    {
        public int EquipmentId { get; set; }
        public string PhoneNumber { get; set; }
        public MailboxType MailboxType { get; set; }
        public string InternetUserName { get; set; }
        public string InternetPassword { get; set; }
        public bool InternetAccess { get; set; }

    }
}