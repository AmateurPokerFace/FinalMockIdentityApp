using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.StartPracticeController
{
    public class ScheduleASessionViewModel
    {
        public ScheduleASessionViewModel()
        {
            SessionViewModelCheckboxOptions = new List<ScheduleASessionViewModelCheckboxOptions>();
        }


        public DateTime PracticeStartTimeAndDate { get; set; }
        public DateTime PracticeEndTimeAndDate { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? PracticeLocation { get; set; }

        public List<ScheduleASessionViewModelCheckboxOptions> SessionViewModelCheckboxOptions { get; set; }
    }
}
