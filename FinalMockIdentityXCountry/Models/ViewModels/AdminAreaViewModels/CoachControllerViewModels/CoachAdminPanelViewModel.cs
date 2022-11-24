using FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels.MasterAdminControllerViewModels;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels.CoachControllerViewModels
{
    public class CoachAdminPanelViewModel
    {
        public CoachAdminPanelViewModel()
        {
            CoachAdminPanelViewModelViewHelpers = new List<CoachAdminPanelViewModelHelper>();
        }

        public string? CoachAdminPanelRole { get; set; }
        public List<CoachAdminPanelViewModelHelper>? CoachAdminPanelViewModelViewHelpers { get; set; }
    }
}
