using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.StartPracticeController
{
    public class StartNowViewModel
    {
        public StartNowViewModel()
        {
            SelectedStartNowCheckBoxOptions = new List<StartNowCheckBoxOptions>();
        }


        public List<StartNowCheckBoxOptions>? SelectedStartNowCheckBoxOptions { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string PracticeLocation { get; set; }
    }
}
