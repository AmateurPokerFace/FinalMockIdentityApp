namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class ThreadCommentViewModel
    {
        public ThreadCommentViewModel()
        {
            RepliesToMessageBoardResponses = new List<ThreadCommentViewModelHelper>();
        }

        public int MessageBoardResponseId { get; set; }
        public string OriginalAuthorResponse { get; set; }
        public DateTime OriginalAuthorResponseDateTime { get; set; }
        public string OriginalAuthorName { get; set; }
        public List<ThreadCommentViewModelHelper> RepliesToMessageBoardResponses { get; set; }
    }
}
