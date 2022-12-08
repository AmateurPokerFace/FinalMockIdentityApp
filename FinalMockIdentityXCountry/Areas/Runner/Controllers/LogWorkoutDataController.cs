using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels.Helper;
using FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using System.Linq;

namespace FinalMockIdentityXCountry.Areas.Runner.Controllers
{
    [Authorize(Roles = "Runner")]
    [Area("Runner")]
    public class LogWorkoutDataController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question

        public LogWorkoutDataController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelectAPractice()
        {
            var userClaimsIdentity = (ClaimsIdentity?)User.Identity;
            var userClaim = userClaimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            List<SelectAPracticeViewModel> selectAPracticeViewModels = new List<SelectAPracticeViewModel>();

            if (userClaim != null)
            {
                var dbQueries = (from w in _context.WorkoutInformation
                                 join practices in _context.Practices
                                 on w.PracticeId equals practices.Id
                                 join workoutTypes in _context.WorkoutTypes
                                 on w.WorkoutTypeId equals workoutTypes.Id
                                 where practices.PracticeIsInProgress && w.RunnerId == userClaim.Value
                                 select new
                                 {
                                     w.Id,
                                     w.PracticeId,
                                     w.RunnerId, 
                                     w.DataWasLogged,
                                     practices.PracticeStartTimeAndDate,
                                     practices.PracticeLocation,
                                     workoutTypes.WorkoutName
                                 });

                if (dbQueries != null && dbQueries.Count() > 0)
                {
                    foreach (var dbQuery in dbQueries)
                    {
                        SelectAPracticeViewModel selectedVm = new SelectAPracticeViewModel
                        {
                            WorkoutInformationId = dbQuery.Id,
                            PracticeId = dbQuery.PracticeId,
                            RunnerId = dbQuery.RunnerId,
                            DataWasLogged = dbQuery.DataWasLogged,
                            PracticeStartTimeAndDate = dbQuery.PracticeStartTimeAndDate,
                            PracticeLocation = dbQuery.PracticeLocation,
                            WorkoutName = dbQuery.WorkoutName
                        };

                        selectAPracticeViewModels.Add(selectedVm);
                        //currentPracticesViewModel.CurrentPracticesViewModelsHelper.Add(currentViewModel);
                    };

                    return View(selectAPracticeViewModels);
                }

                TempData["error"] = "You do not have any workout history";
                return RedirectToAction("Index", "Home", new { area = "Welcome" });

            }

            TempData["error"] = "Invalid user";
            return RedirectToAction("Index", "Home", new { area = "Welcome" });
        }

        public IActionResult LogData(int workoutInfoId, int practiceId)
        {

            if (workoutInfoId != 0 && practiceId != 0)
            {
                var userClaimsIdentity = (ClaimsIdentity?)User.Identity;
                var userClaim = userClaimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

                if (userClaim != null)
                {
                    var dbQuery = (from w in _context.WorkoutInformation
                                   join practices in _context.Practices
                                   on w.PracticeId equals practices.Id
                                   join workoutTypes in _context.WorkoutTypes
                                   on w.WorkoutTypeId equals workoutTypes.Id
                                   where w.Id == workoutInfoId && w.PracticeId == practiceId && w.DataWasLogged == false
                                   select new
                                   {
                                       w.PracticeId,
                                       w.Id,
                                       w.RunnerId,
                                       practices.PracticeStartTimeAndDate,
                                       practices.PracticeLocation,
                                       workoutTypes.WorkoutName
                                    }).FirstOrDefault();

                    if (dbQuery == null)
                    {
                        TempData["error"] = "Invalid data provided. There was no workout information found in the database";
                        return RedirectToAction("Index", "Home", new { area = "Welcome" });
                    }

                    LogDataViewModel logDataViewModel = new LogDataViewModel
                    {
                        PracticeId = dbQuery.PracticeId,
                        PracticeLocation = dbQuery.PracticeLocation,
                        PracticeStartDateTime = dbQuery.PracticeStartTimeAndDate,
                        RunnerId = dbQuery.RunnerId,
                        WorkoutInformationId = dbQuery.Id,
                        WorkoutName = dbQuery.WorkoutName 
                    };

                    return View(logDataViewModel);
                }

                TempData["error"] = "Invalid user";
                return RedirectToAction("Index", "Home", new { area = "Welcome" });
            }

            TempData["error"] = "Invalid ID(s) provided";
            return RedirectToAction("Index", "Home", new { area = "Welcome" });

        }

