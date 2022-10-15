using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        private readonly XCountryDbContext _context;
        public AttendanceRepository(XCountryDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Attendance attendance)
        {
            _context.Update(attendance);
        }

    }
}
