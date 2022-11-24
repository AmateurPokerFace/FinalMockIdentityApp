using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels.CoachControllerViewModels
{
    public class DeleteUserViewModel
    {
        [ValidateNever]
        public string? UsersName { get; set; }
        public string? UserId { get; set; }
    }
}
