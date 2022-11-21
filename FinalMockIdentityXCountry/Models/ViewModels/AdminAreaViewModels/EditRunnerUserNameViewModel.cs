using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels
{
    public class EditRunnerUserNameViewModel
    {
        public string? NewUserName { get; set; }
        public string? RunnerId { get; set; }
        [ValidateNever]
        public string? CurrentName { get; set; }
    }
}
