using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models
{
    public class MessageBoardResponse
    {
        [Key]
        public int Id { get; set; }
        public int MessageBoardId { get; set; }
        [ValidateNever]
        public MessageBoard MessageBoard { get; set; }
        [Required]
        public string Response { get; set; }
        [Required]
        public string ResponderId { get; set; }
        [ValidateNever]
        public virtual ApplicationUser Responder { get; set; }
        public DateTime ResponseDateTime { get; set; } 
    }
}
