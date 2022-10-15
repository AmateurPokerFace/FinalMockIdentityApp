using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.Delete;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question
        public StartPracticeController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult StartNow()
        {
           
            StartNowViewModel startNowVm = new StartNowViewModel();

            var runnerUsers = _userManager.GetUsersInRoleAsync("Runner").Result;

            foreach (var runner in runnerUsers)
            {
                startNowVm.RunnerUsers.Add((ApplicationUser)runner);
            }
            return View(startNowVm); 
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult StartNow(StartNowViewModel startNowVm, string[] presentRunners)
        {
            if (ModelState.IsValid)
            {

            }
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
                
                _unitOfWork.Practice.Add(practice);
                _unitOfWork.SaveChanges();

                startNowVm.RunnerUsers = new List<ApplicationUser>();

                var runnerUsers = _userManager.GetUsersInRoleAsync("Runner").Result;

                foreach (var runner in runnerUsers)
                {
                    startNowVm.RunnerUsers.Add((ApplicationUser)runner);
                }

                if (startNowVm.RunnerUsers.Count > 0)
                {
                    for (int i = 0; i < startNowVm.RunnerUsers.Count; i++)
                    {
                        foreach (var presentRunner in presentRunners)
                        {
                            if (startNowVm.RunnerUsers[i].Id == presentRunner)
                            {
                                Attendance attendance = new Attendance();
                                attendance.PracticeId = practice.Id;
                                attendance.RunnerId = startNowVm.RunnerUsers[i].Id;
                                attendance.IsPresent = true;
                                attendance.AttendanceDate = practice.PracticeStartTimeAndDate;
                                attendance.HasBeenSignedOut = false;
                                startNowVm.RunnerUsers.Remove(startNowVm.RunnerUsers[i]);
                                _unitOfWork.Attendance.Add(attendance);
                            }
                        }
                    }

                    foreach (var absentRunner in startNowVm.RunnerUsers)
                    {
                        Attendance attendance = new Attendance();
                        attendance.PracticeId = practice.Id;
                        attendance.RunnerId = absentRunner.Id;
                        attendance.IsPresent = false;
                        attendance.AttendanceDate = practice.PracticeStartTimeAndDate;
                        attendance.HasBeenSignedOut = false;
                        _unitOfWork.Attendance.Add(attendance);
                    }

                    _unitOfWork.SaveChanges();

                    return RedirectToAction("Index", "Home", new { area = "Welcome" });
                }
                else
                {
                    // add a page for no runners in database??
                }
            }

            return RedirectToAction("Index", "Home", new { area = "Welcome" });
        }

        public IActionResult ScheduleASession()
        {
            ScheduleASessionViewModel scheduleASessionVm = new ScheduleASessionViewModel();
            var runnerUsers = _userManager.GetUsersInRoleAsync("Runner").Result;

            foreach (var runner in runnerUsers)
            {
                scheduleASessionVm.RunnerUsers.Add((ApplicationUser)runner);
            }
            return View(scheduleASessionVm); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ScheduleASession(ScheduleASessionViewModel scheduleASessionVm, string[] runnersAttending)
        {
            if (ModelState.IsValid)
            {

            }
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

                _unitOfWork.Practice.Add(practice);
                _unitOfWork.SaveChanges();

                scheduleASessionVm.RunnerUsers = new List<ApplicationUser>();

                var runnerUsers = _userManager.GetUsersInRoleAsync("Runner").Result;

                foreach (var runner in runnerUsers)
                {
                    scheduleASessionVm.RunnerUsers.Add((ApplicationUser)runner);
                }


                if (scheduleASessionVm.RunnerUsers.Count > 0)
                {
                    for (int i = 0; i < scheduleASessionVm.RunnerUsers.Count; i++)
                    {
                        foreach (var runnerAttending in runnersAttending)
                        {
                            if (scheduleASessionVm.RunnerUsers[i].Id == runnerAttending)
                            {
                                Attendance attendance = new Attendance();
                                attendance.PracticeId = practice.Id;
                                attendance.RunnerId = scheduleASessionVm.RunnerUsers[i].Id;
                                attendance.IsPresent = true;
                                attendance.AttendanceDate = practice.PracticeStartTimeAndDate;
                                attendance.HasBeenSignedOut = false;
                                scheduleASessionVm.RunnerUsers.Remove(scheduleASessionVm.RunnerUsers[i]);
                                _unitOfWork.Attendance.Add(attendance);
                            }
                        }
                    }

                    foreach (var runnerNotAttending in scheduleASessionVm.RunnerUsers)
                    {
                        Attendance attendance = new Attendance();
                        attendance.PracticeId = practice.Id;
                        attendance.RunnerId = runnerNotAttending.Id;
                        attendance.IsPresent = false;
                        attendance.AttendanceDate = practice.PracticeStartTimeAndDate;
                        attendance.HasBeenSignedOut = false;
                        _unitOfWork.Attendance.Add(attendance);
                    }

                    _unitOfWork.SaveChanges();

                    return RedirectToAction("Index", "Home", new {area = "Welcome"});
                    }
                else
                {
                    // add a page for no runners in database??
                }
            }

            return RedirectToAction("Index", "Home", new { area = "Welcome" });
        }

    }
}
