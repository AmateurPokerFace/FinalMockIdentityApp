using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.StartPracticeController
{
    public class StartNowViewModel
    {
        public StartNowViewModel()
        {
            SessionViewModelCheckboxOptions = new List<ScheduleASessionViewModelCheckboxOptions>();
        }


        [ValidateNever]
        public DateTime PracticeStartTimeAndDate { get; set; } 
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? PracticeLocation { get; set; }

        public List<ScheduleASessionViewModelCheckboxOptions> SessionViewModelCheckboxOptions { get; set; }
    }
}
