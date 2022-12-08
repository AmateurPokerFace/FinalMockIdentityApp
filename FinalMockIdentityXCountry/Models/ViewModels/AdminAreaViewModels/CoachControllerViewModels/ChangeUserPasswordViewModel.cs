using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels.CoachControllerViewModels
{
    public class ChangeUserPasswordViewModel
    {
        [ValidateNever]
        public string UsersName { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }

        [ValidateNever]
        public string UserRole { get; set; }
    }
}
