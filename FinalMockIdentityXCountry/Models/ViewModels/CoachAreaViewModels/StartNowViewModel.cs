using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class StartNowViewModel
    {
        public StartNowViewModel()
        { 
            RunnerUsers = new List<ApplicationUser>();
        }

        public List<ApplicationUser> RunnerUsers { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string PracticeLocation { get; set; }
    }
}
