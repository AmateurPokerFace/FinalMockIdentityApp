using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace FinalMockIdentityXCountry.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; } 
    }
}
