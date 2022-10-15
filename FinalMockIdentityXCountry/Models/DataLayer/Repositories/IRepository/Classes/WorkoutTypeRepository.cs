using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes
{
    public class WorkoutTypeRepository : Repository<WorkoutType>, IWorkoutTypeRepository
    {
        private readonly XCountryDbContext _context;
        public WorkoutTypeRepository(XCountryDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(WorkoutType workoutType)
        {
            _context.Update(workoutType);
        }
    }
}
