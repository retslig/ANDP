

using System.Collections.Generic;

namespace ANDP.API.Rest.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkFlowResponseDto
    {
        /// <summary>
        /// Gets or sets the equipment identifier.
        /// </summary>
        /// <value>
        /// The equipment identifier.
        /// </value>
        public int EquipmentId { get; set; }

        /// <summary>
        /// Gets or sets the case identifier.
        /// </summary>
        /// <value>
        /// The case identifier.
        /// </value>
        public string CaseId { get; set; }

        /// <summary>
        /// Gets or sets the response message.
        /// </summary>
        /// <value>
        /// The response message.
        /// </value>
        public string ResponseMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [response code].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [response code]; otherwise, <c>false</c>.
        /// </value>
        public string ResponseCode { get; set; }

        /// <summary>
        /// Gets or sets the dictionary.
        /// </summary>
        /// <value>
        /// The dictionary.
        /// </value>
        public List<KeyValueDto> Dictionary { get; set; }
    }
}
