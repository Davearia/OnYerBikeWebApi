
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class User
    {

        [Key]
        public int? UserId { get; set; }
   
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AddressLine { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }

    }
}
