
using Microsoft.AspNetCore.Identity;

namespace Data.Entities
{

    /// <summary>
    /// Extends the AspNetUser entity
    /// </summary>
    public class ApiUser : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
