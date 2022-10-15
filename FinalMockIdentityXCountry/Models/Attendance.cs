using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalMockIdentityXCountry.Models
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }
        public bool HasBeenSignedOut { get; set; } 
        public int PracticeId { get; set; }
        [ValidateNever]
        public Practice Practice { get; set; }
        public string RunnerId { get; set; }
        [ValidateNever]
        public virtual ApplicationUser Runner { get; set; }
        //public string CoachId { get; set; }
        //[ValidateNever]
        //public virtual ApplicationUser Coach { get; set; }

    }
}
