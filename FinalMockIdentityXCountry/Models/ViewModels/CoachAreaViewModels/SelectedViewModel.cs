namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class SelectedViewModel
    {
        public SelectedViewModel()
        {
            PracticeWorkouts = new List<string>();
        }

        public int PracticeId { get; set; }
        public TimeOnly PracticeStartTime { get; set; }
        public TimeOnly PracticeEndingTime { get; set; }
        public string? RunnerId { get; set; }
        public List<string> PracticeWorkouts { get; set; }
        public string? RunnersName { get; set; }
    }
}
