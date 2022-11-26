using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels;
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
            //var oneWeekFromInitialDate = DateOnly.FromDateTime(initialDate.AddDays(7));
            if (selectRangeForStatisticsViewModel == null)
            {
                TempData["error"] = "Invalid data provided";
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
                    dateFromSelectedFilter = selectRangeForStatisticsViewModel.InitialDate.Value.AddDays(7);
                }
                else if (selectRangeForStatisticsViewModel.SelectedQueryFilter.ToLower() == "two weeks")
                {
                    dateFromSelectedFilter = selectRangeForStatisticsViewModel.InitialDate.Value.AddDays(14);
                }
                else if (selectRangeForStatisticsViewModel.SelectedQueryFilter.ToLower() == "one month")
                {
                    dateFromSelectedFilter = selectRangeForStatisticsViewModel.InitialDate.Value.AddMonths(1);
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
                                     where wi.RunnerId == userClaimValue && DateOnly.FromDateTime( p.PracticeStartTimeAndDate) >= DateOnly.FromDateTime(selectRangeForStatisticsViewModel.InitialDate.Value.Date)
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
                        
                    }

                }
            }
            
            return View();
        } 
    }
}
