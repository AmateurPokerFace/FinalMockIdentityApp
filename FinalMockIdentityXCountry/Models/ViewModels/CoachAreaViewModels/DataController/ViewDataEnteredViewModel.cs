namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.DataController
{
    public class ViewDataEnteredViewModel
    {
        public ViewDataEnteredViewModel()
        {
            ViewDataEnteredViewModelHelpers = new List<ViewDataEnteredViewModelHelper>();
        }
     
        public string? RunnerName { get; set; }
        public string? PracticeLocation { get; set; }
        public DateTime PracticeStartDateTime { get; set; }
        public int PracticeId { get; set; }
        public string? RunnerId { get; set; }
        public List<ViewDataEnteredViewModelHelper>? ViewDataEnteredViewModelHelpers { get; set; }
    }
}