        [HttpPost]
        public IActionResult LogData(LogDataViewModel logDataViewModel)
        {

            if (ModelState.IsValid)
            {
                WorkoutInformation workoutInformationToUpdate = _context.WorkoutInformation
                .Where(w => w.Id == logDataViewModel.WorkoutInformationId).FirstOrDefault();

                if (workoutInformationToUpdate != null)
                {
                    workoutInformationToUpdate.Distance = logDataViewModel.Distance;
                    workoutInformationToUpdate.Hours = logDataViewModel.Hours;
                    workoutInformationToUpdate.Minutes = logDataViewModel.Minutes;
                    workoutInformationToUpdate.Seconds = logDataViewModel.Seconds;
                    workoutInformationToUpdate.DataWasLogged = true;

                    _context.Update(workoutInformationToUpdate);
                    _context.SaveChanges();

                    TempData["success"] = "The workout data was saved successfully";

                    return RedirectToAction("Index"); // send to success page in the future
                }
                 
                TempData["error"] = "The provided workout information was not found in the database.";
                return RedirectToAction("Index", "Home", new { area = "Welcome" });
            }

            var dbQuery = (from w in _context.WorkoutInformation
                           join practices in _context.Practices
                           on w.PracticeId equals practices.Id
                           join workoutTypes in _context.WorkoutTypes
                           on w.WorkoutTypeId equals workoutTypes.Id
                           where w.Id == logDataViewModel.WorkoutInformationId && w.PracticeId == logDataViewModel.PracticeId && w.DataWasLogged == false
                           select new
                           {
                               w.PracticeId,
                               w.Id,
                               w.RunnerId,
                               practices.PracticeStartTimeAndDate,
                               practices.PracticeLocation,
                               workoutTypes.WorkoutName
                           }).FirstOrDefault();

            if (dbQuery == null)
            {
                TempData["error"] = "Invalid data provided. There was no workout information found in the database";
                return RedirectToAction("Index", "Home", new { area = "Welcome" });
            }
             
            logDataViewModel.PracticeId = dbQuery.PracticeId;
            logDataViewModel.PracticeLocation = dbQuery.PracticeLocation;
            logDataViewModel.PracticeStartDateTime = dbQuery.PracticeStartTimeAndDate;
            logDataViewModel.RunnerId = dbQuery.RunnerId;
            logDataViewModel.WorkoutInformationId = dbQuery.Id;
            logDataViewModel.WorkoutName = dbQuery.WorkoutName; 
             
            return View(logDataViewModel);  
        }

