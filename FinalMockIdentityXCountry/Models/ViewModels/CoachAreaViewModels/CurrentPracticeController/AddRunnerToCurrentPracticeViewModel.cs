using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.CurrentPracticeController
{
    public class AddRunnerToCurrentPracticeViewModel
    {
        public AddRunnerToCurrentPracticeViewModel()
        {
            AddRunnerToCurrentPracticeCheckboxOptions = new List<AddRunnerToCurrentPracticeViewModelCheckboxOption>();
        }

        [ValidateNever]
        public string? PracticeLocation { get; set; }
        [ValidateNever]
        public DateTime PracticeStartDateTime { get; set; } 
        public List<AddRunnerToCurrentPracticeViewModelCheckboxOption>? AddRunnerToCurrentPracticeCheckboxOptions { get; set; }
    }
}
