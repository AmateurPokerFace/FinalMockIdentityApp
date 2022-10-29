using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models
{
    public class WorkoutType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? WorkoutName { get; set; }
    }
}
