using Common.MetaSwitch;

namespace ANDP.Provisioning.API.Rest.Models.MetaSwitch
{
    /// <summary>
    /// 
    /// </summary>
    public class MetaSwitchUpdate
    {
        /// <summary>
        /// Gets or sets the equipment identifier.
        /// </summary>
        /// <value>
        /// The equipment identifier.
        /// </value>
        public int EquipmentId { get; set; }

        /// <summary>
        /// Gets or sets the dn.
        /// </summary>
        /// <value>
        /// The dn.
        /// </value>
        public string Dn { get; set; }

        /// <summary>
        /// Gets or sets the user data.
        /// </summary>
        /// <value>
        /// The user data.
        /// </value>
        public tUserData UserData { get; set; }

    }
}