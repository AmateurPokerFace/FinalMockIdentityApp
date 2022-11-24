using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels.CoachControllerViewModels
{
    public class EditUserUsersNameViewModel
    {
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        [ValidateNever]
        public string? OldUserName { get; set; }
    }
}
