namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces
{
    public interface IWorkoutInformationRepository : IRepository<WorkoutInformation>
    {
        void Update(WorkoutInformation workoutInformation);
    }
}
