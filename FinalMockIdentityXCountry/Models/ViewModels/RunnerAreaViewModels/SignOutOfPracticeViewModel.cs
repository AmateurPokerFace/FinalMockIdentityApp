using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;

namespace FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels
{
    public class SignOutOfPracticeViewModel
    {
        [Required]
        public int PracticeId { get; set; }

        [ValidateNever]
        public DateTime PracticeStartTimeAndDate { get; set; }

        [ValidateNever]
        public string PracticeLocation { get; set; }
        
        [Required]
        public string RunnerId { get; set; }
    }
}
