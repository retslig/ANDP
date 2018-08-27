using System;
using Common.Lib.Common.Enums;

namespace Common.Lib.Domain.Common.Models
{
    public class ApplicationLog
    {
        public int LogId { get; set; }
        public Guid SearchKey { get; set; }
        public string SourceMachineName { get; set; }
        public string AppCode { get; set; }
        public int Pid { get; set; }
        public string UserCode { get; set; }
        public DateTime LoggedDateTime { get; set; }
        public LogLevelType MessageType { get; set; }
        public string MessageData { get; set; }
        public string StackTrace { get; set; }
        public string ExceptionMessage { get; set; }
        public string DestinationMachineName { get; set; }
    }
}