        public IActionResult EditLoggedData(int workoutInfoId, int practiceId)
        {
            if (workoutInfoId != 0 && practiceId != 0)
            {
                var userClaimsIdentity = (ClaimsIdentity?)User.Identity;
                var userClaim = userClaimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

                if (userClaim != null)
                {
                    var dbQuery = (from w in _context.WorkoutInformation
                                   join practices in _context.Practices
                                   on w.PracticeId equals practices.Id
                                   join workoutTypes in _context.WorkoutTypes
                                   on w.WorkoutTypeId equals workoutTypes.Id
                                   where w.Id == workoutInfoId && w.PracticeId == practiceId
                                   select new
                                   {
                                       w.PracticeId,
                                       w.Id,
                                       w.RunnerId,
                                       w.Distance,
                                       w.Hours,
                                       w.Minutes,
                                       w.Seconds,
                                       practices.PracticeStartTimeAndDate,
                                       practices.PracticeLocation,
                                       workoutTypes.WorkoutName
                                   }).FirstOrDefault();

                    if (dbQuery == null)
                    {
                        return RedirectToAction(); // Send to an error page in the future
                    }

                    EditLoggedDataViewModel editLoggedDataViewModel = new EditLoggedDataViewModel
                    {
                        PracticeId = dbQuery.PracticeId,
                        PracticeLocation = dbQuery.PracticeLocation,
                        PracticeStartDateTime = dbQuery.PracticeStartTimeAndDate,
                        RunnerId = dbQuery.RunnerId,
                        WorkoutInformationId = dbQuery.Id,
                        WorkoutName = dbQuery.WorkoutName,
                        Distance = dbQuery.Distance,
                        Hours = dbQuery.Hours,
                        Minutes = dbQuery.Minutes,
                        Seconds = dbQuery.Seconds, 
                    };

                    return View(editLoggedDataViewModel);
                }
                return View();
            }

            return RedirectToAction(); // Send to an error page in the future
        }

        [HttpPost]
        public IActionResult EditLoggedData(EditLoggedDataViewModel editLoggedDataViewModel)
        {
            
            if (ModelState.IsValid)
            {
                WorkoutInformation workoutInformationToUpdate = _context.WorkoutInformation
                .Where(w => w.Id == editLoggedDataViewModel.WorkoutInformationId).FirstOrDefault();

                if (workoutInformationToUpdate != null)
                {
                    workoutInformationToUpdate.Distance = editLoggedDataViewModel.Distance;
                    workoutInformationToUpdate.Hours = editLoggedDataViewModel.Hours;
                    workoutInformationToUpdate.Minutes = editLoggedDataViewModel.Minutes;
                    workoutInformationToUpdate.Seconds = editLoggedDataViewModel.Seconds;
                    workoutInformationToUpdate.DataWasLogged = true;

                    _context.Update(workoutInformationToUpdate);
                    _context.SaveChanges();

                    TempData["success"] = "The workout data was edited successfully";

                    return RedirectToAction("Index"); // send to success page in the future
                }

                TempData["error"] = "The provided workout information was not found in the database.";
                return RedirectToAction("Index", "Home", new { area = "Welcome" });
            }

            var dbQuery = (from w in _context.WorkoutInformation
                           join practices in _context.Practices
                           on w.PracticeId equals practices.Id
                           join workoutTypes in _context.WorkoutTypes
                           on w.WorkoutTypeId equals workoutTypes.Id
                           where w.Id == editLoggedDataViewModel.WorkoutInformationId && w.PracticeId == editLoggedDataViewModel.PracticeId && w.DataWasLogged == false
                           select new
                           {
                               w.PracticeId,
                               w.Id,
                               w.RunnerId,
                               practices.PracticeStartTimeAndDate,
                               practices.PracticeLocation,
                               workoutTypes.WorkoutName
                           }).FirstOrDefault();

            if (dbQuery == null)
            {
                TempData["error"] = "Invalid data provided. There was no workout information found in the database";
                return RedirectToAction("Index", "Home", new { area = "Welcome" });
            }

            editLoggedDataViewModel.PracticeId = dbQuery.PracticeId;
            editLoggedDataViewModel.PracticeLocation = dbQuery.PracticeLocation;
            editLoggedDataViewModel.PracticeStartDateTime = dbQuery.PracticeStartTimeAndDate;
            editLoggedDataViewModel.RunnerId = dbQuery.RunnerId;
            editLoggedDataViewModel.WorkoutInformationId = dbQuery.Id;
            editLoggedDataViewModel.WorkoutName = dbQuery.WorkoutName;

            return View(editLoggedDataViewModel);
        }
    }
}
