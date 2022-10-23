using FinalMockIdentityXCountry.Models.ViewModelHelperClasses;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class AddPracticeWorkoutsViewModel
    {
        public AddPracticeWorkoutsViewModel()
        {
            WorkoutCheckboxOptions = new List<AddPracticeWorkoutCheckboxOptions>();
        }

        public List<AddPracticeWorkoutCheckboxOptions>? WorkoutCheckboxOptions { get; set; }
        public string? RunnerName { get; set; }
        public string? RunnerId { get; set; }
        public int PracticeId { get; set; }

    }
}
