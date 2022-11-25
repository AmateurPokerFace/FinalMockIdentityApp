using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.Utilities;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.Delete;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.StatisticsController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class StatisticsController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StatisticsController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Line()
        {
            List<LineChartData> chartData = new List<LineChartData>
            {
                new LineChartData { xValue = new DateTime(2005, 01, 01), yValue = 21, yValue1 = 28 },
                new LineChartData { xValue = new DateTime(2006, 01, 01), yValue = 24, yValue1 = 44 },
                new LineChartData { xValue = new DateTime(2007, 01, 01), yValue = 36, yValue1 = 48 },
                new LineChartData { xValue = new DateTime(2008, 01, 01), yValue = 38, yValue1 = 50 },
                new LineChartData { xValue = new DateTime(2009, 01, 01), yValue = 54, yValue1 = 66 },
                new LineChartData { xValue = new DateTime(2010, 01, 01), yValue = 57, yValue1 = 78 },
                new LineChartData { xValue = new DateTime(2011, 01, 01), yValue = 70, yValue1 = 84 },
            };
            ViewBag.dataSource = chartData;
            return View();
        }

        public IActionResult SelectedRunner(string runnerId)
        {
            if (runnerId == null)
            {
                return RedirectToAction("Index");
            }

            ApplicationUser applicationUser = _context.ApplicationUsers.Find(runnerId);

            if (applicationUser == null)
            {
                return RedirectToAction("Index");
            }

            var dbQueries = (from wi in _context.WorkoutInformation
                             join wt in _context.WorkoutTypes
                             on wi.WorkoutTypeId equals wt.Id
                             join p in _context.Practices
                             on wi.PracticeId equals p.Id
                             where wi.RunnerId == runnerId && wi.DataWasLogged
                             select new 
                             {
                                wi.Distance,
                                wi.Hours,
                                wi.Minutes,
                                wi.Seconds,
                                wt.WorkoutName,
                                p.PracticeStartTimeAndDate
                             });

            if (dbQueries == null || dbQueries.Count() < 1)
            {
                return RedirectToAction("Index");
            }

            SelectedRunnerViewModel selectedRunnerViewModel = new SelectedRunnerViewModel { RunnerName = $"{applicationUser.FirstName} {applicationUser.LastName}" };
            
            foreach (var dbQuery in dbQueries)
            {
                SelectedRunnerViewModelHelper selectedRunnerViewModelHelper = new SelectedRunnerViewModelHelper
                {
                    Distance = dbQuery.Distance,
                    PracticeDate = DateOnly.FromDateTime(dbQuery.PracticeStartTimeAndDate),
                    WorkoutName = dbQuery.WorkoutName,
                    LoggedWorkoutTime = new TimeSpan(dbQuery.Hours,dbQuery.Minutes,dbQuery.Seconds),
                    Pace = StaticPaceCalculator.CalculatePace(dbQuery.Hours, dbQuery.Minutes, dbQuery.Seconds, dbQuery.Distance),
                };

                selectedRunnerViewModelHelper.PaceString = $"{selectedRunnerViewModelHelper.Pace.Item1}:";
                selectedRunnerViewModelHelper.PaceString += selectedRunnerViewModelHelper.Pace.Item2 < 10 ? $"0{selectedRunnerViewModelHelper.Pace.Item2}" : $"{selectedRunnerViewModelHelper.Pace.Item2}";
                selectedRunnerViewModel.SelectedRunnerViewModelHelpers?.Add(selectedRunnerViewModelHelper);
            }
             
            return View(selectedRunnerViewModel);
        }
    }
}
