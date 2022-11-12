namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.CurrentPracticeController
{
    public class AttendanceViewModel
    {
        public AttendanceViewModel()
        {
            AttendanceViewModelHelpers = new List<AttendanceViewModelHelper>();
        }

        public string? PracticeLocation { get; set; }
        public DateTime PracticeStartTimeAndDate { get; set; }
        public List<AttendanceViewModelHelper>? AttendanceViewModelHelpers { get; set; }
    }
}
