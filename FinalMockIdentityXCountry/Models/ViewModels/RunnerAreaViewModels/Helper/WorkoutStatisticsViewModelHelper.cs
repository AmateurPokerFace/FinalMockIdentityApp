namespace FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels.Helper
{
    public class WorkoutStatisticsViewModelHelper
    {
        public (int, int) Pace { get; set; }
        public string? Workout { get; set; }
        public double Distance { get; set; }
        public string? TimeDisplayString { get; set; }
        public string? PaceDisplayString { get; set; }
        public DateTime PracticeDate { get; set; }
        public string? PracticeLocation { get; set; }
    }
}
