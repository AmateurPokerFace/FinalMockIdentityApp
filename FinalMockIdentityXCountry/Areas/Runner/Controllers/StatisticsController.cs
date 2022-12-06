﻿using FinalMockIdentityXCountry.Migrations;
using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.Utilities;
using FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Security.Claims;

namespace FinalMockIdentityXCountry.Areas.Runner.Controllers
{
    [Authorize(Roles = "Runner")]
    [Area("Runner")]
    public class StatisticsController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question

        public StatisticsController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelectRangeForStatistics()
        {
            // query workoutinformation for this user.
            // If the count == 0: redirect to Index page. User has no statistical data.
            var userClaimsIdentity = (ClaimsIdentity)User.Identity;
            var userClaim = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            CurrentPracticesViewModel currentPracticesViewModel = new CurrentPracticesViewModel();

            if (userClaim != null)
            {
                WorkoutInformation workoutInformation = _context.WorkoutInformation.FirstOrDefault(r => r.RunnerId == userClaim.Value);
                if (workoutInformation == null)
                {
                    TempData["error"] = "You do not have any statistical workout data to query.";
                    return RedirectToAction("Index");
                }

                SelectRangeForStatisticsViewModel selectRangeForStatisticsViewModel = new SelectRangeForStatisticsViewModel { RunnerId = userClaim.Value};
                selectRangeForStatisticsViewModel.QueryFilters.Add(new SelectListItem { Text = "One Week", Value = "One Week" });
                selectRangeForStatisticsViewModel.QueryFilters.Add(new SelectListItem { Text = "Two Weeks", Value = "Two Weeks" });
                selectRangeForStatisticsViewModel.QueryFilters.Add(new SelectListItem { Text = "One Month", Value = "One Month" });
                selectRangeForStatisticsViewModel.QueryFilters.Add(new SelectListItem { Text = "All Time", Value = "All Time" });

                return View(selectRangeForStatisticsViewModel);
            }

            TempData["error"] = "User not found";
            return RedirectToAction("Index"); 
        }

