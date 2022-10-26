using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace FinalMockIdentityXCountry.Areas.Runner.Controllers
{
    [Authorize(Roles = "Runner")]
    [Area("Runner")]
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

            CurrentPracticesViewModel currentPracticesViewModel = new CurrentPracticesViewModel();
             
            if (userClaim != null)
            {  
                var dbQueries = (from a in _context.Attendances
                                 //join aspnetusers in _context.ApplicationUsers
                                 //on userClaimValue equals aspnetusers.Id
                                 join practices in _context.Practices
                                 on a.PracticeId equals practices.Id
                                 where practices.PracticeIsInProgress && a.RunnerId == userClaim.Value
                                 select new
                                 { 
                                     practices.Id,
                                     practices.PracticeStartTimeAndDate,
                                     practices.PracticeLocation,
                                     a.IsPresent,
                                     a.RunnerId,
                                     a.HasBeenSignedOut
                                 }); 

                foreach (var dbQuery in dbQueries)
                {
                    CurrentPracticesViewModelHelper currentViewModel = new CurrentPracticesViewModelHelper();

                    currentViewModel.PracticeId = dbQuery.Id;
                    currentViewModel.PracticeLocation = dbQuery.PracticeLocation;
                    currentViewModel.PracticeStartTimeAndDate = dbQuery.PracticeStartTimeAndDate;
                    currentViewModel.RunnerId = dbQuery.RunnerId;
                    currentViewModel.IsPresent = dbQuery.IsPresent;
                    currentViewModel.HasBeenSignedOut = dbQuery.HasBeenSignedOut;

                    currentPracticesViewModel.CurrentPracticesViewModelsHelper.Add(currentViewModel);
                };
                 
                 
                if (currentPracticesViewModel.CurrentPracticesViewModelsHelper?.Count > 0)
                {
                    return View(currentPracticesViewModel);
                }

                return RedirectToAction(); // return a page stating that the runner didn't exist during the creation of the practice. Coach must manually add the runner??
            }

            return View(); // return an error page
        }

        public IActionResult JoinPractice(string runnerId, int practiceId)
        {
             
            var dbQuery = (from a in _context.Attendances
                             join practices in _context.Practices
                             on a.PracticeId equals practiceId
                             where practices.PracticeIsInProgress && a.RunnerId == runnerId && a.IsPresent == false
                             select new
                             {
                                 practices.Id,
                                 practices.PracticeStartTimeAndDate,
                                 practices.PracticeLocation,
                                 a.RunnerId
                             }).FirstOrDefault();
            if (dbQuery == null)
            {
                return RedirectToAction(); // send to an invalid parameters provided page
            }

            JoinPracticeViewModel joinPracticeViewModel = new JoinPracticeViewModel 
            {
                PracticeId = dbQuery.Id,
                PracticeStartTimeAndDate = dbQuery.PracticeStartTimeAndDate,    
                PracticeLocation = dbQuery.PracticeLocation,
                RunnerId = runnerId
            };
            
            return View(joinPracticeViewModel);
        }

        [HttpPost]
        public IActionResult JoinPractice(JoinPracticeViewModel joinPracticeViewModel)
        {
            if (joinPracticeViewModel.PracticeId == null || joinPracticeViewModel.RunnerId == null)
            {
                return RedirectToAction(); // send to error page in the future
            }

            Attendance attendance = _context.Attendances
                .Where(a => a.PracticeId == joinPracticeViewModel.PracticeId && a.RunnerId == joinPracticeViewModel.RunnerId).FirstOrDefault();

            if (attendance == null)
            {
                return RedirectToAction(); // send to error page in the future
            }

            attendance.IsPresent = true;
            _context.Attendances.Update(attendance);
            _context.SaveChanges();

            return RedirectToAction(nameof(CurrentPractices));
        }

        public IActionResult SignOutOfPractice(string runnerId, int practiceId)
        {
            var dbQuery = (from a in _context.Attendances
                           join practices in _context.Practices
                           on a.PracticeId equals practiceId
                           where practices.PracticeIsInProgress && a.RunnerId == runnerId && a.IsPresent == true
                           select new
                           {
                               practices.Id,
                               practices.PracticeStartTimeAndDate,
                               practices.PracticeLocation,
                               a.RunnerId
                           }).FirstOrDefault();

            if (dbQuery == null)
            {
                return RedirectToAction(); // send to an invalid parameters provided page
            }

            SignOutOfPracticeViewModel signOutOfPracticeViewModel = new SignOutOfPracticeViewModel
            {
                PracticeId = dbQuery.Id,
                PracticeStartTimeAndDate = dbQuery.PracticeStartTimeAndDate,
                PracticeLocation = dbQuery.PracticeLocation,
                RunnerId = runnerId
            };

            return View(signOutOfPracticeViewModel);
        }

        [HttpPost]
        public IActionResult SignOutOfPractice(SignOutOfPracticeViewModel signOutOfPracticeViewModel)
        {
            if (signOutOfPracticeViewModel.PracticeId == null || signOutOfPracticeViewModel.RunnerId == null)
            {
                return RedirectToAction(); // send to error page in the future
            }

            Attendance attendance = _context.Attendances
                .Where(a => a.PracticeId == signOutOfPracticeViewModel.PracticeId && a.RunnerId == signOutOfPracticeViewModel.RunnerId).FirstOrDefault();

            if (attendance == null)
            {
                return RedirectToAction(); // send to error page in the future
            }

            attendance.HasBeenSignedOut = true;
            _context.Attendances.Update(attendance);
            _context.SaveChanges();

            return RedirectToAction(nameof(CurrentPractices));
        }
    }
}
