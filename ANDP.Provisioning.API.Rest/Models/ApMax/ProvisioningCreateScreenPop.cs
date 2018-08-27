namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    /// <summary>
    /// 
    /// </summary>
    public class ProvisioningCreateScreenPop
    {
        /// <summary>
        /// Gets or sets the equipment identifier.
        /// </summary>
        /// <value>
        /// The equipment identifier.
        /// </value>
        public int EquipmentId { get; set; }
        /// <summary>
        /// Gets or sets the type of the screen pop subscriber.
        /// </summary>
        /// <value>
        /// The type of the screen pop subscriber.
        /// </value>
        public ScreenPopSubscriberType ScreenPopSubscriberType { get; set; }
        /// <summary>
        /// Gets or sets the npa NXX.
        /// </summary>
        /// <value>
        /// The npa NXX.
        /// </value>
        public string NpaNxx { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

    }
}