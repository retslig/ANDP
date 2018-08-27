using System;
using Common.Lib.Utility;

namespace ANDP.Lib.Domain.Models
{
    public class Address
    {

        public int Id { get; set; }
        //public AddressType AddressType { get; set; } //"Ship to" or "bill to"
        public string Attention { get; set; }
        public string StreetLine1 { get; set; }
        public string StreetLine2 { get; set; }
        public string Municipality { get; set; } // City
        public string AdministrativeArea { get; set; } // State
        public string SubAdministrativeArea { get; set; } // County
        public string PostalCode { get; set; } //Zip
        public string Country { get; set; }
        public int CreatedById { get;  set; }
        public int ModifiedById { get;  set; }
        public DateTime DateCreated { get;  set; }
        public DateTime DateModified { get;  set; }
        public int Version { get;  set; }
        
        //The setter has to be public for nlog to be able to serialize this.
        //Dictionary isn't serializable and I need it to be for nlog usage. That's why I'm using the SerializableDictionary I built.
        public SerializableDictionary<string, string> ValidationErrors { get; set; }

        public bool Validate()
        {
            ValidationErrors = new SerializableDictionary<string, string>();

            //ToDo: Need to finish validation

            return ValidationErrors.Count > 0;
        }


    }
}
