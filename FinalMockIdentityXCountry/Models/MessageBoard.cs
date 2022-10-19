using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models
{
    public class MessageBoard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? MessageTitle { get; set; }
        [Required]
        public string? MessageBody { get; set; }
        public string? CoachId { get; set; }
        [ValidateNever]
        public virtual ApplicationUser? Coach { get; set; }
        [Required]
        public DateTime PublishedDateTime { get; set; }
        public string? CoachName { get; set; }

        //public int CoachId { get; set; }
        //public Coach Coach { get; set; }
    }
}
