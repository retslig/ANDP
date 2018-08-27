
namespace ANDP.API.Rest.Models
{
    /// <summary>
    /// Service object for Web APi.
    /// </summary>
    public class OrderDto
    {
        /// <summary>
        /// Gets or sets the external company identifier.
        /// </summary>
        /// <value>
        /// The external company identifier.
        /// </value>
        public string ExternalCompanyId { get; set; }

        /// <summary>
        /// Gets or sets the external order identifier.
        /// </summary>
        /// <value>
        /// The external order identifier.
        /// </value>
        public string ExternalOrderId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [test mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [test mode]; otherwise, <c>false</c>.
        /// </value>
        public bool TestMode { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether [send response].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [send response]; otherwise, <c>false</c>.
        /// </value>
        public bool SendResponse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [force provision].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [force provision]; otherwise, <c>false</c>.
        /// </value>
        public bool ForceProvision { get; set; }
    }
}
