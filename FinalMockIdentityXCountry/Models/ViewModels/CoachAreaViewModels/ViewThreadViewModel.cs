using System.Security.Policy;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class ViewThreadViewModel
    {

        public MessageBoard? MessageBoard { get; set; } 
        public IEnumerable<MessageBoardResponse>? DatabaseMessageBoardResponses{ get; set; }
        // Create a RepliesToMessageBoardResponses table?Possible fields: Id, ReplierId, ReplierName, ReplyDateTime, MessageBoardResponseId -> one to many?
    }
}
