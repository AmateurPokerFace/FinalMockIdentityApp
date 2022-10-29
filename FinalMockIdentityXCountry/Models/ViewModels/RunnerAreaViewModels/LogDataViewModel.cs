namespace FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels
{
    public class LogDataViewModel
    {
        public int WorkoutInformationId { get; set; }
        public int PracticeId { get; set; }
        public string? RunnerId { get; set; }
        public DateTime PracticeStartDateTime { get; set; }
        public string? PracticeLocation { get; set; }
        public string? WorkoutName { get; set; }
        public double Pace { get; set; }
        public double Distance { get; set; } 
    }
}
