namespace ANDP.Lib.Domain.Models
{
    public class Contact
    {
        public string Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Corporation { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
        public Address OldAddress { get; set; }
    }
}
