using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations; 
 
namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.StartPracticeController
{
    public class EditDataViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string? PracticeLocation { get; set; }
        [ValidateNever]
        public int PracticeId { get; set; }
    }
}
