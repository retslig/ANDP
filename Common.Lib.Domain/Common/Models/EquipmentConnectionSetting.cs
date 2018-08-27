using System.Collections.Generic;
using Common.Lib.Domain.Common.Services.ConnectionManager;
using Common.Lib.Domain.Common.Services.ConnectionManager.Socket;

namespace Common.Lib.Domain.Common.Models
{
    public class EquipmentConnectionSetting
    {
        public int EquipmentId { get; set; }
        public string Url { get; set; }
        public string Ip { get; set; }
        public int? Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ConnectionType ConnectionType { get; set; }
        public AuthenticationType AuthenticationType { get; set; } // Not used for Telnet or Tcp
        public EncodingType Encoding { get; set; } // Not used for SSH
        public IpVersionType IpVersion { get; set; } // Not used for SSH
        public bool? ShowTelnetCodes { get; set; } // ShowTelnetCodes
        public bool? RemoveNonPrintableChars { get; set; } // RemoveNonPrintableChars
        public bool? ReplaceNonPrintableChars { get; set; } // ReplaceNonPrintableChars
        public bool? CustomBool1 { get; set; } // CustomBool1
        public string CustomString1 { get; set; } // CustomString1
        public int? CustomInt1 { get; set; } // CustomInt1
        public bool? CustomBool2 { get; set; } // CustomBool2
        public string CustomString2 { get; set; } // CustomString2
        public int? CustomInt2 { get; set; } // CustomInt2
        public bool? CustomBool3 { get; set; } // CustomBool3
        public string CustomString3 { get; set; } // CustomString3
        public int? CustomInt3 { get; set; } // CustomInt3
        public int MaxConcurrentConnections { get; set; } //You do not want to set this property lower than Max threads for dispatcher.
        public IEnumerable<LogSequence> LoginSequences { get; set; }
        public IEnumerable<LogSequence> LogoutSequences { get; set; } 
    }
}



