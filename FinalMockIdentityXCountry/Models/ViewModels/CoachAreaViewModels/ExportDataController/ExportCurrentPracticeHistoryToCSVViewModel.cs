namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.ExportDataController
{
    public class ExportCurrentPracticeHistoryToCSVViewModel
    { 
        public DateOnly PracticeDate { get; set; }
        public TimeOnly PracticeStartTime { get; set; }
        public string? PracticeLocation { get; set; }
        public int AttendanceCount { get; set; }
    }
}
