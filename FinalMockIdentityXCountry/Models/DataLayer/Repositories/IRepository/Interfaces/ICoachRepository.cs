namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces
{
    public interface ICoachRepository : IRepository<Coach>
    {
        void Update(Coach coach);
    }
}
