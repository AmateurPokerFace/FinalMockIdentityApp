using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.ViewModelHelperClasses;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.DataController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;

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

        public IActionResult SelectPracticeType()
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
                return RedirectToAction("Index"); // send to an error page in the future
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
                                 w.Hours,
                                 w.Minutes,
                                 w.Seconds,
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
                    vmHelper.Hours = dbQuery.Hours;
                    vmHelper.Minutes = dbQuery.Minutes;
                    vmHelper.Seconds = dbQuery.Seconds;
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
                               w.Hours,
                               w.Minutes,
                               w.Seconds,
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
                Hours = dbQuery.Hours,
                Minutes = dbQuery.Minutes,
                Seconds = dbQuery.Seconds,
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
                workoutInformation.Hours = editEnteredDataViewModel.Hours;
                workoutInformation.Minutes = editEnteredDataViewModel.Minutes;
                workoutInformation.Seconds = editEnteredDataViewModel.Seconds;
                _context.WorkoutInformation.Update(workoutInformation);
                _context.SaveChanges();

                return RedirectToAction("RunnerPracticeWorkoutData", "Data"); // send to a success page in the future
            }
            return RedirectToAction("Index"); // send to an error page in the future
        }

        public IActionResult AddNewWorkoutsToPractice(string runnerId, int practiceId)
        {
            if (runnerId == null || practiceId == 0)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            Practice practice = _context.Practices.Find(practiceId);
            if (practice == null)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            ApplicationUser applicationUser = _context.ApplicationUsers.Find(runnerId);

            if (applicationUser == null)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            var dbQueries = from wt in _context.WorkoutTypes
                            where !_context.WorkoutInformation.Any(i => i.WorkoutTypeId == wt.Id
                            && i.RunnerId == runnerId && i.PracticeId == practiceId)
                            select wt;

            if (dbQueries.Count() < 0)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            AddNewWorkoutsToPracticeViewModel addNewWorkoutsToPracticeViewModel = new AddNewWorkoutsToPracticeViewModel 
            { PracticeId = practiceId, RunnerId = runnerId, RunnerName = $"{applicationUser.FirstName} {applicationUser.LastName}", PracticeLocation = practice.PracticeLocation, PracticeStartDateTime = practice.PracticeStartTimeAndDate};
            
            foreach (var dbQuery in dbQueries)
            {
                NewWorkoutCheckboxOptions newWorkoutCheckboxOptions = new NewWorkoutCheckboxOptions
                {
                    PracticeId = practiceId,
                    RunnerId = runnerId,
                    WorkoutName = dbQuery.WorkoutName,
                    WorkoutTypeId = dbQuery.Id,
                    IsSelected = false
                };

                addNewWorkoutsToPracticeViewModel.SelectedNewWorkoutCheckboxOptions?.Add(newWorkoutCheckboxOptions);
            }

            if (addNewWorkoutsToPracticeViewModel.SelectedNewWorkoutCheckboxOptions == null || addNewWorkoutsToPracticeViewModel.SelectedNewWorkoutCheckboxOptions.Count() < 1)
            {
                return RedirectToAction("Index"); // send to an error page in the future. Runner already has every workout selected
            }


            return View(addNewWorkoutsToPracticeViewModel);
        }

        [HttpPost]
        public IActionResult AddNewWorkoutsToPractice(AddNewWorkoutsToPracticeViewModel addNewWorkoutsToPracticeViewModel)
        {
            if (addNewWorkoutsToPracticeViewModel.RunnerId == null) 
            {
                return RedirectToAction("Index"); // send to an error page in the future.
            }

            Practice practice = _context.Practices.Find(addNewWorkoutsToPracticeViewModel.PracticeId);
            if (practice == null)
            {
                return RedirectToAction("Index"); // send to an error page in the future. (Invalid practice id provided).
            }

            bool loopedOnce = false;
             
            foreach (var newWorkout in addNewWorkoutsToPracticeViewModel.SelectedNewWorkoutCheckboxOptions.Where(i => i.IsSelected))
            {
                loopedOnce = true;

                WorkoutInformation workoutInformation = new WorkoutInformation
                {
                    PracticeId = newWorkout.PracticeId,
                    WorkoutTypeId = newWorkout.WorkoutTypeId,
                    RunnerId = newWorkout.RunnerId,
                };

                _context.WorkoutInformation.Add(workoutInformation);
            }

            if (loopedOnce)
            {
                _context.SaveChanges();
                return RedirectToAction("CurrentPracticeWorkoutData"); // send to a success page in the future
            }

             return RedirectToAction("Index"); // send to an error page in the future. Changes not reflected (loopOnce is false).  

        }

        public IActionResult DeleteWorkoutsFromPractice(string runnerId, int practiceId)
        {
            if (runnerId == null || practiceId == 0)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            Practice practice = _context.Practices.Find(practiceId);
            
            if (practice == null)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            var dbQueries = (from wi in _context.WorkoutInformation
                             join wt in _context.WorkoutTypes
                             on wi.WorkoutTypeId equals wt.Id
                             join aspnetusers in _context.ApplicationUsers
                             on wi.RunnerId equals aspnetusers.Id
                             join p in _context.Practices
                             on wi.PracticeId equals p.Id
                             where wi.RunnerId == runnerId && wi.PracticeId == practiceId
                             select new
                             {
                                 wi.Id,
                                 wt.WorkoutName,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                                 p.PracticeLocation,
                                 p.PracticeStartTimeAndDate,
                                 wi.PracticeId
                             });

            if (dbQueries.Count() > 0)
            {
                DeleteWorkoutsFromPracticeViewModel deleteWorkoutsFromPracticeViewModel = new DeleteWorkoutsFromPracticeViewModel 
                {
                    PracticeLocation = dbQueries.FirstOrDefault()?.PracticeLocation == null ? " " : dbQueries.FirstOrDefault()?.PracticeLocation,
                    PracticeStartDateTime = dbQueries.FirstOrDefault().PracticeStartTimeAndDate,
                    RunnerName = $"{dbQueries.FirstOrDefault()?.FirstName} {dbQueries.FirstOrDefault()?.LastName}",
                    PracticeId = dbQueries.FirstOrDefault().PracticeId
                };

                bool loopedOnce = false;

                foreach (var dbQuery in dbQueries)
                {
                    loopedOnce = true;
                    DeleteWorkoutsFromPracticeCheckBoxOptions deleteWorkoutsFromPracticeCheckBoxOptions = new DeleteWorkoutsFromPracticeCheckBoxOptions
                    {
                        WorkoutName = dbQuery.WorkoutName,
                        WorkoutInformationId = dbQuery.Id,
                        IsSelected = false
                    };

                    deleteWorkoutsFromPracticeViewModel.SelectedCheckboxOptions?.Add(deleteWorkoutsFromPracticeCheckBoxOptions);
                }
                if (loopedOnce)
                {
                    return View(deleteWorkoutsFromPracticeViewModel);
                }

                return RedirectToAction("Index"); // Send to an error page in the future
            } 

            return RedirectToAction("Index"); // Send to an error page in the future
        }

        [HttpPost]
        public IActionResult DeleteWorkoutsFromPractice(DeleteWorkoutsFromPracticeViewModel deleteWorkoutsFromPracticeViewModel)
        {
            if (deleteWorkoutsFromPracticeViewModel.PracticeId == 0)
            {
                return RedirectToAction("Index"); // Send to an error page in the future
            }
             
            
            bool recordFound = false;
            
            foreach (var workout in deleteWorkoutsFromPracticeViewModel.SelectedCheckboxOptions.Where(w => w.IsSelected))
            {
                WorkoutInformation workoutInformation = _context.WorkoutInformation.Find(workout.WorkoutInformationId);

                if (workoutInformation != null)
                {
                    recordFound = true;
                    _context.Remove(workoutInformation);
                }
            }
             
            if (recordFound)
            {
                _context.SaveChanges();

                return RedirectToAction("CurrentPracticeWorkoutData"); // send to a success page in the future
            }

            return RedirectToAction("Index");  // send to an error page in the future (no records were found).
        }

        public IActionResult CurrentPracticeWorkoutData()
        {
            List<CurrentPracticeWorkoutDataViewModel> currentPracticeWorkoutDataViewModels = new List<CurrentPracticeWorkoutDataViewModel>();


            var dbQueries = (from p in _context.Practices
                             join a in _context.Attendances
                             on p.Id equals a.PracticeId
                             where a.IsPresent && p.PracticeIsInProgress
                             group a by new
                             {
                                 a.PracticeId,
                                 p.PracticeLocation,
                                 p.PracticeStartTimeAndDate
                             } into matchesFound
                             select new CurrentPracticeWorkoutDataViewModel
                             {
                                 PracticeId = matchesFound.Key.PracticeId,
                                 PracticeDateTime = matchesFound.Key.PracticeStartTimeAndDate,
                                 PracticeLocation = matchesFound.Key.PracticeLocation,
                                 TotalRunners = matchesFound.Count(),
                             });


            if (dbQueries.Count() > 0)
            {
                foreach (var dbQuery in dbQueries)
                {
                    CurrentPracticeWorkoutDataViewModel currentPracticeVm = new CurrentPracticeWorkoutDataViewModel
                    {
                        PracticeId = dbQuery.PracticeId,
                        PracticeDateTime = dbQuery.PracticeDateTime,
                        PracticeLocation = dbQuery.PracticeLocation,
                        TotalRunners = dbQuery.TotalRunners
                    };

                    currentPracticeWorkoutDataViewModels.Add(currentPracticeVm);
                }
            }

            if (currentPracticeWorkoutDataViewModels.Count() > 0)
            {
                currentPracticeWorkoutDataViewModels = currentPracticeWorkoutDataViewModels.OrderByDescending(d => d.PracticeDateTime).ToList();
                return View(currentPracticeWorkoutDataViewModels);
            }

            return RedirectToAction(nameof(Index)); // send to an error page in the future (no practice history found)

        }

        public IActionResult PracticesThatHaveEndedWorkoutData()
        {
            List<CurrentPracticeWorkoutDataViewModel> currentPracticeWorkoutDataViewModels = new List<CurrentPracticeWorkoutDataViewModel>();

            var dbQueries = (from p in _context.Practices
                             join a in _context.Attendances
                             on p.Id equals a.PracticeId
                             where a.IsPresent && p.PracticeIsInProgress == false
                             group a by new
                             {
                                 a.PracticeId,
                                 p.PracticeLocation,
                                 p.PracticeStartTimeAndDate
                             } into matchesFound
                             select new CurrentPracticeWorkoutDataViewModel
                             {
                                 PracticeId = matchesFound.Key.PracticeId,
                                 PracticeDateTime = matchesFound.Key.PracticeStartTimeAndDate,
                                 PracticeLocation = matchesFound.Key.PracticeLocation,
                                 TotalRunners = matchesFound.Count(),
                             });

            if (dbQueries.Count() > 0)
            {
                foreach (var dbQuery in dbQueries)
                {
                    CurrentPracticeWorkoutDataViewModel currentPracticeVm = new CurrentPracticeWorkoutDataViewModel
                    {
                        PracticeId = dbQuery.PracticeId,
                        PracticeDateTime = dbQuery.PracticeDateTime,
                        PracticeLocation = dbQuery.PracticeLocation,
                        TotalRunners = dbQuery.TotalRunners
                    };

                    currentPracticeWorkoutDataViewModels.Add(currentPracticeVm);
                }
            }

            if (currentPracticeWorkoutDataViewModels.Count() > 0)
            {
                currentPracticeWorkoutDataViewModels = currentPracticeWorkoutDataViewModels.OrderByDescending(d => d.PracticeDateTime).ToList();
                return View(currentPracticeWorkoutDataViewModels);
            }

            return RedirectToAction(nameof(Index)); // send to an error page in the future (no practice history found)
        }

        public IActionResult SelectRunnerFromPractice(int practiceId)
        {
            if (practiceId == 0)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            var dbQueries = (from a in _context.Attendances
                             join p in _context.Practices
                             on a.PracticeId equals p.Id
                             join aspnetusers in _context.ApplicationUsers
                             on a.RunnerId equals aspnetusers.Id
                             where a.PracticeId == practiceId && a.IsPresent
                             select new
                             {
                                 p.PracticeStartTimeAndDate,
                                 p.PracticeLocation,
                                 p.Id,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                                 a.RunnerId
                             });

            if (dbQueries != null && dbQueries.Count() > 0)
            {
                SelectRunnerFromPracticeViewModel selectRunnerFromPracticeViewModel = new SelectRunnerFromPracticeViewModel
                {
                    PracticeLocation = dbQueries.FirstOrDefault()?.PracticeLocation == null ? " " : dbQueries.FirstOrDefault()?.PracticeLocation,
                    PracticeStartTimeAndDate = dbQueries.FirstOrDefault().PracticeStartTimeAndDate
                };

                foreach (var dbQuery in dbQueries)
                {
                    SelectRunnerFromPracticeViewModelHelper viewModelHelper = new SelectRunnerFromPracticeViewModelHelper
                    {
                        PracticeId = dbQuery.Id,
                        RunnerId = dbQuery.RunnerId,
                        RunnerName = $"{dbQuery.FirstName} {dbQuery.LastName}"
                    };

                    selectRunnerFromPracticeViewModel.SelectRunnerFromPracticeViewModelHelpers?.Add(viewModelHelper);
                }

                if (selectRunnerFromPracticeViewModel.SelectRunnerFromPracticeViewModelHelpers != null && selectRunnerFromPracticeViewModel.SelectRunnerFromPracticeViewModelHelpers.Count() > 0)
                {
                    selectRunnerFromPracticeViewModel.SelectRunnerFromPracticeViewModelHelpers = selectRunnerFromPracticeViewModel.SelectRunnerFromPracticeViewModelHelpers.OrderByDescending(r => r.RunnerName).Reverse().ToList();
                    return View(selectRunnerFromPracticeViewModel);
                }
                
                return RedirectToAction("Index"); // send to an error page in the future (no runners were found in the provided practice).
            }

            return RedirectToAction("Index"); // send to an error page in the future
        }

        public IActionResult EditRunnerCurrentPracticeData(string runnerId, int practiceId)
        {
            if (runnerId == null || practiceId == 0)
            {
                return RedirectToAction("Index"); // send to an error page in the future (invalid Id(s) provided)
            }

            EditRunnerCurrentPracticeDataViewModel editRunnerCurrentPracticeDataViewModel = new EditRunnerCurrentPracticeDataViewModel();

            var workoutsQuery = (from w in _context.WorkoutInformation
                                 join wt in _context.WorkoutTypes
                                 on w.WorkoutTypeId equals wt.Id
                                 where w.PracticeId == practiceId && w.RunnerId == runnerId
                                 select new EditRunnerCurrentPracticeDataViewModel()
                                 {
                                     Workout = wt.WorkoutName
                                 }).ToList();

            foreach (var workout in workoutsQuery.Where(w => w.Workout != null))
            {
                editRunnerCurrentPracticeDataViewModel.Workouts?.Add(workout.Workout);
            }

            var dbQuery = (from a in _context.Attendances
                           join p in _context.Practices
                           on a.PracticeId equals p.Id
                           join aspnetusers in _context.ApplicationUsers
                           on a.RunnerId equals aspnetusers.Id
                           where a.IsPresent && a.PracticeId == practiceId && a.RunnerId == runnerId && p.PracticeIsInProgress
                           select new
                           {
                               p.PracticeLocation,
                               p.PracticeStartTimeAndDate,
                               p.Id,
                               a.RunnerId,
                               aspnetusers.FirstName,
                               aspnetusers.LastName
                           }).FirstOrDefault();
             

            if (dbQuery == null)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            editRunnerCurrentPracticeDataViewModel.RunnersName = $"{dbQuery.FirstName} {dbQuery.LastName}";
            editRunnerCurrentPracticeDataViewModel.PracticeLocation = dbQuery.PracticeLocation;
            editRunnerCurrentPracticeDataViewModel.PracticeStartDateTime = dbQuery.PracticeStartTimeAndDate;
            editRunnerCurrentPracticeDataViewModel.RunnerId = dbQuery.RunnerId;
            editRunnerCurrentPracticeDataViewModel.PracticeId = dbQuery.Id;

            IQueryable<WorkoutType> records = _context.WorkoutTypes;
            editRunnerCurrentPracticeDataViewModel.WorkoutTypeRecordCount = records.Count();

            return View(editRunnerCurrentPracticeDataViewModel);
        }
    }
}
