using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.CurrentPracticeController
{
    public class AddRunnerToCurrentPracticeViewModelCheckboxOption
    {
        [ValidateNever]
        public string? RunnerName { get; set; }
        [ValidateNever]
        public string? RunnerId { get; set; }
        public bool IsSelected { get; set; }
        public int AttendanceId { get; set; }
    }
}
