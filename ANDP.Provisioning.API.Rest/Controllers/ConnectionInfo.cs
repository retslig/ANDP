using System;
using Common.Lib.Domain.Common.Services.ConnectionManager;

namespace ANDP.Provisioning.API.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionInfo
    {
        private object _lockObj = new object();

        /// <summary>
        /// Gets or sets the connection manager service.
        /// </summary>
        /// <value>
        /// The connection manager service.
        /// </value>
        public ConnectionManagerService ConnectionManagerService { get; set; }
        
        /// <summary>
        /// Gets or sets the lock object.
        /// </summary>
        /// <value>
        /// The lock object.
        /// </value>
        public object LockObj
        {
            get { return _lockObj; }
            set { _lockObj = value; }
        }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public string SessionId { get; set; }
    }
}