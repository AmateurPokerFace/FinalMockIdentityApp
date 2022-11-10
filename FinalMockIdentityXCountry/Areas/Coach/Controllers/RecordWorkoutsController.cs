using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;
using FinalMockIdentityXCountry.Models.ViewModelHelperClasses;
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
    public class RecordWorkoutsController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question

        public RecordWorkoutsController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult SelectPractice()
        {
            
            IEnumerable<Practice> practices = _context.Practices.Where(p => p.PracticeIsInProgress == true && p.WorkoutsAddedToPractice == false);
            return View(practices);
        }
         
        public IActionResult AddPracticeWorkouts(int practiceId)
        {
            // Make this controller async in the future

            List<AddPracticeWorkoutsViewModel> addPracticeWorkoutsViewModels = new List<AddPracticeWorkoutsViewModel>();

            IEnumerable<WorkoutType> workoutTypes = _context.WorkoutTypes;
            
            if (workoutTypes.Count() < 1)
            {
                return RedirectToAction("Home"); // Send to an error page in the future
            }

            var dbQueries = (from a in _context.Attendances
                             join aspnetusers in _context.ApplicationUsers
                             on a.RunnerId equals aspnetusers.Id 
                             where a.PracticeId == practiceId && a.IsPresent 
                             select new
                             {
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                                 a.PracticeId,
                                 a.RunnerId 
                             });

            bool dbQueryFound = false;

            foreach (var dbQuery in dbQueries)
            {
                dbQueryFound = true;
                AddPracticeWorkoutsViewModel addPracticeWorkoutVm = new AddPracticeWorkoutsViewModel 
                {
                    PracticeId = dbQuery.PracticeId, RunnerId = dbQuery.RunnerId , RunnerName = $"{dbQuery.FirstName} {dbQuery.LastName}"
                };

                addPracticeWorkoutsViewModels.Add(addPracticeWorkoutVm);
            }

            if (dbQueryFound)
            {
                foreach (var vm in addPracticeWorkoutsViewModels)
                {
                    foreach (var workoutType in workoutTypes)
                    {
                        AddPracticeWorkoutCheckboxOptions addPracticeWorkoutCheckbox = new AddPracticeWorkoutCheckboxOptions
                        {
                            PracticeId = vm.PracticeId,
                            RunnerId = vm.RunnerId,
                            WorkoutType = workoutType,
                            WorkoutTypeId = workoutType.Id
                        };

                        vm.SelectedWorkoutCheckboxOptions.Add(addPracticeWorkoutCheckbox);
                    }
                }
                return View(addPracticeWorkoutsViewModels);
            }
            else
            {
                return RedirectToAction(); // return an invalid page (error in database query)
            } 
        }

        [HttpPost]
        public IActionResult AddPracticeWorkouts(List<AddPracticeWorkoutsViewModel> addPracticeWorkoutsViewModels, int practiceId)
        {
            if (addPracticeWorkoutsViewModels != null && practiceId != 0)
            {
                foreach (var addPracticeWorkoutVm in addPracticeWorkoutsViewModels)
                {
                    foreach (var checkboxOptions in addPracticeWorkoutVm.SelectedWorkoutCheckboxOptions)
                    {
                        if (checkboxOptions.IsSelected && checkboxOptions.PracticeId != 0)
                        {
                            WorkoutInformation workoutInfo = new WorkoutInformation
                            {
                                PracticeId = checkboxOptions.PracticeId,
                                RunnerId = checkboxOptions.RunnerId,
                                WorkoutTypeId = checkboxOptions.WorkoutTypeId
                            };
                            _context.WorkoutInformation.Add(workoutInfo);
                        }
                    }    
                }

                Practice practice = _context.Practices.Where(p => p.Id == practiceId).FirstOrDefault();
                practice.WorkoutsAddedToPractice = true;
                _context.Practices.Update(practice);

                _context.SaveChanges();

                return RedirectToAction("SelectPractice", "RecordWorkouts");
            }
            
            return RedirectToAction("Index", "Welcome"); // return error in the future
        }
    }
}
