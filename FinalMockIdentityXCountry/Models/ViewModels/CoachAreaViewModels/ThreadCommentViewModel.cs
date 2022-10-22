namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class ThreadCommentViewModel
    {
        public MessageBoardResponse? MessageBoardResponse { get; set; }
        public IEnumerable<ReplyToMessageBoardResponse>? RepliesToMessageBoardResponse { get; set; }
    }
}
