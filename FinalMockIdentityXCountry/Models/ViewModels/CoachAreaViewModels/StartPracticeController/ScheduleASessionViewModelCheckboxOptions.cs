using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.StartPracticeController
{
    public class ScheduleASessionViewModelCheckboxOptions
    {
        [ValidateNever]
        public string? RunnerName { get; set; }
        [ValidateNever]
        public string? RunnerId { get; set; }
        [ValidateNever]
        public int PracticeId { get; set; }
        public bool IsAttending { get; set; }
    }
}
