namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.ExportDataController
{
    public class ExportAthletePracticeHistoryToCSVViewModel
    { 
        public string? PracticeLocation { get; set; }
        public DateOnly PracticeDate { get; set; }
        public TimeOnly PracticeStartTime { get; set; }
        public string? RunnersName { get; set; }
    }
}
