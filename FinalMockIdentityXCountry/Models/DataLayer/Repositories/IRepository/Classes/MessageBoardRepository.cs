using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes
{
    public class MessageBoardRepository : Repository<MessageBoard>, IMessageBoardRepository
    {
        private readonly XCountryDbContext _context;
        public MessageBoardRepository(XCountryDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(MessageBoard messageBoard)
        {
            _context.Update(messageBoard);
        }
    }
}
