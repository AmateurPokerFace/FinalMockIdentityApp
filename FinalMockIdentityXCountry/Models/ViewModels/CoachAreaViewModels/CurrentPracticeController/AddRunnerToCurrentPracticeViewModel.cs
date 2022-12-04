using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.CurrentPracticeController
{
    public class AddRunnerToCurrentPracticeViewModel
    {
        public AddRunnerToCurrentPracticeViewModel()
        {
            SelectedCheckboxOptions = new List<AddRunnerToCurrentPracticeChkboxOptionViewModel>();
        }

        [ValidateNever]
        public string? PracticeLocation{ get; set; }
        [ValidateNever]
        public DateTime PracticeStartTimeAndDate { get; set; }
        public List<AddRunnerToCurrentPracticeChkboxOptionViewModel>? SelectedCheckboxOptions { get; set; }
    }
}
