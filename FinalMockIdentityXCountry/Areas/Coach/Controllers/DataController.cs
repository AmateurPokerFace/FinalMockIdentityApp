using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.DataController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class DataController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question
        public DataController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelectType()
        {
            return View();
        }

        public IActionResult RunnerPracticeWorkoutData()
        {
            var runnerUsers = _userManager.GetUsersInRoleAsync("Runner").Result;

            if (runnerUsers == null)
            {
                return RedirectToAction("Index"); // send to an error page in the future (no runners in the database).
            }

            RunnerPracticeWorkoutDataViewModel runnerPracticeWorkoutDataViewModel = new RunnerPracticeWorkoutDataViewModel();

            foreach (var runner in runnerUsers)
            {
                runnerPracticeWorkoutDataViewModel.RunnerUsers.Add((ApplicationUser)runner);
            }

            if (runnerPracticeWorkoutDataViewModel.RunnerUsers.Count() < 1)
            {
                return RedirectToAction(); // send to an error page in the future
            }

            return View(runnerPracticeWorkoutDataViewModel); 
        }

        public IActionResult EditWorkoutData(string runnerId)
        {
            if (runnerId == null)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }


            ApplicationUser applicationUser = _context.ApplicationUsers.Find(runnerId);

            if (applicationUser == null)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            EditWorkoutDataViewModel editWorkoutDataViewModel = new EditWorkoutDataViewModel { RunnerName = $"{applicationUser.FirstName} {applicationUser.LastName}"};

            var dbQueries = (from a in _context.Attendances
                             join w in _context.WorkoutInformation
                             on a.RunnerId equals w.RunnerId
                             join p in _context.Practices 
                             on a.PracticeId equals p.Id
                             join workoutTypes in _context.WorkoutTypes
                             on w.WorkoutTypeId equals workoutTypes.Id
                             where a.RunnerId == runnerId && a.PracticeId == w.PracticeId && a.RunnerId == w.RunnerId
                             select new
                             {
                                 a.RunnerId,
                                 a.PracticeId,
                                 p.PracticeStartTimeAndDate,
                                 p.PracticeLocation,
                                 workoutTypes.WorkoutName,
                                 w.Id
                             });

            List<EditWorkoutDataViewModel> editWorkoutDataViewModels = new List<EditWorkoutDataViewModel>();

            if (dbQueries.Count() > 0 && dbQueries != null)
            {
                int dbQueryPreviousPracticeId = dbQueries.FirstOrDefault().PracticeId;
                bool multipleLoops = false;

                foreach (var dbQuery in dbQueries)
                {
                    if (dbQueryPreviousPracticeId != dbQuery.PracticeId) // will not execute on first loop. dbQueryPreviousPracticeId was populated using dbQueriesFirstOrDefault().PracticeId.
                    {
                        editWorkoutDataViewModels.Add(editWorkoutDataViewModel);
                        multipleLoops = true;
                        editWorkoutDataViewModel = new EditWorkoutDataViewModel();
                    }

                    editWorkoutDataViewModel.RunnerId = dbQuery.RunnerId;
                    editWorkoutDataViewModel.PracticeId = dbQuery.PracticeId;
                    editWorkoutDataViewModel.PracticeLocation = dbQuery.PracticeLocation;
                    editWorkoutDataViewModel.RunnerName = $"{applicationUser.FirstName} {applicationUser.LastName}";
                    editWorkoutDataViewModel.PracticeStartTime = dbQuery.PracticeStartTimeAndDate;
                    editWorkoutDataViewModel.Workouts?.Add(dbQuery.WorkoutName);

                    dbQueryPreviousPracticeId = dbQuery.PracticeId;

                }

                if (multipleLoops)
                {
                    editWorkoutDataViewModels.Add(editWorkoutDataViewModel);
                }

                return View(editWorkoutDataViewModels);
            }

            return RedirectToAction("Index"); // send to an error page in the future. Check to see if an empty workoutTypes.WorkoutName adds a blank string (var dbQueries = select new ({}); line).
        }

        public IActionResult ViewDataEntered(string runnerId, int practiceId)
        {
            if (runnerId == null)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            var dbQueries = (from w in _context.WorkoutInformation
                             join p in _context.Practices
                             on w.PracticeId equals p.Id
                             join workoutTypes in _context.WorkoutTypes
                             on w.WorkoutTypeId equals workoutTypes.Id
                             join aspnetusers in _context.ApplicationUsers
                             on w.RunnerId equals aspnetusers.Id
                             where w.PracticeId == practiceId && w.RunnerId == runnerId
                             select new
                             {
                                 p.PracticeLocation,
                                 p.PracticeStartTimeAndDate,
                                 w.Pace,
                                 w.Distance,
                                 workoutTypes.WorkoutName,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                                 w.RunnerId,
                                 w.PracticeId,
                                 w.Id
                             });

            if (dbQueries.Count() > 0 )
            {
                ViewDataEnteredViewModel viewDataEnteredViewModel = new ViewDataEnteredViewModel
                {
                    PracticeId = practiceId,
                    PracticeLocation = dbQueries.FirstOrDefault()?.PracticeLocation,
                    PracticeStartDateTime = dbQueries.FirstOrDefault().PracticeStartTimeAndDate,
                    RunnerId = dbQueries.FirstOrDefault()?.RunnerId,
                    RunnerName = $"{dbQueries.FirstOrDefault()?.FirstName} {dbQueries.FirstOrDefault()?.LastName}" 
                };

                ViewDataEnteredViewModelHelper vmHelper = new ViewDataEnteredViewModelHelper();

                foreach (var dbQuery in dbQueries)
                {
                    vmHelper.Distance = dbQuery.Distance;
                    vmHelper.Pace = dbQuery.Pace;
                    vmHelper.WorkoutName = dbQuery.WorkoutName;
                    vmHelper.WorkoutId = dbQuery.Id;

                    viewDataEnteredViewModel.ViewDataEnteredViewModelHelpers?.Add(vmHelper);

                    vmHelper = new ViewDataEnteredViewModelHelper();
                }

                return View(viewDataEnteredViewModel);
            }
             
            return RedirectToAction("Index"); // send to an error page in the future
        }

        public IActionResult EditEnteredData(int workoutInformationId)
        {
            if (workoutInformationId == 0)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            var dbQuery = (from w in _context.WorkoutInformation
                           join workoutTypes in _context.WorkoutTypes
                           on w.WorkoutTypeId equals workoutTypes.Id
                           join aspnetusers in _context.ApplicationUsers
                           on w.RunnerId equals aspnetusers.Id
                           where w.Id == workoutInformationId
                           select new
                           {
                               w.Id,
                               workoutTypes.WorkoutName,
                               w.Pace,
                               w.Distance,
                               aspnetusers.FirstName,
                               aspnetusers.LastName
                           }).FirstOrDefault();

            if (dbQuery == null)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            EditEnteredDataViewModel editEnteredDataViewModel = new EditEnteredDataViewModel
            {
                RunnerName = $"{dbQuery.FirstName} {dbQuery.LastName}",
                Pace = dbQuery.Pace,
                Distance = dbQuery.Distance,
                WorkoutName = dbQuery.WorkoutName,
                WorkoutInformationId = dbQuery.Id
            };

            if (editEnteredDataViewModel != null)
            {
                return View(editEnteredDataViewModel);
            }

            return RedirectToAction("Index"); // send to an error page in the future
        }

        [HttpPost]
        public IActionResult EditEnteredData(EditEnteredDataViewModel editEnteredDataViewModel)
        {
            if (ModelState.IsValid && editEnteredDataViewModel != null)
            {
                WorkoutInformation workoutInformation = _context.WorkoutInformation.Find(editEnteredDataViewModel.WorkoutInformationId);

                if (workoutInformation == null)
                {
                   return RedirectToAction("Index"); // send to an error page in the future
                }

                workoutInformation.Distance = editEnteredDataViewModel.Distance;
                workoutInformation.Pace = editEnteredDataViewModel.Pace;
                _context.WorkoutInformation.Update(workoutInformation);
                _context.SaveChanges();

                return RedirectToAction("Index"); // send to a success page in the future
            }
            return RedirectToAction("Index"); // send to an error page in the future
        }
    }
}
