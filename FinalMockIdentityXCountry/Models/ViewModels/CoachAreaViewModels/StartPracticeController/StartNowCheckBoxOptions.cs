using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.StartPracticeController
{
    public class StartNowCheckBoxOptions
    {
        public string RunnersId { get; set; }
        public bool IsSelected { get; set; }
        [ValidateNever]
        public string RunnersName { get; set; }
    }
}
