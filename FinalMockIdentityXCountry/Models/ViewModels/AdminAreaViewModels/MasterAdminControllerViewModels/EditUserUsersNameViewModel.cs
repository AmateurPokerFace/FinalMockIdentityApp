using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels.MasterAdminControllerViewModels
{
    public class EditUserUsersNameViewModel
    {
        
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$",
         ErrorMessage = "Characters are not allowed.")]
        public string UserName { get; set; }
        [Required]
        public string UserId { get; set; }
        [ValidateNever]
        public string? OldUserName { get; set; }
    }
}
