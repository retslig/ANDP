
namespace ANDP.API.Rest.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkFlowTriggerDto
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
        /// Gets or sets the trigger identifier.
        /// </summary>
        /// <value>
        /// The trigger identifier.
        /// </value>
        public string TriggerId { get; set; }
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public string Index { get; set; }
    }
}