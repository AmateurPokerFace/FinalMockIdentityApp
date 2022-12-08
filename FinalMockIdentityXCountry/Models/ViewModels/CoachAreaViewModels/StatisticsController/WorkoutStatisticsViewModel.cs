using FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels.Helper;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.StatisticsController
{
    [ValidateNever]
    public class WorkoutStatisticsViewModel
    {
        public WorkoutStatisticsViewModel()
        {
            WorkoutStatisticsViewModelHelpers = new List<WorkoutStatisticsViewModelHelper>();
        }

        public TimeSpan AveragePace { get; set; }
        public TimeSpan FastestPace { get; set; }
        public double LongestDistance { get; set; }
        public List<WorkoutStatisticsViewModelHelper>? WorkoutStatisticsViewModelHelpers { get; set; }
    }
}
