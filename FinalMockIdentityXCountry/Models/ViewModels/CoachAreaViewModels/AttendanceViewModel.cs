namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class AttendanceViewModel
    {
        public AttendanceViewModel()
        {
            Runners = new List<string>();
        }

        public DateTime PracticeStartTimeAndDate { get; set; }
        public string? PracticeLocation { get; set; }
        public List<string>? Runners { get; set; }
    }
}
