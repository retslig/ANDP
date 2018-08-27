
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ANDP.ProvisionCenter.Mvc.Models
{
    public class LogInModel
    {
        [Required]
        [DisplayName("Tenant Name")]
        public string TenantName { get; set; }

        [Required]
        public string Username { get; set; }

        //[Required]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [HiddenInput]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        public string RememberMe { get; set; }

        [HiddenInput]
        public bool RenewSession { get; set; }
    }
}
