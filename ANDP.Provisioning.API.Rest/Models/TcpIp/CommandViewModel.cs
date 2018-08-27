
using System;
using System.Collections.Generic;

namespace ANDP.Provisioning.API.Rest.Models.TcpIp
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandViewModel
    {
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the expected response.
        /// </summary>
        /// <value>
        /// The expected response.
        /// </value>
        public List<string> ExpectedResponse { get; set; }

        /// <summary>
        /// Gets or sets the expected response reg ex.
        /// </summary>
        /// <value>
        /// The expected response reg ex.
        /// </value>
        public string ExpectedResponseRegEx { get; set; }

        /// <summary>
        /// Gets or sets the equipment identifier.
        /// </summary>
        /// <value>
        /// The equipment identifier.
        /// </value>
        public int EquipmentId { get; set; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public string SessionId { get; set; }
    }
}