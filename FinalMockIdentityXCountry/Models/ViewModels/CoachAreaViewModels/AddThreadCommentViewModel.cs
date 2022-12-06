
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class AddThreadCommentViewModel
    {
        [ValidateNever]
        public DateTime MessageBoardPublishedDateTime { get; set; }
        [ValidateNever]
        public string MessageBoardMessageTitle { get; set; }
        [ValidateNever]
        public string MessageBoardMessageBody { get; set; }
        [Required]
        public int MessageBoardId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")]
        public string NewNessageBoardCommentResponse { get; set; }
        [Required]
        public string NewMessageBoardCommentResponderId { get; set; }
    }
}
