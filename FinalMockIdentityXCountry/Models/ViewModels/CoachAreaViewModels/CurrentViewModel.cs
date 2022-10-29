namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class CurrentViewModel
    {
        public CurrentViewModel()
        {
            Runners = new List<string>();
        }

        public int practiceId { get; set; }
        public List<string>? Runners{ get; set; }
        public string? RunnerName { get; set; }
        public DateTime? PracticeStartTimeAndDate { get; set; }
        public string? PracticeLocation { get; set; }
    }
}
