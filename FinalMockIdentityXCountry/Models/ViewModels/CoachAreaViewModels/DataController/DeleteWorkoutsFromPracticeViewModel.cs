namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.DataController
{
    public class DeleteWorkoutsFromPracticeViewModel
    {
        public DeleteWorkoutsFromPracticeViewModel()
        {
            SelectedCheckboxOptions = new List<DeleteWorkoutsFromPracticeCheckBoxOptions>();
        }

        public List<DeleteWorkoutsFromPracticeCheckBoxOptions>? SelectedCheckboxOptions { get; set; }
        public string? RunnerName { get; set; }
        public string? RunnerId { get; set; }
        public string? PracticeLocation { get; set; }
        public DateTime PracticeStartDateTime { get; set; }
        public int PracticeId { get; set; }
        public int WorkoutInformationId { get; set; }
    }
}
