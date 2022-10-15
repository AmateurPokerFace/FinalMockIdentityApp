using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes
{
    public class CoachRepository : Repository<Coach>, ICoachRepository
    {
        private readonly XCountryDbContext _context;
        public CoachRepository(XCountryDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Coach coach)
        {
            _context.Update(coach);
        }
    }
}
