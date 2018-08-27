using ANDP.Provisioning.API.Rest.Areas.HelpPage.ModelDescriptions;
using Common.MetaSphere;

namespace ANDP.Provisioning.API.Rest.Models.MetaSphere
{
    /// <summary>
    /// 
    /// </summary>
    public class MetaSphereUpdate
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