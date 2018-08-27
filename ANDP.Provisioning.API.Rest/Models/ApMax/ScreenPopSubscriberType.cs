using System.Collections.Generic;

namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    /// <summary>
    /// 
    /// </summary>
    public class ScreenPopSubscriberType
    {
        /// <summary>
        /// Gets or sets the mac addresses.
        /// </summary>
        /// <value>
        /// The mac addresses.
        /// </value>
        public List<string> MacAddresses { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [screen pop enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [screen pop enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool ScreenPopEnabled { get; set; }
        /// <summary>
        /// Gets or sets the subscriber phone number.
        /// </summary>
        /// <value>
        /// The subscriber phone number.
        /// </value>
        public string SubscriberPhoneNumber { get; set; }
    }
}