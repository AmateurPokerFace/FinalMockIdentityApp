namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class AllViewModel
    {
        public AllViewModel()
        {
            RunnerUsers = new List<ApplicationUser>();
        }

        public List<ApplicationUser> RunnerUsers { get; set; }
    }
}
