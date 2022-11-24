using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels.CoachControllerViewModels
{
    public class ChangeUserRoleViewModel
    {
        public ChangeUserRoleViewModel()
        {
            RolesList = new List<SelectListItem>();
        }

        public string? SelectedRoleName { get; set; }
        public string? CurrentRoleName { get; set; }
        public string? UserId { get; set; }

        [ValidateNever]
        public List<SelectListItem>? RolesList { get; set; }
    }
}
