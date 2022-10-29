using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalMockIdentityXCountry.Models
{
    public class XCountryDbContext : IdentityDbContext
    {
        public XCountryDbContext(DbContextOptions<XCountryDbContext> options) : base(options) { }

        public DbSet<Attendance> Attendances { get; set; }
        //public DbSet<Coach> Coaches { get; set; }
        public DbSet<MessageBoard> MessageBoards { get; set; }
        public DbSet<MessageBoardResponse> MessageBoardResponses { get; set; }
        public DbSet<Practice> Practices { get; set; }
        //public DbSet<Runner> Runners { get; set; }
        public DbSet<WorkoutInformation> WorkoutInformation { get; set; }
        public DbSet<WorkoutType> WorkoutTypes { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers{ get; set; }
        public DbSet<ReplyToMessageBoardResponse> RepliesToMessageBoardResponse { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Coach>().HasData(
            //    new Coach { FirstName = "Usain", LastName = "Bolt", Id = 1 },
            //    new Coach { FirstName = "Jesse", LastName = "Owens", Id = 2 },
            //    new Coach { FirstName = "Tom", LastName = "Doe", Id = 3 }
            //    );

            //modelBuilder.Entity<Runner>().HasData(
            //    new Runner { FirstName = "Tiger", LastName = "Woods", Id = 1 },
            //    new Runner { FirstName = "Tom", LastName = "Brady", Id = 2 },
            //    new Runner { FirstName = "Michael", LastName = "Jordan", Id = 3 },
            //    new Runner { FirstName = "Lebron", LastName = "James", Id = 4 },
            //    new Runner { FirstName = "Stephen", LastName = "Curry", Id = 5 }
            //    );

            modelBuilder.Entity<WorkoutType>().HasData(
                 new WorkoutType { WorkoutName = "N/A", Id = 1 },
                new WorkoutType { WorkoutName = "100-meters", Id = 2 },
                new WorkoutType { WorkoutName = "200-meters", Id = 3 },
                new WorkoutType { WorkoutName = "400-meters", Id = 4 },
                new WorkoutType { WorkoutName = "800-meters", Id = 5 },
                new WorkoutType { WorkoutName = "1600-meters", Id = 6 }
                );
        }
    }
}

