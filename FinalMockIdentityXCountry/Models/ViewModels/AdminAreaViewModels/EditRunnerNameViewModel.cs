using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels
{
    public class EditRunnerNameViewModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RunnerId { get; set; }
        [ValidateNever]
        public string? OldName { get; set; }
    }
}
