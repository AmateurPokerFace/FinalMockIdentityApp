namespace FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels
{
    public class SignOutOfPracticeViewModel
    {
        public int PracticeId { get; set; }
        public DateTime PracticeStartTimeAndDate { get; set; }
        public string? PracticeLocation { get; set; }
        public string? RunnerId { get; set; }
    }
}
