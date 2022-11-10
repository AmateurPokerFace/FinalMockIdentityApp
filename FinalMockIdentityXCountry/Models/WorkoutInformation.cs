using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models
{
    public class WorkoutInformation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double Distance { get; set; }
        //[Required]
        //public double Pace { get; set; }
        [Required]
        public int Hours { get; set; }
        [Required]
        [Range(0, 60.0)]
        public int Minutes { get; set; }
        [Required]
        [Range(0, 60.0)]
        public int Seconds { get; set; }
        public bool DataWasLogged { get; set; }
        public string? RunnerId { get; set; }
        [ValidateNever]
        public virtual ApplicationUser? Runner { get; set; }
        public int WorkoutTypeId { get; set; }
        [ValidateNever]
        public WorkoutType? WorkoutType { get; set; }
        public int PracticeId { get; set; }
        [ValidateNever]
        public Practice? Practice { get; set; }

    }
}
