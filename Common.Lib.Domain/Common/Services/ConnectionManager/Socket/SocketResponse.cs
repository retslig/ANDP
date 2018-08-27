using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Lib.Domain.Common.Services.ConnectionManager.Socket
{
    public class SocketResponse
    {
        public string Data { get; set; }
        public bool TimeoutOccurred{ get; set; }
    }
}
