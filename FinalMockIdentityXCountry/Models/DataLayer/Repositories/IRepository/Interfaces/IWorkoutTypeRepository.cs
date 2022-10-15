namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces
{
    public interface IWorkoutTypeRepository : IRepository<WorkoutType>
    {
        void Update(WorkoutType workoutType);
    }
}
