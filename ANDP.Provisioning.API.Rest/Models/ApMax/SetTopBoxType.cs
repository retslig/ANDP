
namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class SetTopBoxType
    {
        public int AllowedStreamCount { get; set; }
        public bool Authorized { get; set; }
        public int BandwidthGroup { get; set; }
        public int FipsCountyCode { get; set; }
        public int FipsStateCode { get; set; }
        public int InstallDate { get; set; }
        public int MaxBandwidthKbps { get; set; }
        public int MaxSimultaneousRecordings { get; set; }
        public int RfChannel { get; set; }
        public string StbModel { get; set; }
        public string SerialNumber { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public int WholeHomeGroup { get; set; }
        public int HdmiMode { get; set; }
        public int? IRBand { get; set; }
        public bool? InactivityDetuneOverride { get; set; }
        public string STBName { get; set; }
        public int WiringType { get; set; }
    }
}