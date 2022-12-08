using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;
using FinalMockIdentityXCountry.Models.ViewModelHelperClasses;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.Delete;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.PracticeHistoryController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation.Context;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Versioning;
using System.Data;
using System.Security.Claims;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class PracticeHistoryController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question

        public PracticeHistoryController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index() 
        {
            return View(); 
        }

        public IActionResult History()
        {
            var practicesWithMoreThanOneAttendance = (from p in _context.Practices
                                                      join a in _context.Attendances
                                                      on p.Id equals a.PracticeId
                                                      where a.IsPresent && p.PracticeIsInProgress == false
                                                      group a by new
                                                      {
                                                          a.PracticeId,
                                                          p.PracticeLocation,
                                                          p.PracticeStartTimeAndDate
                                                      } into matchesFound
                                                      select new HistoryViewModel()
                                                      {
                                                          PracticeId = matchesFound.Key.PracticeId,
                                                          PracticeDateTime = matchesFound.Key.PracticeStartTimeAndDate,
                                                          PracticeLocation = matchesFound.Key.PracticeLocation,
                                                          TotalRunners = matchesFound.Count(),
                                                      }).ToList();

            List<int> practiceIdsWithMoreThanOneAttendance = new List<int>();

            List<HistoryViewModel> models = new List<HistoryViewModel>();
            
            if (practicesWithMoreThanOneAttendance != null && practicesWithMoreThanOneAttendance.Count() > 0)
            {
                foreach (var practiceWithMoreThanOneAttendance in practicesWithMoreThanOneAttendance)
                {
                    HistoryViewModel historyViewModel = new HistoryViewModel()
                    {
                        PracticeDateTime = practiceWithMoreThanOneAttendance.PracticeDateTime,
                        PracticeLocation = practiceWithMoreThanOneAttendance.PracticeLocation,
                        TotalRunners = practiceWithMoreThanOneAttendance.TotalRunners,
                        PracticeId = practiceWithMoreThanOneAttendance.PracticeId 
                    };

                    if (historyViewModel != null)
                    {
                        practiceIdsWithMoreThanOneAttendance.Add(practiceWithMoreThanOneAttendance.PracticeId);
                        models.Add(historyViewModel);
                    }
                    
                }  
            }

            if (practiceIdsWithMoreThanOneAttendance != null && practiceIdsWithMoreThanOneAttendance.Count > 0)
            {
                var practicesWithNoAttendances = _context.Practices.Where(x => !practiceIdsWithMoreThanOneAttendance.Contains(x.Id) && x.PracticeIsInProgress == false).ToList();

                if (practicesWithNoAttendances != null && practicesWithNoAttendances.Count > 0)
                {
                    foreach (var practiceWithNoAttendance in practicesWithNoAttendances)
                    {
                        HistoryViewModel historyViewModel = new HistoryViewModel
                        {
                            PracticeDateTime = practiceWithNoAttendance.PracticeStartTimeAndDate,
                            PracticeLocation = practiceWithNoAttendance.PracticeLocation,
                            PracticeId = practiceWithNoAttendance.Id,
                            TotalRunners = 0
                        };

                        if (historyViewModel != null)
                        {
                            models.Add(historyViewModel);
                        }
                    }
                }
            }
            else
            {
                var practicesWithNoAttendances = _context.Practices.Where(x => x.PracticeIsInProgress == false).ToList();
                if (practicesWithNoAttendances != null && practicesWithNoAttendances.Count > 0)
                {
                    foreach (var practiceWithNoAttendance in practicesWithNoAttendances)
                    {
                        HistoryViewModel historyViewModel = new HistoryViewModel
                        {
                            PracticeDateTime = practiceWithNoAttendance.PracticeStartTimeAndDate,
                            PracticeLocation = practiceWithNoAttendance.PracticeLocation,
                            PracticeId = practiceWithNoAttendance.Id,
                            TotalRunners = 0
                        };

                        if (historyViewModel != null)
                        {
                            models.Add(historyViewModel);
                        }
                    }
                }
            }

            if (models != null && models.Count > 0)
            {
                models = models.OrderByDescending(x => x.PracticeDateTime).ToList();
                return View(models);
            }

            TempData["error"] = "There was no practice history found";
            return RedirectToAction("Index", "Home", new { area = "Welcome" });
        }
        

        public IActionResult Selected(int practiceId)
        {
            if (practiceId == 0)
            {
                TempData["error"] = "The practice id provided is invalid.";
                return RedirectToAction("Index"); 
            }

            //Practice practice = _context.Practices.Where(p => p.Id == practiceId).FirstOrDefault();

            //if (practice == null)
            //{
            //    TempData["error"] = "The practice provided is invalid.";
            //    return RedirectToAction("Index"); 
            //}

            var dbQueries = (from p in _context.Practices
                             join a in _context.Attendances
                             on p.Id equals a.PracticeId
                             join aspnetusers in _context.ApplicationUsers
                             on a.RunnerId equals aspnetusers.Id
                             where a.IsPresent && p.PracticeIsInProgress == false && p.Id == practiceId
                             select new
                             {
                                 p.PracticeLocation,
                                 p.PracticeStartTimeAndDate,
                                 p.PracticeEndTimeAndDate,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName
                             });

            List<SelectedViewModel> models = new List<SelectedViewModel>();

            if (dbQueries != null && dbQueries.Count() > 0)
            {
                foreach (var dbQuery in dbQueries)
                {
                    SelectedViewModel selectedViewModel = new SelectedViewModel
                    { 
                        PracticeStartTime = TimeOnly.FromDateTime(dbQuery.PracticeStartTimeAndDate),
                        PracticeEndTime = TimeOnly.FromDateTime(dbQuery.PracticeEndTimeAndDate),
                        PracticeLocation= dbQuery.PracticeLocation,
                        RunnersName = $"{dbQuery.FirstName} {dbQuery.LastName}"
                    };

                    if (selectedViewModel != null)
                    {
                        models.Add(selectedViewModel);
                    }
                }
            }

            if (models != null && models.Count > 0)
            {
                return View(models);
            }

            TempData["error"] = "The practice provided is invalid.";
            return RedirectToAction("Index");
        }

        public IActionResult SelectedRunnerHistory(string runnerId)
        {
            if (runnerId == null)
            {
                TempData["error"] = "Invalid runner id provided";
                return RedirectToAction("Index"); // send to an error page in the future
            }

            List<SelectedRunnerHistoryViewModel> selectedRunnerHistoryViewModels = new List<SelectedRunnerHistoryViewModel>();
            SelectedRunnerHistoryViewModel selectedRunnerHistoryVm = new SelectedRunnerHistoryViewModel();

            var dbQueries = (from p in _context.Practices
                             join w in _context.WorkoutInformation
                             on p.Id equals w.PracticeId
                             join workoutTypes in _context.WorkoutTypes
                             on w.WorkoutTypeId equals workoutTypes.Id
                             join aspnetusers in _context.ApplicationUsers
                             on w.RunnerId equals aspnetusers.Id
                             where w.RunnerId == runnerId
                             select new
                             {
                                 p.PracticeStartTimeAndDate,
                                 p.PracticeLocation,
                                 workoutTypes.WorkoutName,
                                 w.PracticeId,
                                 w.RunnerId,
                                 aspnetusers.Id,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName
                             });

            if (dbQueries.Count() < 1)
            {
                TempData["error"] = "The selected runner has no practice history";
                return RedirectToAction("Index"); // send to a page that states the runner has no practice history in the future
            }

            int practiceId = dbQueries.FirstOrDefault().PracticeId;

            if (practiceId == null)
            {
                TempData["error"] = "Invalid practice id provided";
                return RedirectToAction("Index"); // send to an invalid practice id page (null value)
            }

            bool multipleLoops = false; // will be used to add the last object instance to the selectedViewModels List

            foreach (var dbQuery in dbQueries)
            { 
                if (practiceId != dbQuery.PracticeId) // will not execute on first loop. runnerId was populated using dbQueriesFirstOrDefault().RunnerId.
                {
                    selectedRunnerHistoryViewModels.Add(selectedRunnerHistoryVm);
                    multipleLoops = true;
                    selectedRunnerHistoryVm = new SelectedRunnerHistoryViewModel();
                }

                selectedRunnerHistoryVm.PracticeId = dbQuery.PracticeId;
                selectedRunnerHistoryVm.RunnerId = dbQuery.RunnerId;
                selectedRunnerHistoryVm.RunnersName = $"{dbQuery.FirstName} {dbQuery.LastName}";
                selectedRunnerHistoryVm.PracticeLocation = dbQuery.PracticeLocation == null ? " " : dbQuery.PracticeLocation;
                selectedRunnerHistoryVm.PracticeStartDate = DateOnly.FromDateTime(dbQuery.PracticeStartTimeAndDate);

                if (dbQuery.WorkoutName != null)
                {
                    selectedRunnerHistoryVm.PracticeWorkouts.Add(dbQuery.WorkoutName);
                }
            }

            if (multipleLoops)
            { 
                selectedRunnerHistoryViewModels.Add(selectedRunnerHistoryVm);
            }

            if (selectedRunnerHistoryViewModels.Count() > 0)
            {
                return View(selectedRunnerHistoryViewModels);
            }

            TempData["error"] = "No data found";
            return RedirectToAction("Index"); // send to an error page in the future
        }
    }
}
