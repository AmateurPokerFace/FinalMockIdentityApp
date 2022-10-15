using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;

namespace FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private XCountryDbContext _context;

        public UnitOfWork(XCountryDbContext context)
        {
            _context = context;
            Attendance = new AttendanceRepository(_context);
            Coach = new CoachRepository(_context);
            MessageBoard = new MessageBoardRepository(_context);
            MessageBoardResponses = new MessageBoardResponsesRepository(_context);
            Practice = new PracticeRepository(_context);
            Runner = new RunnerRepository(_context);
            WorkoutInformation = new WorkoutInformationRepository(_context);
            WorkoutType = new WorkoutTypeRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);
        }

        public IAttendanceRepository Attendance { get; private set; }

        public ICoachRepository Coach { get; private set; }

        public IMessageBoardRepository MessageBoard { get; private set; }

        public IMessageBoardResponsesRepository MessageBoardResponses { get; private set; }

        public IPracticeRepository Practice { get; private set; }

        public IRunnerRepository Runner { get; private set; }

        public IWorkoutInformationRepository WorkoutInformation { get; private set; }

        public IWorkoutTypeRepository WorkoutType { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; set; }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
