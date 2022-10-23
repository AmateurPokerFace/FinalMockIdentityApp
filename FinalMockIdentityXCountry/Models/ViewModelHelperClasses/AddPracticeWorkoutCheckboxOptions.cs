namespace FinalMockIdentityXCountry.Models.ViewModelHelperClasses
{
    public class AddPracticeWorkoutCheckboxOptions
    {
        public int PracticeId { get; set; }
        public string? RunnerId { get; set; }
        public int WorkoutTypeId { get; set; }
        public WorkoutType? WorkoutType { get; set; }
        public bool IsSelected { get; set; }
    }
}
