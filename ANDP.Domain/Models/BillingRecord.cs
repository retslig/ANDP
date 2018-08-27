using System;

namespace ANDP.Lib.Domain.Models
{
    public class BillingRecord
    {
        //Company
        public string ExternalCompanyId { get; set; }
        public int CompanyId { get; set; }

        //Accounts
        public string ExternalAccountId { get; set; }
        //public string ExternalAccountGroupId { get; set; }
        public string AccountName { get; set; }
        public string AccountPhone { get; set; }
        public string AccountType { get; set; } // Business or Residential

        //Services
        public string ExternalServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceClass { get; set; }
        public string ServiceLocation { get; set; }
        public string ServiceProvider { get; set; }
        public string ServicePhone { get; set; }
        public string ServiceStatus { get; set; } // Active, disabled

        //Item
        public string ExternalItemId { get; set; }

        //Features
        public string FeatureCode { get; set; }
        public string FeatureDescription { get; set; }
        public DateTime FeatureActiviationDate { get; set; }
        public DateTime FeatureDeactivationDate { get; set; }
        //public int FeatureQuantity { get; set; }
        public string FeaturePhone { get; set; }

        //Plant
        //Phone
        //public string AccessLineName { get; set; }
        //public int AccessLineNumber { get; set; }
        //public int OntPortNumber { get; set; }
        //public int OntIp { get; set; }

        //Video
        //public string SettopIp { get; set; }
        public int MaxPurchaseLimit { get; set; }
        public string PurchasePin { get; set; }
        public string RatingPin { get; set; }
        public string ServiceArea { get; set; }
        public string FipsCountyCode { get; set; }
        public string FipsStateCode { get; set; }
        public string HeadEnd { get; set; }

        //Internet
        //public string EmailAddress { get; set; }
    }
}
