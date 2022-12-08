namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.ExportDataController
{
    public class ExportAllPracticeHistoryToCSVViewModel
    {
        public DateOnly PracticeDateOnly { get; set; }
        public TimeOnly PracticeStartTime { get; set; }
        public string? PracticeLocation { get; set; }
        public int AttendanceCount { get; set; }
        public DateTime PracticeFullDateTime { get; set; }
        public int PracticeId { get; set; }
    }
}
