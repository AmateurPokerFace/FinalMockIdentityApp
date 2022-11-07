namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.DataController
{
    public class RunnerPracticeWorkoutDataViewModel
    {
        public RunnerPracticeWorkoutDataViewModel()
        {
            RunnerUsers = new List<ApplicationUser>();
        }

        public List<ApplicationUser> RunnerUsers { get; set; }
    }
}
