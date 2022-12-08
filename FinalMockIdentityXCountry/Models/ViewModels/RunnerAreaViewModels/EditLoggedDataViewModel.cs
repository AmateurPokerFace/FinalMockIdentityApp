using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels
{
    public class EditLoggedDataViewModel 
    {
        [Required]
        public int WorkoutInformationId { get; set; }

        [Required]
        public int PracticeId { get; set; }

        [Required]
        public string RunnerId { get; set; }

        [ValidateNever]
        public DateTime PracticeStartDateTime { get; set; }

        [ValidateNever]
        public string? PracticeLocation { get; set; }

        [ValidateNever]
        public string? WorkoutName { get; set; }

        [Required]
        [Range(0, 15)]
        public int Hours { get; set; }

        [Required]
        [Range(0, 60)]
        public int Minutes { get; set; }

        [Required]
        [Range(0, 60)]
        public int Seconds { get; set; }

        [Required]
        [Range(0, 60)]
        public double Distance { get; set; }
    }
}
