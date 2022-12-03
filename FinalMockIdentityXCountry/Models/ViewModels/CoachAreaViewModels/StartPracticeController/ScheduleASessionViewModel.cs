using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.StartPracticeController
{
    public class ScheduleASessionViewModel
    {
        public ScheduleASessionViewModel()
        {
            SelectedStartNowCheckBoxOptions = new List<StartNowCheckBoxOptions>();
        }

        public List<StartNowCheckBoxOptions>? SelectedStartNowCheckBoxOptions { get; set; }
        [Required]
        public DateTime PracticeStartTimeAndDate { get; set; }
        [Required]
        public DateTime PracticeEndTimeAndDate { get; set; }
        [StringLength(100, MinimumLength = 2)]
        [Required]
        public string PracticeLocation { get; set; }
    }
}
