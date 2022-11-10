using FinalMockIdentityXCountry.Models.ViewModelHelperClasses;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.DataController
{
    public class AddNewWorkoutsToPracticeViewModel
    {
        public AddNewWorkoutsToPracticeViewModel()
        {
            SelectedNewWorkoutCheckboxOptions = new List<NewWorkoutCheckboxOptions>();
        }

        public List<NewWorkoutCheckboxOptions>? SelectedNewWorkoutCheckboxOptions { get; set; }
        public string? RunnerName { get; set; }
        public string? RunnerId { get; set; }
        public int PracticeId { get; set; }
        public string? PracticeLocation { get; set; }
        public DateTime PracticeStartDateTime { get; set; }

    }
}
