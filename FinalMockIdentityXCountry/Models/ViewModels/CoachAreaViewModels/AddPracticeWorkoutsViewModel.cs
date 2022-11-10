using FinalMockIdentityXCountry.Models.ViewModelHelperClasses;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class AddPracticeWorkoutsViewModel
    {
        public AddPracticeWorkoutsViewModel()
        {
            SelectedWorkoutCheckboxOptions = new List<AddPracticeWorkoutCheckboxOptions>();
        }

        public List<AddPracticeWorkoutCheckboxOptions>? SelectedWorkoutCheckboxOptions { get; set; }
        public string? RunnerName { get; set; }
        public string? RunnerId { get; set; }
        public int PracticeId { get; set; }

    }
}
