using System.Collections.Generic;

namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    /// <summary>
    /// 
    /// </summary>
    public class ProvisioningUpdateIptvServiceReference
    {
        /// <summary>
        /// Gets or sets the old service reference.
        /// </summary>
        /// <value>
        /// The old service reference.
        /// </value>
        public string OldServiceReference { get; set; }
        /// <summary>
        /// Gets or sets the new service reference.
        /// </summary>
        /// <value>
        /// The new service reference.
        /// </value>
        public string NewServiceReference { get; set; }
        /// <summary>
        /// Gets or sets the equipment identifier.
        /// </summary>
        /// <value>
        /// The equipment identifier.
        /// </value>
        public int EquipmentId { get; set; }
    }
}