using System.Collections.Generic;

namespace ANDP.Lib.Domain.Models
{
    public class AccountGroup
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public List<Account> Accounts { get; set; } 


    }
}
