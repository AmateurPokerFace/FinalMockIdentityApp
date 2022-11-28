namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.ExportDataController
{
    public class ExportRunnerPracticeDataToCSVViewModel
    {
        public string? RunnersName { get; set; }
        public string? PracticeLocation { get; set; }
        public DateOnly PracticeDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? WorkoutName { get; set; }
    }
}
