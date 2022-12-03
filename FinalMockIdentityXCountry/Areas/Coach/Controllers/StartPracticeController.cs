using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.StartPracticeController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using System.Text;

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

        public IActionResult Index() 
        {
            return View();
        }
        public IActionResult StartNow()
        {

            StartNowViewModel startNowVm = new StartNowViewModel();

            var runnerUsers = _userManager.GetUsersInRoleAsync("Runner").Result;
            List<ApplicationUser> users = new List<ApplicationUser>();

            foreach (var runner in runnerUsers)
            {
                users.Add((ApplicationUser)runner);
            }

            if (users != null && users.Count > 0)
            {
                users = users.OrderBy(x => x.UserName).ToList();

                foreach (var user in users)
                {
                    StartNowCheckBoxOptions checkBoxOptions = new StartNowCheckBoxOptions
                    {
                        RunnersName = $"{user.FirstName} {user.LastName}",
                        RunnersId = user.Id,
                    };

                    startNowVm.SelectedStartNowCheckBoxOptions?.Add(checkBoxOptions);
                }
            }
             
            return View(startNowVm);
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

                    try
                    {
                        _context.Practices.Add(practice);
                        _context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        TempData["error"] = e.Message;
                        return RedirectToAction(nameof(Index));
                    }
                    

                    if (startNowVm.SelectedStartNowCheckBoxOptions != null && startNowVm.SelectedStartNowCheckBoxOptions.Count > 0)
                    {
                        bool attendanceFound = false; 

                        foreach (var selectedCheckboxOption in startNowVm.SelectedStartNowCheckBoxOptions)
                        {
                            Attendance attendance = new Attendance
                            {
                                PracticeId = practice.Id,
                                RunnerId = selectedCheckboxOption.RunnersId,
                                IsPresent = selectedCheckboxOption.IsSelected,
                                HasBeenSignedOut = false,
                            };

                            if (attendance != null)
                            {
                                attendanceFound = true;
                                _context.Attendances.Add(attendance); 
                            }
                        }

                        if (attendanceFound) 
                        {
                            _context.SaveChanges(); 
                        } 
                    }
                    
                    TempData["success"] = "The practice was started successfully";
                    return RedirectToAction(nameof(Index));
                } 
            }

            TempData["error"] = "The practice was unable to be started because invalid data was submitted.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ScheduleASession()
        {
            ScheduleASessionViewModel scheduleASessionVm = new ScheduleASessionViewModel();
            var runnerUsers = _userManager.GetUsersInRoleAsync("Runner").Result;
            List<ApplicationUser> users = new List<ApplicationUser>();

            foreach (var runner in runnerUsers)
            {
                users.Add((ApplicationUser)runner);
            }

            if (users != null && users.Count > 0)
            {
                users = users.OrderBy(x => x.UserName).ToList();

                foreach (var user in users)
                {
                    StartNowCheckBoxOptions checkBoxOptions = new StartNowCheckBoxOptions
                    {
                        RunnersName = $"{user.FirstName} {user.LastName}",
                        RunnersId = user.Id,
                    };

                    scheduleASessionVm.SelectedStartNowCheckBoxOptions?.Add(checkBoxOptions);
                }
            }

            return View(scheduleASessionVm);
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
                        CoachId = userClaim.Value,
                        PracticeStartTimeAndDate = scheduleASessionVm.PracticeStartTimeAndDate,
                        PracticeEndTimeAndDate = scheduleASessionVm.PracticeEndTimeAndDate,
                        PracticeIsInProgress = true,
                        PracticeLocation = scheduleASessionVm.PracticeLocation,
                    };

                    try
                    {
                        _context.Practices.Add(practice);
                        _context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        TempData["error"] = e.Message;
                        return RedirectToAction(nameof(Index));
                    }

                    if (scheduleASessionVm.SelectedStartNowCheckBoxOptions != null && scheduleASessionVm.SelectedStartNowCheckBoxOptions.Count > 0)
                    {
                        bool attendanceFound = false;

                        foreach (var selectedCheckboxOption in scheduleASessionVm.SelectedStartNowCheckBoxOptions)
                        {
                            Attendance attendance = new Attendance
                            {
                                PracticeId = practice.Id,
                                RunnerId = selectedCheckboxOption.RunnersId,
                                IsPresent = selectedCheckboxOption.IsSelected,
                                HasBeenSignedOut = false,
                            };

                            if (attendance != null)
                            {
                                attendanceFound = true;
                                _context.Attendances.Add(attendance);
                            }
                        }

                        if (attendanceFound)
                        {
                            _context.SaveChanges();
                        }
                    }

                    TempData["success"] = "The practice was scheduled successfully";
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["error"] = "The practice was unable to be scheduled because invalid data was submitted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
