using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels
{
    public class EditRunnerUserNameViewModel
    {
        public string? UserName { get; set; }
        public string? RunnerId { get; set; }
        [ValidateNever]
        public string? OldUserName { get; set; }
    }
}
