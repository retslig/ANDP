using System;

namespace Common.Lib.Common.Email
{
    public class EmailDistributionList
    {
        public int Id { get; set; }
        public string ApplicationName { get; set; }
        public string ServerName { get; set; }
        public string EmailServerName { get; set; }
        public string Type { get; set; }
        public string Key { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Attachments { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public Nullable<int> Version { get; set; }
    }
}
