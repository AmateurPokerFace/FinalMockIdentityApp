namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.PracticeHistoryController
{
    public class SelectedViewModelHelper
    {
        public SelectedViewModelHelper()
        {
            Workouts = new List<string>();
        }

        public string? RunnerName { get; set; }
        public string? RunnerId { get; set; }
        public List<string>? Workouts { get; set; }
        public int PracticeId { get; set; }
    }
}
