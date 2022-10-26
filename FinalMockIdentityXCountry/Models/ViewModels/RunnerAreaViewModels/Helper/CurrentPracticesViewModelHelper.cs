namespace FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels.Helper
{
    public class CurrentPracticesViewModelHelper
    {
        public int PracticeId { get; set; }
        public DateTime PracticeStartTimeAndDate { get; set; }
        public string? PracticeLocation { get; set; }
        public string? RunnerId { get; set; }
        public bool IsPresent { get; set; }
        public bool HasBeenSignedOut { get; set; }
    }
}
