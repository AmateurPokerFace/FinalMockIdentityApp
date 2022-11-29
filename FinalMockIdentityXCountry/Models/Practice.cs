using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalMockIdentityXCountry.Models
{
    public class Practice
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime PracticeStartTimeAndDate { get; set; }
        public DateTime PracticeEndTimeAndDate { get; set; }
        public bool PracticeIsInProgress { get; set; } // set the default to true in the future -> new practice == in progress until cancelled by coach
        [Required]
        public string? PracticeLocation { get; set; }
        public string CoachId { get; set; }
        [ValidateNever]
        public virtual ApplicationUser Coach { get; set; }
        public bool WorkoutsAddedToPractice { get; set; }
        public void StartPractice()
        {
            PracticeStartTimeAndDate = DateTime.Now;
        }


        public void EndPractice()
        {
            PracticeEndTimeAndDate = DateTime.Now;
        }
    }
}
