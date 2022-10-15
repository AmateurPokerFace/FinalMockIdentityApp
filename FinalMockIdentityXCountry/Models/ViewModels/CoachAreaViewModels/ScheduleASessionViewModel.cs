using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class ScheduleASessionViewModel
    {
        public ScheduleASessionViewModel()
        { 
            RunnerUsers = new List<ApplicationUser>();
        }

        public List<ApplicationUser> RunnerUsers { get; set; }
        [Required]
        public DateTime PracticeStartTimeAndDate { get; set; }
        [Required]
        public DateTime PracticeEndTimeAndDate { get; set; }
        [StringLength(100, MinimumLength = 2)]
        [Required]
        public string PracticeLocation { get; set; }

    }
}
