namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ProvisioningCreateVoicemail
    {
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public int EquipmentId { get; set; }
        public string VoiceMailPackageName { get; set; }
        public string SubscriberName { get; set; }
        public string SubscriberBillingAccountNumber { get; set; }
        public MailboxType MailBoxType { get; set; }
        public string NotificationCenterDescription { get; set; }
    }
}