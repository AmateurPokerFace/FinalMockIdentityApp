using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels
{
    [ValidateNever]
    public class WorkoutStatisticsViewModel
    {
        public DateTime PracticeDate { get; set; }
        public double Distance { get; set; }
        public string? TimeDisplayString { get; set; }
        public string? PaceDisplayString { get; set; }
        public string? Workout { get; set; }
        public double AveragePace { get; set; }
        public double FastestPace { get; set; }
        public double LongestDistance { get; set; }
    }
}
