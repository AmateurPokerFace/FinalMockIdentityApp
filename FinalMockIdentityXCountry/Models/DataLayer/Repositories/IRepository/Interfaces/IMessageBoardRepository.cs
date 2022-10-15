using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces
{
    public interface IMessageBoardRepository : IRepository<MessageBoard>
    {
        void Update(MessageBoard messageBoard);
    }
}
