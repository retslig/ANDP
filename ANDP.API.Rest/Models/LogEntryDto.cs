using Common.Lib.Common.Enums;

namespace ANDP.API.Rest.Models
{
    public class LogEntryDto
    {
        public string Entry { get; set; }
        public LogLevelType LogLevelType { get; set; }
    }
}