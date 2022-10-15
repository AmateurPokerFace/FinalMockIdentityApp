namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class EndViewModel
    {
        public EndViewModel()
        {
            ApplicationUsers = new List<ApplicationUser>();
        }
        public List<ApplicationUser>? ApplicationUsers { get; set; }
        public IEnumerable<Attendance>? Attendances { get; set; }
        public Attendance? Attendance { get; set; }
    }
}
