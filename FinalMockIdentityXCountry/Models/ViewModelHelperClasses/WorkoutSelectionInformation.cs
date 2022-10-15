using Microsoft.AspNetCore.Identity;

namespace FinalMockIdentityXCountry.Models.ViewModelHelperClasses
{
    public class WorkoutSelectionInformation
    {
        public int WorkoutTypeId { get; set; } 
        public bool WorkoutIsSelected { get; set; } 
        public int PracticeId { get; set; }
        public DateTime WorkoutDateTime { get; set; }
        public string WorkoutName { get; set; }
        public string RunnerId { get; set; }
        public string PracticeLocation { get; set; }

    }
}
