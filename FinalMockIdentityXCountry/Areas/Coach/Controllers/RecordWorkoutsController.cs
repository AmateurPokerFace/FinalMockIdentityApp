using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;
using FinalMockIdentityXCountry.Models.ViewModelHelperClasses;
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
    public class RecordWorkoutsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly UserManager<IdentityUser> _userManager; 
        public RecordWorkoutsController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult SelectPractice()
        {
            
            IEnumerable<Practice> practices = _unitOfWork.Practice.GetAll(p => p.PracticeIsInProgress == true);
            return View(practices);
        }


        public IActionResult RecordRunnersWorkouts(int practiceId)
        {
            var userClaimsIdentity = (ClaimsIdentity)User.Identity;
            var userClaim = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim != null)
            {
                var userClaimValue = userClaim.Value;
                
                Practice practice = _unitOfWork.Practice.GetAll(p => p.Id == practiceId).Where(c => c.CoachId == userClaimValue).FirstOrDefault();

                if (practice == null)
                {
                    // return a page stating there are no practices for the coach with that id?
                    return View(); 
                }

                TempData["RecordRunnersWorkoutPracticeLocation"] = practice.PracticeLocation;
                TempData["RecordRunnersWorkoutStartTime"] = practice.PracticeStartTimeAndDate;

                List<RecordWorkoutsViewModel> recordWorkoutViewModels = new List<RecordWorkoutsViewModel>();

                List<ApplicationUser> applicationUsers = new List<ApplicationUser>();

                List<Attendance> attendanceRunners = _unitOfWork.Attendance.GetAll(p => p.PracticeId == practiceId).Where(r => r.IsPresent).ToList();

                var identityUsers = _userManager.GetUsersInRoleAsync("Runner").Result;

                foreach (var runner in identityUsers)
                {
                    applicationUsers.Add((ApplicationUser)runner);
                }

                IEnumerable<WorkoutType> workoutTypes = _unitOfWork.WorkoutType.GetAll();

                foreach (var attendingRunner in attendanceRunners)
                {
                    foreach (var user in applicationUsers)
                    {
                        if (attendingRunner.RunnerId == user.Id)
                        {
                            RecordWorkoutsViewModel viewModel = new RecordWorkoutsViewModel();
                            foreach (var workoutType in workoutTypes)
                            {
                                WorkoutSelectionInformation workoutSelectionInformation = new WorkoutSelectionInformation
                                {
                                    PracticeId = practiceId,
                                    WorkoutDateTime = practice.PracticeStartTimeAndDate,
                                    WorkoutTypeId = workoutType.Id,
                                    WorkoutName = workoutType.WorkoutName,
                                    RunnerId = attendingRunner.RunnerId,
                                    PracticeLocation = practice.PracticeLocation
                                };

                                viewModel.WorkoutSelections.Add(workoutSelectionInformation);
                                viewModel.RunnerId = user.Id;
                                viewModel.RunnersName = $"{user.FirstName} {user.LastName}";
                            }

                            recordWorkoutViewModels.Add(viewModel);
                        }
                    }
                }

                return View(recordWorkoutViewModels);
            }

            return View();
        }

        [HttpPost]
        public IActionResult RecordRunnersWorkouts(List<RecordWorkoutsViewModel> recordWorkoutsViewModels)
        {
            // Create a warning page if any workouts contain null values.
            List<RecordWorkoutsViewModel> records = new List<RecordWorkoutsViewModel>();
            if (recordWorkoutsViewModels.Count > 0)
            {
                foreach (var record in recordWorkoutsViewModels)
                {
                    foreach (var workoutInfo in record.WorkoutSelections)
                    {
                        if (workoutInfo.WorkoutIsSelected)
                        {
                            WorkoutInformation workoutInformation = new WorkoutInformation 
                            {
                                WorkoutDateTime = workoutInfo.WorkoutDateTime,
                                WorkoutTypeId = workoutInfo.WorkoutTypeId,
                                PracticeId = workoutInfo.PracticeId,
                                RunnerId = workoutInfo.RunnerId
                            };
                            _unitOfWork.WorkoutInformation.Add(workoutInformation);
                        }
                    }
                }
                _unitOfWork.SaveChanges();
                return RedirectToAction("Index", "Welcome");
            }

            return RedirectToAction("Index", "Welcome");
        }
    }
}
