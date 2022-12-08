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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CurrentPractices()
        {
            var userClaimsIdentity = (ClaimsIdentity?)User.Identity;
            var userClaim = userClaimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            CurrentPracticesViewModel currentPracticesViewModel = new CurrentPracticesViewModel();
             
            if (userClaim != null)
            {  
                List<int> currentAttendingPracticeIds= new List<int>(); // practices where the current user is marked present
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

                if (dbQueries != null && dbQueries.Count() > 0)
                {
                    foreach (var dbQuery in dbQueries)
                    {
                        CurrentPracticesViewModelHelper currentViewModel = new CurrentPracticesViewModelHelper();

                        currentViewModel.PracticeId = dbQuery.Id;
                        currentViewModel.PracticeLocation = dbQuery.PracticeLocation;
                        currentViewModel.PracticeStartTimeAndDate = dbQuery.PracticeStartTimeAndDate;
                        currentViewModel.RunnerId = dbQuery.RunnerId;
                        currentViewModel.IsPresent = dbQuery.IsPresent;
                        currentViewModel.HasBeenSignedOut = dbQuery.HasBeenSignedOut;

                        currentPracticesViewModel.CurrentPracticesViewModelsHelper?.Add(currentViewModel);
                        
                        if (currentViewModel != null)
                        {
                            currentAttendingPracticeIds.Add(currentViewModel.PracticeId);
                        }
                    };
                     
                }

                if (currentAttendingPracticeIds != null && currentAttendingPracticeIds.Count > 0)
                {
                    var distinctPractices = _context.Practices.Where(x => currentAttendingPracticeIds.Contains(x.Id) == false 
                                                                   && x.PracticeIsInProgress).ToList();

                    if (distinctPractices != null && distinctPractices.Count > 0)
                    {
                        foreach (var distinctPractice in distinctPractices)
                        {
                            CurrentPracticesViewModelHelper currentViewModel = new CurrentPracticesViewModelHelper
                            {
                                IsPresent = false,
                                HasBeenSignedOut = false,
                                PracticeId = distinctPractice.Id,
                                PracticeLocation = distinctPractice.PracticeLocation,
                                PracticeStartTimeAndDate = distinctPractice.PracticeStartTimeAndDate,
                                RunnerId = userClaim.Value
                            };

                            if (currentViewModel != null)
                            {
                                currentPracticesViewModel.CurrentPracticesViewModelsHelper?.Add(currentViewModel);
                            }
                        }
                        
                    }
                }

                if (currentPracticesViewModel.CurrentPracticesViewModelsHelper != null 
                    && currentPracticesViewModel.CurrentPracticesViewModelsHelper.Count > 0)
                {
                    currentPracticesViewModel.CurrentPracticesViewModelsHelper = currentPracticesViewModel.CurrentPracticesViewModelsHelper.OrderByDescending(x => x.PracticeStartTimeAndDate).ToList();
                    return View(currentPracticesViewModel);
                }

                TempData["error"] = "There are no current practices available";
                return RedirectToAction("Index","Home", new {area = "Welcome"});
            }

            TempData["error"] = "Invalid user";
            return RedirectToAction("Index", "Home", new { area = "Welcome" });
        }

        public IActionResult JoinPractice(string runnerId, int practiceId)
        {
            var userClaimsIdentity = (ClaimsIdentity?)User.Identity;
            var userClaim = userClaimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim != null)
            {
                var dbQuery = (from a in _context.Attendances
                               join practices in _context.Practices
                               on a.PracticeId equals practiceId
                               where practices.PracticeIsInProgress && a.RunnerId == userClaim.Value && a.IsPresent == false && practices.Id == practiceId
                               select new
                               {
                                   practices.Id,
                                   practices.PracticeStartTimeAndDate,
                                   practices.PracticeLocation,
                                   a.RunnerId
                               }).FirstOrDefault();
                
                if (dbQuery != null)
                {
                    JoinPracticeViewModel joinPracticeViewModel = new JoinPracticeViewModel
                    {
                        PracticeId = dbQuery.Id,
                        PracticeStartTimeAndDate = dbQuery.PracticeStartTimeAndDate,
                        PracticeLocation = dbQuery.PracticeLocation,
                        RunnerId = runnerId
                    };

                    return View(joinPracticeViewModel);
                }

                //if dbQuery is null, check to see if a practice exists
                Practice practice = _context.Practices.Where(x => x.PracticeIsInProgress && x.Id == practiceId).FirstOrDefault();
                
                if (practice == null)
                {
                    TempData["error"] = "Invalid practice provided. No results were found";
                    return RedirectToAction("Index", "Home", new { area = "Welcome" });
                }

                JoinPracticeViewModel joinPacticeViewModel = new JoinPracticeViewModel
                {
                    PracticeId = practice.Id,
                    PracticeLocation = practice.PracticeLocation,
                    PracticeStartTimeAndDate = practice.PracticeStartTimeAndDate,
                    RunnerId = userClaim.Value
                };

                return View(joinPacticeViewModel);
            }

            TempData["error"] = "Invalid user";
            return RedirectToAction("Index", "Home", new { area = "Welcome" });
        }

        [HttpPost]
        public IActionResult JoinPractice(JoinPracticeViewModel joinPracticeViewModel)
        {
            if (joinPracticeViewModel.PracticeId == 0)
            {
                TempData["error"] = "Invalid practice id provided";
                return RedirectToAction("Index", "Home", new { area = "Welcome" });
            }

            if (ModelState.IsValid)
            {
                var userClaimsIdentity = (ClaimsIdentity?)User.Identity;
                var userClaim = userClaimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

                if (userClaim != null)
                {
                    Attendance attendance = _context.Attendances
                    .Where(a => a.PracticeId == joinPracticeViewModel.PracticeId && a.RunnerId == userClaim.Value).FirstOrDefault();

                    if (attendance != null)
                    {
                        attendance.IsPresent = true;
                        _context.Attendances.Update(attendance);
                        _context.SaveChanges();
                        TempData["success"] = "You were added to the practice successfully";
                        return RedirectToAction(nameof(CurrentPractices));
                    }
                    else
                    {
                        Practice practice = _context.Practices.Where(x => x.PracticeIsInProgress && x.Id == joinPracticeViewModel.PracticeId).FirstOrDefault();

                        if (practice == null)
                        {
                            TempData["error"] = "Invalid practice id provided";
                            return RedirectToAction("Index", "Home", new { area = "Welcome" });
                        }
                        else
                        {
                            Attendance newAttendanceRecord = new Attendance
                            {
                                HasBeenSignedOut = false,
                                IsPresent= true,
                                PracticeId = practice.Id,
                                RunnerId= userClaim.Value,
                            };

                            if (newAttendanceRecord != null)
                            {
                                _context.Attendances.Add(newAttendanceRecord);
                                _context.SaveChanges();
                                TempData["success"] = "You were added to the practice successfully";
                                return RedirectToAction(nameof(CurrentPractices));
                            }
                        }
                    }
                }

                TempData["error"] = "Invalid user provided";
                return RedirectToAction("Index", "Home", new { area = "Welcome" });
            }

            TempData["error"] = "Invalid data provided";
            return RedirectToAction("Index", "Home", new { area = "Welcome" });
        }

        public IActionResult SignOutOfPractice(string runnerId, int practiceId)
        {
            var dbQuery = (from a in _context.Attendances
                           join practices in _context.Practices
                           on a.PracticeId equals practiceId
                           where practices.PracticeIsInProgress && a.RunnerId == runnerId && a.IsPresent == true && practices.Id == practiceId
                           select new
                           {
                               practices.Id,
                               practices.PracticeStartTimeAndDate,
                               practices.PracticeLocation,
                               a.RunnerId
                           }).FirstOrDefault();

            if (dbQuery == null)
            {
                TempData["error"] = "Invalid data provided. There is no practice with the provided id available for you to sign out of.";
                return RedirectToAction("Index", "Home", new { area = "Welcome" });
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
            if (signOutOfPracticeViewModel.PracticeId == 0 || signOutOfPracticeViewModel.RunnerId == null)
            {
                TempData["error"] = "Invalid data provided. The provided ID(s) are invalid";
                return RedirectToAction("Index", "Home", new { area = "Welcome" });
            }

            if (ModelState.IsValid)
            {
                var userClaimsIdentity = (ClaimsIdentity?)User.Identity;
                var userClaim = userClaimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

                if (userClaim != null) 
                {
                    Attendance attendance = _context.Attendances
                    .Where(a => a.PracticeId == signOutOfPracticeViewModel.PracticeId && a.RunnerId == userClaim.Value).FirstOrDefault();

                    if (attendance == null)
                    {
                        TempData["error"] = "Invalid data provided. No attendance exists for you with the provided data.";
                        return RedirectToAction(nameof(CurrentPractices));
                    }

                    attendance.HasBeenSignedOut = true;
                    _context.Attendances.Update(attendance);
                    _context.SaveChanges();

                    TempData["success"] = "You signed out of the practice successfully";
                    return RedirectToAction(nameof(CurrentPractices));
                }

                TempData["error"] = "Invalid user";
                return RedirectToAction("Index", "Home", new { area = "Welcome" });

            }

            TempData["error"] = "Invalid data provided. The sign out of practice attempt was unsuccessful";
            return RedirectToAction(nameof(CurrentPractices));
        }
    }
}
