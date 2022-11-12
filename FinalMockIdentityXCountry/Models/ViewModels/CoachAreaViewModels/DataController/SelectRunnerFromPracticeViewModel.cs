namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.DataController
{
    public class SelectRunnerFromPracticeViewModel
    {
        public SelectRunnerFromPracticeViewModel()
        {
            SelectRunnerFromPracticeViewModelHelpers = new List<SelectRunnerFromPracticeViewModelHelper>();
        }

        public DateTime PracticeStartTimeAndDate { get; set; }
        public string? PracticeLocation { get; set; }
        public List<SelectRunnerFromPracticeViewModelHelper>? SelectRunnerFromPracticeViewModelHelpers { get; set; }
    }
}
