using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.DataController
{
    public class EditWorkoutDataViewModel
    {
        public EditWorkoutDataViewModel()
        {
            Workouts = new List<string>();
        }

        public string? RunnerName { get; set; }
        public string? RunnerId { get; set; }
        public int PracticeId { get; set; }
        public DateTime PracticeStartTime { get; set; }
        public string? PracticeLocation { get; set; }
        public List<string>? Workouts { get; set; }
        [ValidateNever]
        public bool ShowReadDeleteButtons { get; set; } // show delete workout and view/edit data entered hyperlinks
    }
}
