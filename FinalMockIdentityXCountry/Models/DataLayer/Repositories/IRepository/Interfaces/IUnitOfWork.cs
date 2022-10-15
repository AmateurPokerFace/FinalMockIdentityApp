namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces
{
    public interface IUnitOfWork
    {
        IAttendanceRepository Attendance { get; }
        ICoachRepository Coach { get; } 
        IMessageBoardRepository MessageBoard { get; }
        IMessageBoardResponsesRepository MessageBoardResponses { get; }
        IPracticeRepository Practice { get; }
        IRunnerRepository Runner { get; }
        IWorkoutInformationRepository WorkoutInformation { get; }
        IWorkoutTypeRepository WorkoutType { get; }
        IApplicationUserRepository ApplicationUser { get; }
        public void SaveChanges();
    }
}
