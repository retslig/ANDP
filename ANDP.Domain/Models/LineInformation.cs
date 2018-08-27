using System.Collections.Generic;

namespace ANDP.Lib.Domain.Models
{
    public class LineInformation
    {
        public ActionType ActionType { get; set; }
        public string LidBBlock { get; set; }
        public string LidBClassCode { get; set; }
        public string DirectoryPublish { get; set; }
        public string CallerName { get; set; }
        public string PrivacyIndicator { get; set; }
        public string PhoneNumber { get; set; }
        public string OldPhoneNumber { get; set; }
        public string Esn { get; set; }
        public string OldEsn { get; set; }
        public string Mdn { get; set; }
        public string OldMdn { get; set; }
        public string Msid { get; set; }
        public string OldMsid { get; set; }

        public bool PortIn { get; set; }
        public bool PortOut { get; set; }
        public string PersistentProfile { get; set; }
        public List<LCC> LCCS { get; set; }

        public InterLataPic InterLataPic { get; set; }
        public IntraLataPic IntraLataPic { get; set; }
        public InterNationalPic InterNationalPic { get; set; }
    }
}
