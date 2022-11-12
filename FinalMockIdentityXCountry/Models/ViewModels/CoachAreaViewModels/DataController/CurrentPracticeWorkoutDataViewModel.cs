namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.DataController
{
    public class CurrentPracticeWorkoutDataViewModel
    {
        public int PracticeId { get; set; }
        public DateTime PracticeDateTime { get; set; }
        public string? PracticeLocation { get; set; }
        public int TotalRunners { get; set; }
    }
}
