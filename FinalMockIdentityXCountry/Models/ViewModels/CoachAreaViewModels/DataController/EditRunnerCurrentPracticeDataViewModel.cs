namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.DataController
{
    public class EditRunnerCurrentPracticeDataViewModel
    {
        public EditRunnerCurrentPracticeDataViewModel()
        {
            Workouts = new List<string>();
        }

        public List<string>? Workouts { get; set; }
        public string? PracticeLocation { get; set; }
        public DateTime PracticeStartDateTime { get; set; }
        public string? RunnersName { get; set; }
        public string? RunnerId { get; set; }
        public int PracticeId { get; set; }
        public string? Workout { get; set; }
    }
}
