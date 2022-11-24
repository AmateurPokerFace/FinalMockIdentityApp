using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels.CoachControllerViewModels
{
    public class ChangeUserPasswordViewModel
    {
        [ValidateNever]
        public string? UsersName { get; set; }
        public string? UserId { get; set; }
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        public string? UserRole { get; set; }
    }
}
