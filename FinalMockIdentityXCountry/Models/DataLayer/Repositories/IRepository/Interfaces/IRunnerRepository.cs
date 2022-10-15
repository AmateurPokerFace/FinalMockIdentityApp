namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces
{
    public interface IRunnerRepository : IRepository<Runner>
    {
        void Update(Runner runner);
    }
}
