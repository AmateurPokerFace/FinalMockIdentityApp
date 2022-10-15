using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes
{
    public class WorkoutInformationRepository : Repository<WorkoutInformation>, IWorkoutInformationRepository
    {
        private readonly XCountryDbContext _context;
        public WorkoutInformationRepository(XCountryDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(WorkoutInformation workoutInformation)
        {
            _context.Update(workoutInformation);
        }
    }
}
