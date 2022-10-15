using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces
{
    public interface IPracticeRepository : IRepository<Practice>
    { 
        void Update(Practice practice);
    }
}
