using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        void Update(Attendance attendance);
    }
}
