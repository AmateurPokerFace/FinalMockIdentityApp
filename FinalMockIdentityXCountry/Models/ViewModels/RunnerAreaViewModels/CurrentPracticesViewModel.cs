using FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels.Helper;

namespace FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels
{
    public class CurrentPracticesViewModel
    {
        public CurrentPracticesViewModel()
        {
            CurrentPracticesViewModelsHelper = new List<CurrentPracticesViewModelHelper>();
        }

       
        public List<CurrentPracticesViewModelHelper>? CurrentPracticesViewModelsHelper { get; set; }
    }
}
