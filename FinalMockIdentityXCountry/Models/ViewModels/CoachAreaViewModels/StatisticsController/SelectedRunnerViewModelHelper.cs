namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.StatisticsController
{
    public class SelectedRunnerViewModelHelper
    {
        public string? WorkoutName { get; set; }
        public DateOnly PracticeDate { get; set; }
        public TimeSpan LoggedWorkoutTime { get; set; }
        public double Distance { get; set; }
        public (int, int) Pace { get; set; } // Index 0 = Minutes Index 1 = Seconds
        public string? PaceString { get; set; }

    }
}
