using FinalMockIdentityXCountry.Models;
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

        public IActionResult CurrentPractices()
        {
            IEnumerable<Practice> practices = _context.Practices.Where(p => p.PracticeIsInProgress);
            if (practices == null || practices.Count() == 0)
            {
               return View("NoPracticesInProgress");
            }

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
                    Practice practice = _context.Practices.Find(currentPracticeId); // no runners are in the provided practiceId
                    if (practice == null) // check to see if a practice exists with the provided id in the database
                    {
                        TempData["error"] = "The practice doesn't exist in the database. Please provide a valid practice id.";
                    }
                    else
                    {
                        currentViewModel.PracticeLocation = practice.PracticeLocation;
                        currentViewModel.PracticeStartTimeAndDate = practice.PracticeStartTimeAndDate;
                        return View(currentViewModel);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Attendance(int practiceId)
        {
            if (practiceId == 0)
            {
                return RedirectToAction(); // send to an invalid page in the future
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
                return RedirectToAction(); // send to an invalid page in the future
            }

            AttendanceViewModel attendanceViewModel = new AttendanceViewModel
            {
                PracticeLocation = dbQueries.FirstOrDefault()?.PracticeLocation == null ? " " : dbQueries.FirstOrDefault()?.PracticeLocation,
                PracticeStartTimeAndDate = dbQueries.FirstOrDefault().PracticeStartTimeAndDate
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

        public IActionResult SignRunnerOut(string runnerId, int practiceId)
        {
            if (practiceId == 0)
            {
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
