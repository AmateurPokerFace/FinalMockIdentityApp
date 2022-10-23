namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.Delete
{
    public class RecordAttendanceViewModel
    {
        public virtual List<ApplicationUser>? Runners { get; set; }
        public virtual ApplicationUser? UserCoach { get; set; }
        public Practice? Practice { get; set; }
        public Attendance? Attendance { get; set; }
        public string[]? PresentRunnersIds { get; set; }
    }
}
