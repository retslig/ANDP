using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class CallingNameType
    {
        public int BgId { get; set; }
        public string Cname { get; set; }
        public string CallingNumber { get; set; }
        public string Presentation { get; set;}
        public bool UserOverride { get; set; }

    }
}