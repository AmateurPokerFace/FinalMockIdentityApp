

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class AddReplyToThreadCommentViewModel
    {
        [ValidateNever]
        public DateTime OriginalMessageBoardResponseDateTime { get; set; }
        public int OriginalMessageBoardResponseId { get; set; }
        [ValidateNever]
        public string OriginalMessageBoardResponseAuthorName { get; set; }
        [ValidateNever]
        public string OriginalMessageBoardResponse { get; set; }
        [Required(ErrorMessage ="This field is required")]
        public string NewReplyToMessageBoardResponse { get; set; }
        [Required]
        public string NewReplyerId { get; set; } 
    }
}
