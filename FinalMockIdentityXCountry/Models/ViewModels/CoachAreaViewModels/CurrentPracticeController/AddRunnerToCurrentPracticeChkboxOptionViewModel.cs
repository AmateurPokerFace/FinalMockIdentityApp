using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.CurrentPracticeController
{
    public class AddRunnerToCurrentPracticeChkboxOptionViewModel
    {
        [ValidateNever]
        public string RunnersName { get; set; }
        public string RunnerId { get; set; }
        public int PracticeId { get; set; }
        public bool IsSelected { get; set; }
    }
}
