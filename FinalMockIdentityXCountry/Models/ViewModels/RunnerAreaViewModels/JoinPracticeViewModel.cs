using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels
{
    public class JoinPracticeViewModel
    {
        [Required]
        public int PracticeId { get; set; }

        [ValidateNever]
        public DateTime PracticeStartTimeAndDate { get; set; }

        [ValidateNever]
        public string? PracticeLocation { get; set; }
        
        [Required]
        public string RunnerId { get; set; }
    }
}
