using System.Collections.Generic;

namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class VoiceMailBoxType
    {
        //public string Id { get; set; }
        public string Description { get; set; }
        public MailboxType MailBoxType { get; set; }
        public List<NotificationInfoType> Notifications { get; set; }
        public int? MaxMailBoxTime { get; set; }
        public int? MaxMessageLength { get; set; }
        public int? MessageCount { get; set; }
        //public string OptionsPackage { get; set; }
    }
}