using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class CurrentPracticeController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question
        
        public CurrentPracticeController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult CurrentPractices()
        {
            var userClaimsIdentity = (ClaimsIdentity)User.Identity;
            var userClaim = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim != null)
            {
                var userClaimValue = userClaim.Value;

                IEnumerable<Practice> practices = _context.Practices
                                    .Where(c => c.CoachId == userClaimValue)
                                    .Where(p => p.PracticeIsInProgress);

                if (practices.Count() == 0)
                {
                    // return a page stating there are no practices for the coach with that id?
                    return View("NoPracticesInProgress");
                } 

                return View(practices);
            }
                return View();
        }

        public IActionResult NoPracticesInProgress()
        {
            return View();
        }

        public IActionResult EditWorkouts()
        {
            return View();
        }

        public IActionResult Current(int currentPracticeId)
        {
            if (currentPracticeId != 0)
            {
                CurrentViewModel currentViewModel = new CurrentViewModel { practiceId = currentPracticeId };
               
                var dbQueries = (from a in _context.Attendances
                                 join aspnetusers in _context.ApplicationUsers
                                 on a.RunnerId equals aspnetusers.Id
                                 join practices in _context.Practices
                                 on a.PracticeId equals practices.Id
                                 where a.PracticeId == currentPracticeId && a.IsPresent && practices.PracticeIsInProgress
                                 select new
                                 {
                                     aspnetusers.FirstName,
                                     aspnetusers.LastName,
                                     practices.PracticeStartTimeAndDate,
                                     practices.PracticeLocation,
                                 });

                bool dbQueryFound = false;

                foreach (var dbQuery in dbQueries)
                {
                    dbQueryFound = true;
                    currentViewModel.PracticeLocation = dbQuery.PracticeLocation;
                    currentViewModel.PracticeStartTimeAndDate = dbQuery.PracticeStartTimeAndDate;
                    currentViewModel.RunnerName = $"{dbQuery.FirstName} {dbQuery.LastName}";
                    currentViewModel.Runners.Add(currentViewModel.RunnerName);
                }

                if (dbQueryFound)
                {
                    if (currentViewModel.Runners.Count() > 0)
                    {
                        return View(currentViewModel);
                    }
                    else
                    {
                        // empty runners list. may not need else statement
                    }
                }
                else
                {
                    return RedirectToAction(); // return a practice already ended page
                }
            }

            return RedirectToAction();
        }
    }
}
