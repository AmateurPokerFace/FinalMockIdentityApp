using FinalMockIdentityXCountry.Models.ViewModelHelperClasses;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels
{
    public class WaitingForApprovalListViewModel
    {
        
        public WaitingForApprovalListViewModel()
        {
            SelectedApproveUserCheckboxOptions = new List<ApproveUserCheckboxOptions>();
        }

        [ValidateNever]
        public string? Name { get; set; }
        [ValidateNever]
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
        public List<ApproveUserCheckboxOptions>? SelectedApproveUserCheckboxOptions { get; set; }
         
    }
}
