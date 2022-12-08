using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.Utilities;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.Delete;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.StatisticsController;
using FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels.Helper;
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

        public IActionResult SelectRunner()
        {
            var runnersWithWorkoutHistory = (from w in _context.WorkoutInformation
                                             join aspnetusers in _context.ApplicationUsers
                                             on w.RunnerId equals aspnetusers.Id
                                             select new
                                             {
                                                 aspnetusers.Id,
                                                 aspnetusers.FirstName,
                                                 aspnetusers.LastName,
                                             });

            if (runnersWithWorkoutHistory != null && runnersWithWorkoutHistory.Count() > 0)
            {
                List<SelectRunnerViewModel> selectRunnerViewModels = new List<SelectRunnerViewModel>();
                
                foreach (var runnerWithWorkoutHistory in runnersWithWorkoutHistory)
                {
                    SelectRunnerViewModel viewModel = new SelectRunnerViewModel 
                    {
                        RunnersId = runnerWithWorkoutHistory.Id,
                        RunnersName = $"{runnerWithWorkoutHistory.FirstName} {runnerWithWorkoutHistory.LastName}"
                    };

                    if (viewModel != null)
                    {
                        selectRunnerViewModels.Add(viewModel);
                    }
                }

                selectRunnerViewModels = selectRunnerViewModels.DistinctBy(x => x.RunnersId).ToList();

                return View(selectRunnerViewModels);
            }

            TempData["error"] = "There are no runners in the database that have workout history.";
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Statistics(string runnerId)
        {
            if (runnerId == null)
            {
                TempData["error"] = "Invalid runner id provided.";
                return RedirectToAction(nameof(Index));
            }

            ApplicationUser applicationUser = _context.ApplicationUsers.Find(runnerId);
            
            if (applicationUser == null)
            {
                TempData["error"] = "Invalid user provided. The user doesn't exist";
                return RedirectToAction(nameof(Index));
            }
            var dbQueries = (from wi in _context.WorkoutInformation
                             join p in _context.Practices
                             on wi.PracticeId equals p.Id
                             join wt in _context.WorkoutTypes
                             on wi.WorkoutTypeId equals wt.Id
                             where wi.RunnerId == runnerId
                             select new
                             {
                                 p.PracticeStartTimeAndDate,
                                 p.PracticeLocation,
                                 wi.Distance,
                                 wi.Hours,
                                 wi.Minutes,
                                 wi.Seconds,
                                 wt.WorkoutName
                             });

            

            if (dbQueries != null && dbQueries.Count() > 0)
            {
                List<WorkoutStatisticsViewModel> workoutStatisticsViewModels = new List<WorkoutStatisticsViewModel>();
                WorkoutStatisticsViewModel model = new WorkoutStatisticsViewModel();

                foreach (var dbQuery in dbQueries.OrderByDescending(p => p.PracticeStartTimeAndDate))
                {
                    WorkoutStatisticsViewModelHelper modelHelper = new WorkoutStatisticsViewModelHelper
                    {
                        Distance = dbQuery.Distance,
                        Workout = dbQuery.WorkoutName,
                        Pace = StaticPaceCalculator.CalculatePace(dbQuery.Hours, dbQuery.Minutes, dbQuery.Seconds, dbQuery.Distance),
                        TimeDisplayString = new TimeSpan(dbQuery.Hours, dbQuery.Minutes, dbQuery.Seconds).ToString(),
                        PracticeDate = dbQuery.PracticeStartTimeAndDate,
                        PracticeLocation = dbQuery.PracticeLocation,
                        RunnersName = $"{applicationUser.FirstName} {applicationUser.LastName}",
                    };

                    if (modelHelper != null)
                    {
                        TimeSpan testTimeSpan = TimeSpan.FromMinutes(modelHelper.Pace.Item1);
                        testTimeSpan += TimeSpan.FromSeconds(modelHelper.Pace.Item2);

                        modelHelper.PaceDisplayString = testTimeSpan.ToString();

                        model.WorkoutStatisticsViewModelHelpers?.Add(modelHelper);
                    }

                }

                TimeSpan fastestPaceTimeSpan = TimeSpan.Parse("00:00:00");
                TimeSpan ts;
                double longestDistance = 0;
                List<double> recordedDistances = new List<double>();
                model.AveragePace = TimeSpan.Parse("00:00:00");

                List<TimeSpan> paceTimeSpanList = new List<TimeSpan>(); // will be used to compute the average pace time 

                if (model.WorkoutStatisticsViewModelHelpers != null && model.WorkoutStatisticsViewModelHelpers.Count > 0)
                {
                    foreach (var vmHelper in model.WorkoutStatisticsViewModelHelpers)
                    {
                        if (vmHelper.PaceDisplayString != null)
                        {
                            ts = TimeSpan.Parse(vmHelper.PaceDisplayString);
                            paceTimeSpanList.Add(ts);
                            recordedDistances.Add(vmHelper.Distance);
                            ts = TimeSpan.Parse(vmHelper.PaceDisplayString);
                        }
                    }

                    if (paceTimeSpanList != null && paceTimeSpanList.Count > 0)
                    {
                        double doubleAverageTicks = paceTimeSpanList.Average(TimeSpan => TimeSpan.Ticks);
                        long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
                        model.AveragePace = new TimeSpan(longAverageTicks);

                        model.AveragePace = RoundTimeSpanSeconds.RoundSeconds(model.AveragePace);

                        fastestPaceTimeSpan = paceTimeSpanList.Where(x => x.TotalSeconds > 0).Min();
                        model.FastestPace = RoundTimeSpanSeconds.RoundSeconds(fastestPaceTimeSpan);
                    }
                    else
                    {
                        model.FastestPace = TimeSpan.Parse("00:00:00");
                    }

                    if (recordedDistances != null && recordedDistances.Count > 0)
                    {
                        model.LongestDistance = recordedDistances.Max();
                    }
                    else
                    {
                        model.LongestDistance = longestDistance;
                    }
                    return View(model);
                }

                else
                {
                    TempData["error"] = "The database search returned no results.";
                    return RedirectToAction("Index");
                }
            
            }

            TempData["error"] = "There was no data found with the provided query";
            return RedirectToAction(nameof(Index));

        }
        
    }
}
