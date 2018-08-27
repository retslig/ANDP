
namespace ANDP.Provisioning.API.Rest.Models.ApMax
{
    public class ServiceInfoType
    {
        public string ApSystemId { get; set; }
        public string BillingServiceAddress { get; set; }
        public string BillingServiceID { get; set; }
        public string ServiceGuid { get; set; }
        public ServiceType ServiceType { get; set; }
    }
}