
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models
{
    public class ReplyToMessageBoardResponse
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? ReplyerId { get; set; }
        [ValidateNever]
        public virtual ApplicationUser? Replyer { get; set; }
        public string? ReplyerName { get; set; }
        public DateTime ReplyDateTime { get; set; }
        [Required]
        public string? Reply { get; set; }
        public int MessageBoardResponseId { get; set; }
        [ValidateNever]
        public MessageBoardResponse? MessageBoardResponse { get; set; }
    }
}
