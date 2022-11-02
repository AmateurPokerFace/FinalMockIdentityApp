using FinalMockIdentityXCountry.Models.ViewModelHelperClasses;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class SelectedAthleteViewModel
    {
        public SelectedAthleteViewModel()
        {
            SelectedAthleteHelper = new List<SelectedAthleteViewModelHelper>();
        }
        
        public string? AthleteName { get; set; }
        public List<SelectedAthleteViewModelHelper>? SelectedAthleteHelper { get; set; }
    }
}
