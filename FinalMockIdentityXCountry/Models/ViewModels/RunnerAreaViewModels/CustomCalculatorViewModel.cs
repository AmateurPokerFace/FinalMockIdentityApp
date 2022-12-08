using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels
{
    public class CustomCalculatorViewModel
    {
        //int hours = 0, int minutes = 0, int seconds = 0, double distance = 0
        [Required]
        [Range(0, 12)]
        public int Hours { get; set; } = 0;

        [Required]
        [Range(0, 60)]
        public int Minutes { get; set; } = 0;
        
        [Required]
        [Range(0, 60)]
        public int Seconds { get; set; } = 0;
        
        [Required] 
        [Range(1, 100)]
        public double Distance { get; set; } = 0;
        
        [ValidateNever]
        [Display(Name = "Pace")]
        public string? PaceString { get; set; } = "";
    }
}
