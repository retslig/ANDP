using System;
using System.Collections.Generic;
using Common.Lib.Utility;

namespace ANDP.Lib.Domain.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MasterPhone { get; set; } 
        public string ExternalAccountId { get; set; }
        public string ExternalAccountGroupId { get; set; }
        public int CompanyId { get; set; }
        public string Type { get; set; } // Business or Residential
        public string BillCycle { get; set; } 
        public StatusType StatusType { get; set; }
        public string CreatedByUser { get; set; }
        public string ModifiedByUser { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int Version { get; set; }
        public Contact Contact { get; set; }

        //The setter has to be public for nlog to be able to serialize this.
        //Dictionary isn't serializable and I need it to be for nlog usage. That's why I'm using the SerializableDictionary I built.
        public SerializableDictionary<string, string> ValidationErrors { get; set; }

        public bool Validate()
        {
            ValidationErrors = new SerializableDictionary<string, string>();

            if (string.IsNullOrEmpty(ExternalAccountId))
            {
                ValidationErrors.Add(LambdaHelper<Account>.GetPropertyName(x => x.ExternalAccountId), "Account.ExternalAccountId is a mandatory field.");
            }

            return ValidationErrors.Count > 0;
        }
    }
}
