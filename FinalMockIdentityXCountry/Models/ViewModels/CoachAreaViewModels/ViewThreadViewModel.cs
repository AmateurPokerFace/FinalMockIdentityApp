using System.Security.Policy;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class ViewThreadViewModel
    {

        public ViewThreadViewModel()
        {
            DatabaseMessageBoardResponses = new List<ViewThreadModelHelper>();
        }
        public DateTime MessageBoardPublishedDateTime { get; set; }
        public string MessageBoardTitle { get; set; }
        public string MessageBoardsAuthorName { get; set; }
        public string MessageBoardBody { get; set; }
        public int MessageBoardId { get; set; }
        public List<ViewThreadModelHelper> DatabaseMessageBoardResponses { get; set; }
    }
}
