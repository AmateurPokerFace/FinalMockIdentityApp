using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels.CoachControllerViewModels
{
    public class EditUsersNameViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [DisplayName("First Name")]
        [StringLength(maximumLength: 35, MinimumLength = 1, ErrorMessage = "First Name must be between 1 and 35 characters")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayName("Last Name")]
        [StringLength(maximumLength: 35, MinimumLength = 1, ErrorMessage = "Last Name must be between 1 and 35 characters")]
        public string LastName { get; set; }

        [Required]
        public string UserId { get; set; }

        [ValidateNever]
        public string? OldName { get; set; }
    }
}
