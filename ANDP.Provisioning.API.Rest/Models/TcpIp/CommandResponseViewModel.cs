using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ANDP.Provisioning.API.Rest.Models.TcpIp
{
    public class CommandResponseViewModel
    {
        public string Data { get; set; }
        public bool TimeoutOccurred { get; set; }
        public string SessionId { get; set; }
    }
}