using System;

namespace ANDP.Lib.Domain.Models
{
    public class Directory
    {
        public int DirectoryPhoneNumber { get; set; } //not sure if we need this. Would this always be the same as the phone number on the Phone Service?
        public ActionType ActionType { get; set; }
        public DateTime EffectiveDate { get; set; }
        public bool LocalWhitePageList { get; set; }
        public bool DirectoryAssistanceListing { get; set; }
        public bool PrintPhoneNumber { get; set; }
        public bool ListingBold { get; set; }
        public bool ListingItalic { get; set; }
        public string ListingUnderline { get; set; }
        public string Listing { get; set; }
        public string ListingFirstName { get; set; }
        public string ListingSuffix { get; set; }
        public string ListingTitle { get; set; }
        public Address ListingAddress { get; set; } //Not sure we need this. Would this always be the same as the address on the account level?
        public int DefaultIndentation { get; set; }
        public string DirectoryClassId { get; set; }
        public int DirectoryListingPhoneId { get; set; }
        public int NumberOfDirectories { get; set; }
        public int DirectoryDeliveryContactId { get; set; }

    }
}
