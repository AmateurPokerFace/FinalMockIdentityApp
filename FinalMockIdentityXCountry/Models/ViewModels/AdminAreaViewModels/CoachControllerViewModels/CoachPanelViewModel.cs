 namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels.CoachControllerViewModels
{
    public class CoachPanelViewModel
    {
        public CoachPanelViewModel()
        {
            CoachPanelViewModelHelpers = new List<CoachPanelViewModelHelper>();
        }

        public string? MasterAdminPanelRole { get; set; }
        public List<CoachPanelViewModelHelper>? CoachPanelViewModelHelpers { get; set; }
    }
}
