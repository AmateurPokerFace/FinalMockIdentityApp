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

        [Required]
        public DateTime WorkoutDateTime { get; set; }

        [Required]
        public double Pace { get; set; }

        //public int RunnerId { get; set; }
        //public Runner Runner { get; set; }
        public string RunnerId { get; set; }
        [ValidateNever]
        public virtual ApplicationUser Runner { get; set; }
        public int WorkoutTypeId { get; set; }
        [ValidateNever]
        public WorkoutType WorkoutType { get; set; }
        public int PracticeId { get; set; }
        [ValidateNever]
        public Practice Practice { get; set; }

        //public string ApplicationUserId { get; set; }
        //public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