        public IActionResult WorkoutStatistics(SelectRangeForStatisticsViewModel selectRangeForStatisticsViewModel)  
        {
            if (selectRangeForStatisticsViewModel == null)
            {
                TempData["error"] = "Invalid data provided";
                return RedirectToAction("SelectRangeForStatistics");
            }

            if (selectRangeForStatisticsViewModel.InitialDate == null && selectRangeForStatisticsViewModel.SelectedQueryFilter.ToLower() != "all time") 
            {
                TempData["error"] = "Select a date before attempting to query";
                return RedirectToAction("SelectRangeForStatistics");
            }

            if (selectRangeForStatisticsViewModel.SelectedQueryFilter != null)
            {
                DateTime dateFromSelectedFilter = DateTime.Now;
                bool dateRangeSelected = true;

                var userClaimsIdentity = (ClaimsIdentity)User.Identity;
                var userClaim = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (userClaim == null) 
                {
                    TempData["error"] = "User not found";
                    return RedirectToAction("Index");
                }

                string userClaimValue = userClaim.Value;

                if (selectRangeForStatisticsViewModel.SelectedQueryFilter.ToLower() == "one week")
                {
                    dateFromSelectedFilter = selectRangeForStatisticsViewModel.InitialDate.AddDays(7);
                }
                else if (selectRangeForStatisticsViewModel.SelectedQueryFilter.ToLower() == "two weeks")
                {
                    dateFromSelectedFilter = selectRangeForStatisticsViewModel.InitialDate.AddDays(14);
                }
                else if (selectRangeForStatisticsViewModel.SelectedQueryFilter.ToLower() == "one month")
                {
                    dateFromSelectedFilter = selectRangeForStatisticsViewModel.InitialDate.AddMonths(1);
                }
                else
                {
                    dateRangeSelected = false; // No specific range was selected. Will be used to query all data
                }

                if (dateRangeSelected)
                {

                    var dbQueries = (from wi in _context.WorkoutInformation
                                     join p in _context.Practices
                                     on wi.PracticeId equals p.Id
                                     join wt in _context.WorkoutTypes
                                     on wi.WorkoutTypeId equals wt.Id
                                     where wi.RunnerId == userClaimValue && DateOnly.FromDateTime(p.PracticeStartTimeAndDate) >= DateOnly.FromDateTime(selectRangeForStatisticsViewModel.InitialDate.Date)
                                     && DateOnly.FromDateTime(p.PracticeStartTimeAndDate) <= DateOnly.FromDateTime(dateFromSelectedFilter.Date)
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
                                PracticeLocation = dbQuery.PracticeLocation
                            };

                            if (modelHelper != null)
                            {

                                //modelHelper.PaceDisplayString = modelHelper.Pace.Item1 < 10
                                //                            ? $"00:0{modelHelper.Pace.Item1}:"
                                //                            : $"00:{modelHelper.Pace.Item1}:"; // this needs to be fixed to avoid ts error


                                TimeSpan testTimeSpan = TimeSpan.FromMinutes(modelHelper.Pace.Item1);
                                testTimeSpan += TimeSpan.FromSeconds(modelHelper.Pace.Item2);

                                modelHelper.PaceDisplayString = testTimeSpan.ToString();


                                                            //modelHelper.PaceDisplayString += modelHelper.Pace.Item2 < 10
                                                            //? $"0{modelHelper.Pace.Item2}"
                                                            //: $"{modelHelper.Pace.Item2}"; // this also needs to be fixed to avoid ts error

                                model.WorkoutStatisticsViewModelHelpers?.Add(modelHelper);
                            }
                            
                        }

                        TimeSpan fastestPaceTimeSpan = TimeSpan.Parse("00:00:00");
                        TimeSpan ts;
                        double longestDistance = 0;
                        model.AveragePace = TimeSpan.Parse("00:00:00");

                        List<TimeSpan> paceTimeSpanList = new List<TimeSpan>(); // will be used to compute the average pace time 

                        if (model.WorkoutStatisticsViewModelHelpers != null && model.WorkoutStatisticsViewModelHelpers.Count > 0)
                        {
                            foreach (var vmHelper in model.WorkoutStatisticsViewModelHelpers)
                            {
                                ts = TimeSpan.Parse(vmHelper.PaceDisplayString);
                                paceTimeSpanList.Add(ts);

                                if (ts.CompareTo(fastestPaceTimeSpan) == 1)
                                {
                                    fastestPaceTimeSpan = ts;
                                }
                                if (vmHelper.Distance > longestDistance)
                                {
                                    longestDistance = vmHelper.Distance;
                                }
                                ts = TimeSpan.Parse(vmHelper.PaceDisplayString);
                            }

                            if (model.WorkoutStatisticsViewModelHelpers != null && model.WorkoutStatisticsViewModelHelpers.Count > 0)
                            {
                                double doubleAverageTicks = paceTimeSpanList.Average(TimeSpan => TimeSpan.Ticks);
                                long longAverageTicks = Convert.ToInt64(doubleAverageTicks); 
                                model.AveragePace = new TimeSpan(longAverageTicks);
                                
                                model.AveragePace = RoundTimeSpanSeconds.RoundSeconds(model.AveragePace);
                            } 
                        }

                        model.FastestPace = RoundTimeSpanSeconds.RoundSeconds(fastestPaceTimeSpan);
                        model.LongestDistance = longestDistance;

                        return View(model);
                    }

                    
                }
                else
                {
                    var dbQueries = (from wi in _context.WorkoutInformation
                                     join p in _context.Practices
                                     on wi.PracticeId equals p.Id
                                     join wt in _context.WorkoutTypes
                                     on wi.WorkoutTypeId equals wt.Id
                                     where wi.RunnerId == userClaimValue 
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
                                PracticeLocation = dbQuery.PracticeLocation
                            };

                            if (modelHelper != null)
                            {
                                modelHelper.PaceDisplayString = modelHelper.Pace.Item1 < 10
                                                            ? $"00:0{modelHelper.Pace.Item1}:"
                                                            : $"00:{modelHelper.Pace.Item1}:";


                                modelHelper.PaceDisplayString += modelHelper.Pace.Item2 < 10
                                ? $"0{modelHelper.Pace.Item2}"
                                : $"{modelHelper.Pace.Item2}";

                                model.WorkoutStatisticsViewModelHelpers?.Add(modelHelper);
                            }

                        }

                        TimeSpan fastestPaceTimeSpan = TimeSpan.Parse("00:00:00");
                        TimeSpan ts;
                        double longestDistance = 0;
                        model.AveragePace = TimeSpan.Parse("00:00:00");

                        List<TimeSpan> paceTimeSpanList = new List<TimeSpan>(); // will be used to compute the average pace time 

                        if (model.WorkoutStatisticsViewModelHelpers != null && model.WorkoutStatisticsViewModelHelpers.Count > 0)
                        {
                            foreach (var vmHelper in model.WorkoutStatisticsViewModelHelpers)
                            {
                                ts = TimeSpan.Parse(vmHelper.PaceDisplayString);
                                paceTimeSpanList.Add(ts);

                                if (ts.CompareTo(fastestPaceTimeSpan) == 1)
                                {
                                    fastestPaceTimeSpan = ts;
                                }
                                if (vmHelper.Distance > longestDistance)
                                {
                                    longestDistance = vmHelper.Distance;
                                }
                                ts = TimeSpan.Parse(vmHelper.PaceDisplayString);
                            }

                            if (model.WorkoutStatisticsViewModelHelpers != null && model.WorkoutStatisticsViewModelHelpers.Count > 0)
                            {
                                double doubleAverageTicks = paceTimeSpanList.Average(TimeSpan => TimeSpan.Ticks);
                                long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
                                model.AveragePace = new TimeSpan(longAverageTicks);

                                model.AveragePace = RoundTimeSpanSeconds.RoundSeconds(model.AveragePace);
                            }
                        }

                        model.FastestPace = RoundTimeSpanSeconds.RoundSeconds(fastestPaceTimeSpan);
                        model.LongestDistance = longestDistance;

                        return View(model);
                    }

                }
            }

            TempData["error"] = "Invalid query filter provided";
            return RedirectToAction("Index");
        } 
    }
}
