using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.DataController;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.ExportDataController;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.PracticeHistoryController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.ImageEditor;
using System.Text;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class ExportDataController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question

        public ExportDataController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExportPracticeDataToCSV(int practiceId)
        {
            if (practiceId == 0)
            {
                TempData["error"] = "Invalid data provided";
                return RedirectToAction("Index");
            }

            var dbQueries = (from p in _context.Practices
                             join a in _context.Attendances
                             on p.Id equals a.PracticeId
                             join wi in _context.WorkoutInformation
                             on p.Id equals wi.PracticeId
                             join wt in _context.WorkoutTypes
                             on wi.WorkoutTypeId equals wt.Id
                             join aspnetusers in _context.ApplicationUsers
                             on wi.RunnerId equals aspnetusers.Id
                             where p.Id == practiceId && a.IsPresent && wi.RunnerId == a.RunnerId && p.PracticeIsInProgress == false
                             select new
                             {
                                 p.PracticeStartTimeAndDate,
                                 p.PracticeEndTimeAndDate,
                                 p.PracticeLocation,
                                 wt.WorkoutName,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName
                             });

            List<ExportPracticeDataToCSVViewModel> exportPracticeDataToCSVViewModels = new List<ExportPracticeDataToCSVViewModel>();

            if (dbQueries != null && dbQueries.Count() > 0)
            {
                foreach (var dbQuery in dbQueries)
                {
                    ExportPracticeDataToCSVViewModel exportVm = new ExportPracticeDataToCSVViewModel
                    {
                        RunnersName = $"{dbQuery.FirstName} {dbQuery.LastName}",
                        PracticeLocation = dbQuery.PracticeLocation,
                        WorkoutName = dbQuery.WorkoutName,
                        PracticeDate = DateOnly.FromDateTime(dbQuery.PracticeStartTimeAndDate),
                        StartTime = TimeOnly.FromDateTime(dbQuery.PracticeStartTimeAndDate),
                        EndTime = TimeOnly.FromDateTime(dbQuery.PracticeEndTimeAndDate)
                    };

                    exportPracticeDataToCSVViewModels.Add(exportVm);
                }

                var builder = new StringBuilder();
                builder.AppendLine("Runner,Date,Start Time,Ending Time,Location,Workout");

                foreach (var data in exportPracticeDataToCSVViewModels)
                {
                    builder.AppendLine($"{data.RunnersName},{data.PracticeDate},{data.StartTime},{data.EndTime},{data.PracticeLocation},{data.WorkoutName}");
                }
                 
                return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", $"practice_data_{practiceId}.csv");
            }

            TempData["error"] = "There was no query data found for the provided runners.";
            return RedirectToAction("Index");

        }

        [HttpPost]
        public IActionResult ExportRunnerPracticeDataToCSV(int practiceId, string runnerId) 
        {
            if (practiceId == 0 || runnerId == null)
            {
                TempData["error"] = "Invalid data provided";
                return RedirectToAction("Index");
            }

            var dbQueries = (from p in _context.Practices
                             join a in _context.Attendances
                             on p.Id equals a.PracticeId
                             join wi in _context.WorkoutInformation
                             on p.Id equals wi.PracticeId
                             join wt in _context.WorkoutTypes
                             on wi.WorkoutTypeId equals wt.Id
                             join aspnetusers in _context.ApplicationUsers
                             on wi.RunnerId equals aspnetusers.Id
                             where a.RunnerId == runnerId && p.Id == practiceId && a.IsPresent && wi.RunnerId == a.RunnerId && p.PracticeIsInProgress == false
                             select new
                             {
                                 p.PracticeStartTimeAndDate,
                                 p.PracticeEndTimeAndDate,
                                 p.PracticeLocation,
                                 wt.WorkoutName,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName
                             });

            List<ExportRunnerPracticeDataToCSVViewModel> exportRunnerPracticeDataToCSVViewModels = new List<ExportRunnerPracticeDataToCSVViewModel>();

            if (dbQueries != null && dbQueries.Count() > 0)
            {
                foreach (var dbQuery in dbQueries)
                {
                    ExportRunnerPracticeDataToCSVViewModel exportVm = new ExportRunnerPracticeDataToCSVViewModel()
                    {
                        RunnersName = $"{dbQuery.FirstName} {dbQuery.LastName}",
                        PracticeLocation = dbQuery.PracticeLocation,
                        WorkoutName = dbQuery.WorkoutName,
                        PracticeDate = DateOnly.FromDateTime(dbQuery.PracticeStartTimeAndDate),
                        StartTime = TimeOnly.FromDateTime(dbQuery.PracticeStartTimeAndDate),
                        EndTime = TimeOnly.FromDateTime(dbQuery.PracticeEndTimeAndDate)
                    };

                    exportRunnerPracticeDataToCSVViewModels.Add(exportVm);
                }

                var builder = new StringBuilder();
                builder.AppendLine("Runner,Date,Start Time,Ending Time,Location,Workout");

                foreach (var data in exportRunnerPracticeDataToCSVViewModels)
                {
                    builder.AppendLine($"{data.RunnersName},{data.PracticeDate},{data.StartTime},{data.EndTime},{data.PracticeLocation},{data.WorkoutName}");
                }
                 
                return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", $"runner_practice_data_{practiceId}.csv");
            }

            TempData["error"] = "There was no query data found for the provided runner.";
            return RedirectToAction("Index");
            
        }

        [HttpPost]
        public IActionResult ExportAthletePracticeHistoryToCSV(string runnerId)
        {
            if (runnerId == null)
            {
                TempData["error"] = "Invalid runner id provided";
                return RedirectToAction("Index");
            }

            var dbQueries = (from a in _context.Attendances
                             join p in _context.Practices
                             on a.PracticeId equals p.Id
                             join aspnetusers in _context.ApplicationUsers
                             on a.RunnerId equals aspnetusers.Id
                             where a.RunnerId == runnerId && a.IsPresent
                             select new
                             {
                                 p.PracticeLocation,
                                 p.PracticeStartTimeAndDate,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                             });

            if (dbQueries != null && dbQueries.Count() > 0)
            {
                List<ExportAthletePracticeHistoryToCSVViewModel> exportAthletePracticeHistoryToCSVViewModels = new List<ExportAthletePracticeHistoryToCSVViewModel>();

                string outputRunnerName = "";
                try
                {
                    outputRunnerName = $"{dbQueries.FirstOrDefault().FirstName}{dbQueries.FirstOrDefault().LastName}";
                }
                catch (ArgumentNullException)
                {

                    outputRunnerName = "unknown";
                }
                
                foreach (var dbQuery in dbQueries)
                {
                    ExportAthletePracticeHistoryToCSVViewModel exportVm = new ExportAthletePracticeHistoryToCSVViewModel
                    {
                        RunnersName = $"{dbQuery.FirstName} {dbQuery.LastName}",
                        PracticeDate = DateOnly.FromDateTime(dbQuery.PracticeStartTimeAndDate),
                        PracticeStartTime = TimeOnly.FromDateTime(dbQuery.PracticeStartTimeAndDate),
                        PracticeLocation = dbQuery.PracticeLocation
                    };

                    exportAthletePracticeHistoryToCSVViewModels.Add(exportVm);
                }

                var builder = new StringBuilder();
                builder.AppendLine("Runner,Date,Start Time,Location");

                if (exportAthletePracticeHistoryToCSVViewModels != null && exportAthletePracticeHistoryToCSVViewModels.Count > 0)
                {
                    
                    exportAthletePracticeHistoryToCSVViewModels = exportAthletePracticeHistoryToCSVViewModels.OrderBy(i => i.PracticeLocation).ToList();

                    foreach (var data in exportAthletePracticeHistoryToCSVViewModels)
                    {
                        builder.AppendLine($"{data.RunnersName},{data.PracticeDate},{data.PracticeStartTime},{data.PracticeLocation}");
                    }

                    return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", $"{outputRunnerName}_practice_history.csv");
                }
                
            }

            TempData["error"] = "There was no data found with the provided query.";
            return View("Index");
        }

        [HttpPost]
        public IActionResult ExportAllPracticeHistoryToCSV()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExportCurrentPracticeHistoryToCSV(int practiceId) 
        {
            var dbQueries = (from p in _context.Practices
                             join a in _context.Attendances
                             on p.Id equals a.PracticeId
                             where a.IsPresent && p.PracticeIsInProgress == false
                             group a by new
                             {
                                 a.PracticeId,
                                 p.PracticeLocation,
                                 p.PracticeStartTimeAndDate
                             } into matchesFound
                             select new CurrentPracticeWorkoutDataViewModel
                             {
                                 PracticeId = matchesFound.Key.PracticeId,
                                 PracticeDateTime = matchesFound.Key.PracticeStartTimeAndDate,
                                 PracticeLocation = matchesFound.Key.PracticeLocation,
                                 TotalRunners = matchesFound.Count(),
                             });
            return View();
        }
    }
}
