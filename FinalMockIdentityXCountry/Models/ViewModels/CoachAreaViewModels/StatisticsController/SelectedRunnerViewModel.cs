namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.StatisticsController
{
    public class SelectedRunnerViewModel
    {
        public SelectedRunnerViewModel()
        {
            SelectedRunnerViewModelHelpers = new List<SelectedRunnerViewModelHelper>();
        }

        public string? RunnerName { get; set; }
        public double AveragePace { get; set; }
        public double FastestPace { get; set; }
        public double LongestDistance { get; set; }
        public List<SelectedRunnerViewModelHelper>? SelectedRunnerViewModelHelpers { get; set; }
    }
}
