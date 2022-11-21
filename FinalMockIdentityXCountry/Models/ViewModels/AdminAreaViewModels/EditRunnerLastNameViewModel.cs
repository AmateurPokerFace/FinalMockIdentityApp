using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels
{
    public class EditRunnerLastNameViewModel
    { 
        public string? NewLastName { get; set; }
        public string? RunnerId { get; set; }
        [ValidateNever]
        public string? CurrentName { get; set; }
    }
}
