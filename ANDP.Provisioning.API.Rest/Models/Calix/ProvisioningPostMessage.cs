using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ANDP.Provisioning.API.Rest.Models.Calix
{
    public class ProvisioningPostMessage
    {
        public int EquipmentId { get; set; }
        public string SoapMessage { get; set; }
    }
}