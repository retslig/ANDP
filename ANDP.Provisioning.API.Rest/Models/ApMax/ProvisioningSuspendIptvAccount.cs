
namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    /// <summary>
    /// 
    /// </summary>
    public class ProvisioningSuspendIptvAccount
    {
        /// <summary>
        /// Gets or sets the equipment identifier.
        /// </summary>
        /// <value>
        /// The equipment identifier.
        /// </value>
        public int EquipmentId { get; set; }

        /// <summary>
        /// Gets or sets the subscriber identifier.
        /// </summary>
        /// <value>
        /// The subscriber identifier. Either SubscriberId or ServiceReference need to be populated. Both aren't required.
        /// </value>
        public string SubscriberId { get; set; }

        /// <summary>
        /// Gets or sets the service reference.
        /// </summary>
        /// <value>
        /// The service reference. Either SubscriberId or ServiceReference need to be populated. Both aren't required.
        /// </value>
        public string ServiceReference { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ProvisioningSuspendIptvAccount"/> is suspend.
        /// </summary>
        /// <value>
        ///   <c>true</c> if suspend; otherwise, <c>false</c>.
        /// </value>
        public bool Suspend { get; set; }


        /// <summary>
        /// Gets or sets the suspend reason.
        /// </summary>
        /// <value>
        /// The suspend reason.
        /// </value>
        public string SuspendReason { get; set; }
    }
}