using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.DataController
{
    public class NewWorkoutCheckboxOptions
    {
        public int PracticeId { get; set; }
        public string? RunnerId { get; set; }
        public int WorkoutTypeId { get; set; }
        public string? WorkoutName { get; set; } 
        public bool IsSelected { get; set; }
    }
}
