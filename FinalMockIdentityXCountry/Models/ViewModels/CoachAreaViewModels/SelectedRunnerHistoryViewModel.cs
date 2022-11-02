using FinalMockIdentityXCountry.Models.ViewModelHelperClasses;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class SelectedRunnerHistoryViewModel
    {
        public SelectedRunnerHistoryViewModel()
        {
            PracticeWorkouts = new List<string>();
        }

        public string? RunnersName { get; set; }
        public int PracticeId { get; set; }
        public DateOnly PracticeStartDate { get; set; }
        public string? PracticeLocation { get; set; }
        public string? RunnerId { get; set; }
        public List<string>? PracticeWorkouts { get; set; }

    }
}
