namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ProvisioningReassignVoicemail
    {
        public string OldPhoneNumber { get; set; }
        public string NewPhoneNumber { get; set; }
        public string MailBoxDescription { get; set; }
        public string SubscriberName { get; set; }
        public string InternetUserName { get; set; }
        public string InternetPassword { get; set; }
        public bool InternetAccess { get; set; }
        public bool DeleteOldSubscriber { get; set; }
        public int EquipmentId { get; set; }




    }
}