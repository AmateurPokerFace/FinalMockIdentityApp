namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels.MasterAdminControllerViewModels
{
    public class MasterAdminPanelViewModel
    {
        public MasterAdminPanelViewModel()
        {
            MasterAdminPanelViewModelHelpers = new List<MasterAdminPanelViewModelHelper>();
        }

        public string? MasterAdminPanelRole { get; set; }
        public List<MasterAdminPanelViewModelHelper>? MasterAdminPanelViewModelHelpers { get; set; }
    }
}
