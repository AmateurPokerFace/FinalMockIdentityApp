using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces
{
    public interface IMessageBoardResponsesRepository : IRepository<MessageBoardResponse>
    {
        void Update(MessageBoardResponse messageBoardResponse);
    }
}
