namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class SelectedRunnerHistoryViewModel
    {
        public SelectedRunnerHistoryViewModel()
        {
            PracticeWorkouts = new List<string>();
        }

        public int PracticeId { get; set; }
        public TimeOnly PracticeStartTime { get; set; }
        public TimeOnly PracticeEndingTime { get; set; }
        public string? PracticeLocation { get; set; }
        public string? RunnerId { get; set; }
        public List<string> PracticeWorkouts { get; set; }
        public string? RunnersName { get; set; }
    }
}
