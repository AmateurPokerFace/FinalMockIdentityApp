using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models
{
    public class MessageBoardResponse
    {
        [Key]
        public int Id { get; set; }

        public string? RunnerResponse { get; set; }
        public string? CoachResponse { get; set; }
        public string RunnerId { get; set; }
        [ValidateNever]
        public virtual ApplicationUser Runner { get; set; }
        public string CoachId { get; set; }
        [ValidateNever]
        public virtual ApplicationUser Coach { get; set; }

        //public int RunnerId { get; set; }
        //public Runner Runner { get; set; }

        //public int CoachId { get; set; }
        //public Coach Coach { get; set; }

        public int MessageBoardId { get; set; }
        public MessageBoard? MessageBoard { get; set; }
    }
}
