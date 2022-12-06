using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.Utilities;
using FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.CurrentPracticeController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CurrentPractices()
        {
            IEnumerable<Practice> practices = _context.Practices.Where(p => p.PracticeIsInProgress);
            if (practices == null || practices.Count() == 0)
            {
               return View("NoPracticesInProgress");
            }

            practices = practices.OrderByDescending(x => x.PracticeStartTimeAndDate).ToList();

            return View(practices);
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
                Practice practice = _context.Practices.Find(currentPracticeId);
                if (practice == null) // check to see if a practice exists with the provided id in the database
                {
                    TempData["error"] = "The practice doesn't exist in the database. Please provide a valid practice id.";
                    return RedirectToAction("Index");
                }
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
                  
                if (dbQueries != null && dbQueries.Count() > 0)
                {
                    foreach (var dbQuery in dbQueries)
                    {
                        currentViewModel.PracticeLocation = dbQuery.PracticeLocation;
                        currentViewModel.PracticeStartTimeAndDate = dbQuery.PracticeStartTimeAndDate;
                        currentViewModel.RunnerName = $"{dbQuery.FirstName} {dbQuery.LastName}";
                        currentViewModel.Runners?.Add(currentViewModel.RunnerName);
                    }

                    return View(currentViewModel);
                }
                else
                {
                    currentViewModel.PracticeLocation = practice.PracticeLocation;
                    currentViewModel.PracticeStartTimeAndDate = practice.PracticeStartTimeAndDate; 
                    return View(currentViewModel);
                }
                
                
            }

            TempData["error"] = "Invalid practice id provided";
            return RedirectToAction("Index");
        }

        public IActionResult Attendance(int practiceId)
        {
            if (practiceId == 0)
            {
                TempData["error"] = "Invalid practice id provided";
                return RedirectToAction("Index"); 
            }
            Practice practice = _context.Practices.Find(practiceId);
           
            if (practice == null)
            {
                TempData["error"] = "Invalid practice id provided";
                return RedirectToAction("Index");
            }

            var dbQueries = (from a in _context.Attendances
                             join aspnetusers in _context.ApplicationUsers
                             on a.RunnerId equals aspnetusers.Id
                             join p in _context.Practices
                             on a.PracticeId equals p.Id
                             where a.IsPresent && a.HasBeenSignedOut == false && a.PracticeId == practiceId
                             select new
                             {
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                                 a.RunnerId,
                                 p.PracticeLocation,
                                 p.PracticeStartTimeAndDate,
                                 p.Id
                             });

            if (dbQueries.Count() < 1 || dbQueries == null)
            {
                if (practice == null)
                {
                    TempData["error"] = "Invalid practice provided";
                    return RedirectToAction("Index"); // send to an invalid page in the future
                }
                else
                {
                    return RedirectToAction("EmptyAttendance",new {practiceId = practiceId});
                }
                
            }

            AttendanceViewModel attendanceViewModel = new AttendanceViewModel
            {
                PracticeLocation = practice?.PracticeLocation == null ? " " : dbQueries.FirstOrDefault()?.PracticeLocation,
                PracticeStartTimeAndDate = practice.PracticeStartTimeAndDate,
                PracticeId = practice.Id
            };

            foreach (var dbQuery in dbQueries)
            {
                AttendanceViewModelHelper attendanceViewModelHelper = new AttendanceViewModelHelper
                {
                    PracticeId = dbQuery.Id,
                    RunnerId = dbQuery.RunnerId,
                    Runner = $"{dbQuery?.FirstName} {dbQuery?.LastName}"
                };

                attendanceViewModel.AttendanceViewModelHelpers?.Add(attendanceViewModelHelper);
            }

            if (attendanceViewModel.AttendanceViewModelHelpers != null && attendanceViewModel.AttendanceViewModelHelpers.Count() > 0)
            {
                return View(attendanceViewModel);
            }

            return RedirectToAction("Index"); // send to an error page in the future
        }

        public IActionResult EmptyAttendance(int practiceId)
        {
            Practice practice = _context.Practices.Where(p => p.PracticeIsInProgress && p.Id == practiceId).FirstOrDefault();
            if (practice == null)
            {
                TempData["error"] = "Invalid practice id provided";
                return RedirectToAction("Index");
            }

            EmptyAttendanceViewModel model = new EmptyAttendanceViewModel(practiceId);

            return View(model);
        }

        public IActionResult AddRunnerToCurrentPractice(int practiceId)
        {
            if (practiceId == 0)
            {
                TempData["error"] = "Invalid practice id provided";
                return RedirectToAction("CurrentPractices");
            }

            Practice practice = _context.Practices.Where(p => p.PracticeIsInProgress && p.Id == practiceId).FirstOrDefault(); 
             
            if (practice == null)
            {
                TempData["error"] = "Invalid practice id provided. Practice not found.";
                return RedirectToAction("CurrentPractices");
            }

            var runnerUsers = _userManager.GetUsersInRoleAsync("Runner").Result;
            List<ApplicationUser> users = new List<ApplicationUser>();

            if (runnerUsers == null || runnerUsers.Count < 1)
            {
                TempData["error"] = "There were no runners found in the database.";
                return RedirectToAction("Index", "Home", new { area = "Welcome" });
            }

            foreach (var runner in runnerUsers)
            {
                users.Add((ApplicationUser)runner);
            }

            AddRunnerToCurrentPracticeViewModel model = new AddRunnerToCurrentPracticeViewModel
            {
                PracticeLocation = practice.PracticeLocation,
                PracticeStartTimeAndDate = practice.PracticeStartTimeAndDate
            };
             
            //runners that are already logged as present
            var currentAttendanceRunnerIds = _context.Attendances.Where(a => a.IsPresent && a.PracticeId == practiceId).Select(x => x.RunnerId).ToList(); 

            if (currentAttendanceRunnerIds != null && currentAttendanceRunnerIds.Count > 0)
            {
               // var distinctRunners = model.SelectedCheckboxOptions?.Where(x => x.RunnerId.Any(a => attendanceRunnerIds.Contains(x.RunnerId)) == false);
                var distinctRunners = users.Where(x => x.Id.Any(a => currentAttendanceRunnerIds.Contains(x.Id)) == false);
                foreach (var distinctRunner in distinctRunners)
                {
                    AddRunnerToCurrentPracticeChkboxOptionViewModel chkboxOptionViewModel = new AddRunnerToCurrentPracticeChkboxOptionViewModel
                    {
                        RunnerId = distinctRunner.Id,
                        RunnersName = $"{distinctRunner.FirstName} {distinctRunner.LastName}",
                        PracticeId = practiceId
                    };

                    if (chkboxOptionViewModel != null)
                    {
                        model.SelectedCheckboxOptions?.Add(chkboxOptionViewModel);
                    }
                }
            }
            else
            {
                foreach (var user in users)
                {
                    AddRunnerToCurrentPracticeChkboxOptionViewModel chkboxOptionViewModel = new AddRunnerToCurrentPracticeChkboxOptionViewModel
                    {
                        RunnerId = user.Id,
                        RunnersName = $"{user.FirstName} {user.LastName}",
                        PracticeId = practiceId
                    };

                    if (chkboxOptionViewModel != null)
                    {
                        model.SelectedCheckboxOptions?.Add(chkboxOptionViewModel);
                    }
                }
            }

            if (model.SelectedCheckboxOptions != null && model.SelectedCheckboxOptions.Count > 0)
            {
                return View(model);
            }

            TempData["warning"] = "No runners were found with the provided data. Confirm that a runner exists in the database and the runner is not marked as present.";
            return RedirectToAction("Index", "Home", new { area = "Welcome" });
        }

        [HttpPost]
        public async Task<IActionResult>AddRunnerToCurrentPractice(AddRunnerToCurrentPracticeViewModel model) 
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedCheckboxOptions != null && model.SelectedCheckboxOptions.Count > 0)
                {
                    int runnersAddedToAttendance = 0;
                    foreach (var selectedOption in model.SelectedCheckboxOptions.Where(x => x.IsSelected))
                    {
                        Attendance attendance = new Attendance
                        {
                            IsPresent = selectedOption.IsSelected,
                            HasBeenSignedOut = false,
                            PracticeId = selectedOption.PracticeId,
                            RunnerId = selectedOption.RunnerId,
                        };

                        if (attendance != null)
                        {
                            _context.Attendances.Add(attendance);   
                            runnersAddedToAttendance++;
                        }
                    }

                    if (runnersAddedToAttendance == 0)
                    {
                        TempData["warning"] = "No runners were added to the attendance";
                        return RedirectToAction(nameof(CurrentPractices));
                    }

                    await _context.SaveChangesAsync();
                    TempData["success"] = runnersAddedToAttendance == 1 ? $"{runnersAddedToAttendance} runner was added to the practice" : $"{runnersAddedToAttendance} runners were added to the practice";
                    return RedirectToAction(nameof(CurrentPractices));
                }
            }

            TempData["error"] = "Invalid data was provided. There were no runners added to the attendance";
            return RedirectToAction(nameof(CurrentPractices));
        }

        public IActionResult SignRunnerOut(string runnerId, int practiceId)
        {
            if (practiceId == 0)
            {
                TempData["error"] = "Invalid practice id provided";
                return RedirectToAction("Index"); // send to an error page in the future
            } 

            var dbQuery = (from a in _context.Attendances
                           join p in _context.Practices
                           on a.PracticeId equals p.Id
                           join aspnetusers in _context.ApplicationUsers
                           on a.RunnerId equals aspnetusers.Id
                           where a.RunnerId == runnerId && p.Id == practiceId
                           select new
                           {
                               a.Id,
                               a.RunnerId,
                               p.PracticeLocation,
                               p.PracticeStartTimeAndDate,
                               aspnetusers.FirstName,
                               aspnetusers.LastName
                           }).FirstOrDefault();

            if (dbQuery == null)
            {
                TempData["error"] = "No runner was found with the provided data";
                return RedirectToAction("Index"); // send to an error page in the future
            }

            SignRunnerOutViewModel signRunnerOutViewModel = new SignRunnerOutViewModel 
            {
                AttendanceId = dbQuery.Id,
                PracticeLocation = dbQuery.PracticeLocation,
                PracticeStartDateTime = dbQuery.PracticeStartTimeAndDate,
                RunnersName = $"{dbQuery.FirstName} {dbQuery.LastName}"
            };

            

            return View(signRunnerOutViewModel);
        }

        [HttpPost]
        public IActionResult SignRunnerOut(int attendanceId)
        {
            if (attendanceId == 0)
            {
                return RedirectToAction("Index"); // send to an error page in the future (invalid id provided)
            }

            Attendance attendance = _context.Attendances.Find(attendanceId);

            if (attendance == null)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            attendance.HasBeenSignedOut = true;
            _context.Attendances.Update(attendance);
            _context.SaveChanges();

            return RedirectToAction(nameof(CurrentPractices)); // Send to a success page in the future
        }

    }
}
