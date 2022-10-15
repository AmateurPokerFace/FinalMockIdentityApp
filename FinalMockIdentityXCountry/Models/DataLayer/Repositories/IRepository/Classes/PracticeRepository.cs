using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes
{
    public class PracticeRepository : Repository<Practice>, IPracticeRepository
    {
        private readonly XCountryDbContext _context;
        public PracticeRepository(XCountryDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Practice practice)
        {
            _context.Update(practice);
        }
    }
}
