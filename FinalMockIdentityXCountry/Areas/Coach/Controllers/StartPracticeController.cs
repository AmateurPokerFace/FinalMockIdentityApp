using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.StartPracticeController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class StartPracticeController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question
        public StartPracticeController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult StartNow()
        {
            StartNowViewModel startNowViewModel = new StartNowViewModel();

            var dbQueries = (from aspnetuserroles in _context.UserRoles
                             join aspnetusers in _context.ApplicationUsers
                             on aspnetuserroles.UserId equals aspnetusers.Id
                             join aspnetroles in _context.Roles
                             on aspnetuserroles.RoleId equals aspnetroles.Id
                             where aspnetroles.Name.ToLower() == "runner"
                             select new
                             {
                                 aspnetusers.Id,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                             });

            if (dbQueries != null && dbQueries.Count() > 0)
            {
                foreach (var dbQuery in dbQueries)
                {
                    ScheduleASessionViewModelCheckboxOptions checkboxOption = new ScheduleASessionViewModelCheckboxOptions
                    {
                        RunnerId = dbQuery.Id,
                        RunnerName = $"{dbQuery.FirstName} {dbQuery.LastName}",
                    };

                    startNowViewModel.SessionViewModelCheckboxOptions.Add(checkboxOption);
                }

                return View(startNowViewModel);
            }

            return RedirectToAction("Index"); // send to an error page in the future?

        }

        [HttpPost]
        public IActionResult StartNow(StartNowViewModel startNowVm)
        {
            if (ModelState.IsValid)
            {
                var userClaimsIdentity = (ClaimsIdentity)User.Identity;
                var userClaim = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (userClaim != null)
                {
                    Practice practice = new Practice
                    {
                        PracticeStartTimeAndDate = DateTime.Now,
                        PracticeIsInProgress = true,
                        PracticeLocation = startNowVm.PracticeLocation,
                        CoachId = userClaim.Value
                    };

                    _context.Practices.Add(practice);
                    _context.SaveChanges();

                    bool loopedAtLeastOnce = false;

                    foreach (var attendingRunner in startNowVm.SessionViewModelCheckboxOptions.Where(i => i.IsAttending))
                    {
                        loopedAtLeastOnce = true;

                        Attendance attendance = new Attendance()
                        {
                            PracticeId = practice.Id,
                            IsPresent = true,
                            RunnerId = attendingRunner.RunnerId,

                        };

                        _context.Attendances.Add(attendance);
                    }

                    foreach (var absentRunner in startNowVm.SessionViewModelCheckboxOptions.Where(i => i.IsAttending == false))
                    {
                        loopedAtLeastOnce = true;
                        Attendance attendance = new Attendance()
                        {
                            PracticeId = practice.Id,
                            IsPresent = false,
                            RunnerId = absentRunner.RunnerId,

                        };

                        _context.Attendances.Add(attendance);
                    }

                    if (loopedAtLeastOnce)
                    {
                        _context.SaveChanges();

                        return RedirectToAction();
                    }

                }

            } 

            return View(startNowVm);
        }

        public IActionResult ScheduleASession()
        {
            ScheduleASessionViewModel scheduleASessionViewModel = new ScheduleASessionViewModel();

            var dbQueries = (from aspnetuserroles in _context.UserRoles
                             join aspnetusers in _context.ApplicationUsers
                             on aspnetuserroles.UserId equals aspnetusers.Id
                             join aspnetroles in _context.Roles
                             on aspnetuserroles.RoleId equals aspnetroles.Id
                             where aspnetroles.Name.ToLower() == "runner"
                             select new
                             {
                                 aspnetusers.Id,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                             });

            if (dbQueries != null && dbQueries.Count() > 0)
            {
                foreach (var dbQuery in dbQueries)
                {
                    ScheduleASessionViewModelCheckboxOptions checkboxOption = new ScheduleASessionViewModelCheckboxOptions
                    {
                        RunnerId = dbQuery.Id,
                        RunnerName = $"{dbQuery.FirstName} {dbQuery.LastName}",
                    };

                    scheduleASessionViewModel.SessionViewModelCheckboxOptions.Add(checkboxOption);
                }

                return View(scheduleASessionViewModel);
            }

            return RedirectToAction("Index"); // send to an error page in the future?
        }

        [HttpPost]
        public IActionResult ScheduleASession(ScheduleASessionViewModel scheduleASessionVm)
        {
            if (ModelState.IsValid)
            {
                var userClaimsIdentity = (ClaimsIdentity)User.Identity;
                var userClaim = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (userClaim != null)
                {
                    Practice practice = new Practice
                    {
                        PracticeStartTimeAndDate = scheduleASessionVm.PracticeStartTimeAndDate,
                        PracticeEndTimeAndDate = scheduleASessionVm.PracticeEndTimeAndDate,
                        PracticeIsInProgress = true,
                        PracticeLocation = scheduleASessionVm.PracticeLocation,
                        CoachId = userClaim.Value
                    };

                    _context.Practices.Add(practice);
                    _context.SaveChanges();

                    bool loopedAtLeastOnce = false;

                    foreach (var attendingRunner in scheduleASessionVm.SessionViewModelCheckboxOptions.Where(i => i.IsAttending))
                    {
                        loopedAtLeastOnce = true;

                        Attendance attendance = new Attendance()
                        {
                            PracticeId = practice.Id,
                            IsPresent = true,
                            RunnerId = attendingRunner.RunnerId,

                        };

                        _context.Attendances.Add(attendance);
                    }

                    foreach (var absentRunner in scheduleASessionVm.SessionViewModelCheckboxOptions.Where(i => i.IsAttending == false))
                    {
                        loopedAtLeastOnce = true;
                        Attendance attendance = new Attendance()
                        {
                            PracticeId = practice.Id,
                            IsPresent = false,
                            RunnerId = absentRunner.RunnerId,

                        };

                        _context.Attendances.Add(attendance);
                    }

                    if (loopedAtLeastOnce)
                    {
                        _context.SaveChanges();

                        return RedirectToAction();
                    }

                } 

            }

            return View(scheduleASessionVm);
        }
    }
}
