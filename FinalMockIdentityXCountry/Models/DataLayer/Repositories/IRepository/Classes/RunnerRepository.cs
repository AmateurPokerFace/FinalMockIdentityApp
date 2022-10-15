using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes
{
    public class RunnerRepository : Repository<Runner>, IRunnerRepository
    {
        private readonly XCountryDbContext _context;
        public RunnerRepository(XCountryDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Runner runner)
        {
            _context.Update(runner);
        }
    }
}
