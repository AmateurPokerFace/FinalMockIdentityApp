using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.Delete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation.Context;
using NuGet.Versioning;
using System.Data;
using System.Security.Claims;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class PracticeHistoryController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question

        public PracticeHistoryController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult History()
        {
            List<PracticeHistoryViewModel> practiceHistoryViewModels = new List<PracticeHistoryViewModel>();
            PracticeHistoryViewModel practiceHistoryViewModel = new PracticeHistoryViewModel();

            var userClaimsIdentity = (ClaimsIdentity)User.Identity;
            var userClaim = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            int count = 0;

            if (userClaim != null)
            {
                var userClaimValue = userClaim.Value;
                var dbQueries = (from a in _context.Attendances
                                 join p in _context.Practices
                                 on a.PracticeId equals p.Id
                                 where a.IsPresent && p.CoachId == userClaimValue
                                 select new
                                 {
                                     a.AttendanceDate
                                 });
                                  
               //var testQuery = _context.Attendances.Select(z => new {z.PracticeId})
               //     .GroupBy(x => new {x.PracticeId})
               //     .Select(g => new Evaluation)
            }

            //    var dbQueries = (from a in _context.Attendances
            //                 join aspnetusers in _context.ApplicationUsers
            //                 on a.RunnerId equals aspnetusers.Id
            //                 join practices in _context.Practices
            //                 on a.PracticeId equals practices.Id
            //                 where a.PracticeId == currentPracticeId && a.IsPresent && practices.PracticeIsInProgress
            //                 select new
            //                 {
            //                     aspnetusers.FirstName,
            //                     aspnetusers.LastName,
            //                     practices.PracticeStartTimeAndDate,
            //                     practices.PracticeLocation,
            //                 });

            //bool dbQueryFound = false;

            //foreach (var dbQuery in dbQueries)
            //{
            //    dbQueryFound = true;
            //    currentViewModel.PracticeLocation = dbQuery.PracticeLocation;
            //    currentViewModel.PracticeStartTimeAndDate = dbQuery.PracticeStartTimeAndDate;
            //    currentViewModel.RunnerName = $"{dbQuery.FirstName} {dbQuery.LastName}";
            //    currentViewModel.Runners.Add(currentViewModel.RunnerName);
            //}

            //if (dbQueryFound)
            //{
            //    if (currentViewModel.Runners.Count() > 0)
            //    {
            //        return View(currentViewModel);
            //    }
            //    else
            //    {
            //        // empty runners list. may not need else statement
            //    }
            //}
            //else
            //{
            //    return RedirectToAction(); // return a practice already ended page
            //}

            return View(practiceHistoryViewModel);
        }
    }
}
