
namespace ANDP.Lib.Domain.Models
{
    public class InternetUser
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ActionType ActionType { get; set; }
    }
}
