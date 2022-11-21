using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels
{
    public class EditRunnerNameViewModel
    {
        public string? NewFirstName { get; set; }
        public string? NewLastName { get; set; }
        public string? RunnerId { get; set; }
        [ValidateNever]
        public string? CurrentName { get; set; }
    }
}
