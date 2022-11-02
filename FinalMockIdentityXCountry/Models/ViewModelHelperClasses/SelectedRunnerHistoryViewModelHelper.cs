namespace FinalMockIdentityXCountry.Models.ViewModelHelperClasses
{
    public class SelectedRunnerHistoryViewModelHelper
    {
        public SelectedRunnerHistoryViewModelHelper()
        {
            PracticeWorkouts = new List<string>();
        }

        public int PracticeId { get; set; }
        public DateOnly PracticeStartDate { get; set; }
        public string? PracticeLocation { get; set; }
        public string? RunnerId { get; set; }
        public List<string> PracticeWorkouts { get; set; }
        
    }
}
