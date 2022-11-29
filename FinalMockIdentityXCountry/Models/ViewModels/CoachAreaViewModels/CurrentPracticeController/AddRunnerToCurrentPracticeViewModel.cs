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
        public string? Name { get; set; }
        [ValidateNever]
        public string? RunnerId { get; set; }
        public int AttendanceId { get; set; }
        public string? PracticeLocation{ get; set; }
        public DateTime PracticeStartTimeAndDate { get; set; }
        public List<AddRunnerToCurrentPracticeChkboxOptionViewModel>? SelectedCheckboxOptions { get; set; }
    }
}
