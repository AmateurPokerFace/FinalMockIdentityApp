using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class WorkoutsController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question

        public WorkoutsController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CurrentWorkouts()
        {
            IEnumerable<WorkoutType> workoutTypes = _context.WorkoutTypes;
            return View(workoutTypes);
        }

        public IActionResult AddNewWorkout()
        {
            AddNewWorkoutViewModel addNewWorkoutViewModel = new AddNewWorkoutViewModel();
            return View(addNewWorkoutViewModel);
        }

        [HttpPost]
        public IActionResult AddNewWorkout(AddNewWorkoutViewModel addNewWorkoutViewModel)
        {
            if (addNewWorkoutViewModel == null)
            {
                return RedirectToAction(); //send to an error page in the future
            }

            WorkoutType workoutType = _context.WorkoutTypes.Where(w => w.WorkoutName.ToLower() == addNewWorkoutViewModel.WorkoutName.ToLower()).FirstOrDefault(); // see if the provided workout name is already in the database

            if (workoutType != null) // match found
            {
                TempData["error"] = $"The workout: {workoutType.WorkoutName} was not added to the database because it already exists in the database.";
                return RedirectToAction("CurrentWorkouts");
            }
             
            workoutType = new WorkoutType { WorkoutName = addNewWorkoutViewModel.WorkoutName };
            
            _context.WorkoutTypes.Add(workoutType);
            _context.SaveChanges();

            TempData["success"] = $"The workout: {workoutType.WorkoutName} was added to the database successfully";
            return RedirectToAction("CurrentWorkouts");
        }

        public IActionResult WorkoutExists(string workoutName)
        {
            return View(model: workoutName);
        }

        public IActionResult WorkoutSaved(string workoutName)
        {
            return View(model: workoutName);
        }

        public IActionResult EditWorkoutName(int workoutId)
        {
            if (workoutId == 0)
            {
                TempData["error"] = "Invalid workout id provided";
                return RedirectToAction(nameof(CurrentWorkouts)); //send to an error page in the future
            }

            WorkoutType workoutType = _context.WorkoutTypes.Where(w => w.Id == workoutId).FirstOrDefault();
            
            if (workoutType == null)
            {
                TempData["error"] = "Invalid id provided. Workout type not found";
                return RedirectToAction("CurrentWorkouts"); //Send to an error page in the future
            }

            return View(workoutType); 
        }

        [HttpPost]
        public IActionResult EditWorkoutName(WorkoutType workoutType)
        {
            if (ModelState.IsValid)
            {
                _context.WorkoutTypes.Update(workoutType);
                _context.SaveChanges();

                TempData["success"] = "The workout edit saved successfully";
                return RedirectToAction(nameof(CurrentWorkouts)); // SEND to a success page in the future
            }

            TempData["error"] = "An error occured. The provided workouttype was not valid";
            return RedirectToAction(nameof(CurrentWorkouts)); //send to an error page in the future  
        }
    }
}
