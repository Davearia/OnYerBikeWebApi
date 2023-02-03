
using System.ComponentModel.DataAnnotations;

namespace Data.Dtos
{

    public class LoginUserDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(15, ErrorMessage = "Your password is limited to {2} to {1} characters", MinimumLength = 1)]
        public string Password { get; set; } = string.Empty;

    }

    /// <summary>
    /// Class to persist user logging into site for auth purposes, no relationship with User.cs as this just readonly data gleaned from the Adventureworks2019 DB
    /// </summary>
    public class UserDto : LoginUserDto
    {

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<string> Roles { get; set;} = new List<string>();
      
    }
}
