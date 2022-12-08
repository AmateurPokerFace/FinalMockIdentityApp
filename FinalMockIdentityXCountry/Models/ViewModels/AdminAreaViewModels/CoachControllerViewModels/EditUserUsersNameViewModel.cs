using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels.CoachControllerViewModels
{
    public class EditUserUsersNameViewModel
    {
        [Required]
        [RegularExpression(@"([a-zA-Z\d]+[\w\d]*|)[a-zA-Z]+[\w\d.]*",
            ErrorMessage = "The Username field only accepts letters, numbers, and underscores and must start with a letter.")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        public string UserName { get; set; }
        [Required]
        public string UserId { get; set; }
        [ValidateNever]
        public string? OldUserName { get; set; }
    }
}
