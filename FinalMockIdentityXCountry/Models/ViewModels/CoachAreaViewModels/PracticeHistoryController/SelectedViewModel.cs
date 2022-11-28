namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.PracticeHistoryController
{
    public class SelectedViewModel
    {
        public SelectedViewModel()
        {
            SelectedViewModelHelpers = new List<SelectedViewModelHelper>();
        }
        public string? PracticeLocation { get; set; }
        public TimeOnly PracticeStartTime { get; set; }
        public TimeOnly PracticeEndTime { get; set; }
        public List<SelectedViewModelHelper>? SelectedViewModelHelpers { get; set; }
    }
}
