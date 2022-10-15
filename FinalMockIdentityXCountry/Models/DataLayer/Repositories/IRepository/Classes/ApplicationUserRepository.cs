using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private XCountryDbContext _context;
        public ApplicationUserRepository(XCountryDbContext context) : base(context)
        {
            _context = context;
        }


    }
}
