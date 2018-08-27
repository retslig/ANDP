namespace Common.MetaSwitch
{
    public class Subscriber
    {
        public string Dn { get; set; }
        public string MetaSwitchName { get; set; }
        public string BusinessGroupName { get; set; }
        public string SubscriberGroup { get; set; }
        public string PersistentProfile { get; set; }
        public string SipUserName { get; set; }
        public string SipDomainName { get; set; }
        public string SipPassword { get; set; }
        public string AccessDevice { get; set; }
        public string Gateway { get; set; }
        public int AccessLineNumber { get; set; }
        public tMeta_Subscriber_BaseInformation_NumberStatus NumberStatus { get; set; }
        public tMeta_Subscriber_BaseInformation_SignalingType SignalingType { get; set; }
    }
}