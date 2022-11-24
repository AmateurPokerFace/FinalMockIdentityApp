using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels
{
    public class AdminPanelViewModel
    {
        public AdminPanelViewModel()
        {
            AdminPanelViewModelHelpers = new List<AdminPanelViewModelHelper>();
        }
        
        public string? AdminPanelRole { get; set; }
        public List<AdminPanelViewModelHelper>? AdminPanelViewModelHelpers { get; set; }

    }
}
