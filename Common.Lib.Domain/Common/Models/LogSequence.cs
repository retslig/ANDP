
namespace Common.Lib.Domain.Common.Models
{
    public class LogSequence
    {
        public int SequenceNumber { get; set; } // SequenceNumber
        public string Command { get; set; } // Command
        public string ExpectedResponse { get; set; } // ExpectedResponse
        public int Timeout { get; set; } // Timeout
    }
}
