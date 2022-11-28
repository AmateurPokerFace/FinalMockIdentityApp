using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.ViewModelHelperClasses;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class AthleteHistoryController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question

        public AthleteHistoryController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult All()
        {
            var runnerUsers = _userManager.GetUsersInRoleAsync("Runner").Result;

            if (runnerUsers == null)
            {
                return RedirectToAction(); // send to an error page in the future (no runners in the database).
            }

            AllViewModel allViewModel = new AllViewModel();

            foreach (var runner in runnerUsers)
            { 
                allViewModel.RunnerUsers.Add((ApplicationUser)runner);
            }

            if (allViewModel.RunnerUsers.Count() < 1)
            {
                return RedirectToAction(); // send to an error page in the future
            }

            return View(allViewModel);
        }

        public IActionResult SelectedAthlete(string runnerId)
        {
            ApplicationUser applicationUser = _context.ApplicationUsers.Find(runnerId);
            
            if (applicationUser == null)
            {
                return RedirectToAction(); // send to an error page in the future
            }

            SelectedAthleteViewModel selectedAthleteViewModel = new SelectedAthleteViewModel();
            selectedAthleteViewModel.AthleteName = $"{applicationUser.FirstName} {applicationUser.LastName}";
            selectedAthleteViewModel.RunnerId = applicationUser.Id;

            var dbQueries = (from a in _context.Attendances
                             join p in _context.Practices
                             on a.PracticeId equals p.Id
                             where a.RunnerId == applicationUser.Id && a.IsPresent
                             select new
                             {
                                 p.PracticeLocation,
                                 p.PracticeStartTimeAndDate, 
                                 a.RunnerId,
                                 p.Id
                             });

            if (dbQueries == null)
            {
                return RedirectToAction(); // send to an error page in the future
            }

            foreach (var dbQuery in dbQueries)
            {
                SelectedAthleteViewModelHelper selectedAthleteViewModelHelper = new SelectedAthleteViewModelHelper
                {
                    PracticeLocation = dbQuery.PracticeLocation == null ? " " : dbQuery.PracticeLocation,
                    PracticeStartTimeAndDate = dbQuery.PracticeStartTimeAndDate,
                    RunnerId = dbQuery.RunnerId,
                    PracticeId = dbQuery.Id
                };

                if (selectedAthleteViewModelHelper != null)
                {
                    selectedAthleteViewModel.SelectedAthleteHelper.Add(selectedAthleteViewModelHelper);
                }
            }

            if (selectedAthleteViewModel.SelectedAthleteHelper != null && selectedAthleteViewModel.SelectedAthleteHelper.Count() > 0)
            {
                selectedAthleteViewModel.SelectedAthleteHelper = selectedAthleteViewModel.SelectedAthleteHelper.OrderByDescending(p => p.PracticeStartTimeAndDate).ToList();
            }

            return View(selectedAthleteViewModel);
        }
    }
}
