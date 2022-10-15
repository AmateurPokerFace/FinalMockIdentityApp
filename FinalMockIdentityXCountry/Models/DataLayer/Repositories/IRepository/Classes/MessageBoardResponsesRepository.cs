using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes
{
    public class MessageBoardResponsesRepository : Repository<MessageBoardResponse>, IMessageBoardResponsesRepository
    {
        private readonly XCountryDbContext _context;
        public MessageBoardResponsesRepository(XCountryDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(MessageBoardResponse messageBoardResponses)
        {
            _context.Update(messageBoardResponses);
        }
    }
}
