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
            List<HistoryViewModel> historyViewModels = new List<HistoryViewModel>();
 

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
                                  select new HistoryViewModel()
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
                    HistoryViewModel historyViewModel = new HistoryViewModel
                    {
                        PracticeId = dbQuery.PracticeId,
                        PracticeDateTime = dbQuery.PracticeDateTime,
                        PracticeLocation = dbQuery.PracticeLocation,
                        TotalRunners = dbQuery.TotalRunners
                    };

                    historyViewModels.Add(historyViewModel);
                } 
            }

            if (historyViewModels.Count() > 0)
            {
                historyViewModels = historyViewModels.OrderByDescending(d => d.PracticeDateTime).ToList();
                return View(historyViewModels);
            }

            return RedirectToAction(nameof(Index)); // send to an error page in the future (no practice history found)
        }
        

        public IActionResult Selected(int practiceId)
        {
            if (practiceId == 0)
            {
                TempData["error"] = "The practice id provided is invalid.";
                return RedirectToAction("Index"); 
            }

            Practice practice = _context.Practices.Where(p => p.Id == practiceId).FirstOrDefault();

            if (practice == null)
            {
                TempData["error"] = "The practice provided is invalid.";
                return RedirectToAction("Index"); 
            }

            var dbQueries = (from p in _context.Practices
                             join a in _context.Attendances
                             on p.Id equals a.PracticeId
                             join wi in _context.WorkoutInformation
                             on p.Id equals wi.PracticeId
                             join wt in _context.WorkoutTypes
                             on wi.WorkoutTypeId equals wt.Id
                             join aspnetusers in _context.ApplicationUsers
                             on wi.RunnerId equals aspnetusers.Id
                             where p.Id == practiceId && a.IsPresent && wi.RunnerId == a.RunnerId && p.PracticeIsInProgress == false
                             select new
                             {
                                 p.PracticeStartTimeAndDate,
                                 p.PracticeEndTimeAndDate,
                                 p.PracticeLocation,
                                 wt.WorkoutName,
                                 wi.PracticeId,
                                 wi.RunnerId,
                                 aspnetusers.Id,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName
                             });

            SelectedViewModel selectedViewModel = new SelectedViewModel
            {
                PracticeStartTime =  TimeOnly.FromDateTime(practice.PracticeStartTimeAndDate),
                PracticeEndTime = TimeOnly.FromDateTime(practice.PracticeEndTimeAndDate),
                PracticeLocation = practice.PracticeLocation,
                PracticeId = practice.Id
            };

            SelectedViewModelHelper viewModelHelper = new SelectedViewModelHelper();

            if (dbQueries != null && dbQueries.Count() > 0)
            {
                int currentRow = 1;
                bool firstLoop = true;
                string currentRunnerId = "";
                int dbQueriesRecordCount = dbQueries.Count();

                foreach (var dbQuery in dbQueries)
                {
                    if (firstLoop)
                    {
                        viewModelHelper.RunnerName = $"{dbQuery.FirstName} {dbQuery.LastName}";
                        viewModelHelper.RunnerId = dbQuery.RunnerId;
                        viewModelHelper.PracticeId = dbQuery.PracticeId;
                    }

                    if (currentRunnerId != dbQuery.RunnerId && firstLoop == false)
                    {
                        selectedViewModel.SelectedViewModelHelpers?.Add(viewModelHelper);

                        viewModelHelper = new SelectedViewModelHelper
                        {
                            RunnerName = $"{dbQuery.FirstName} {dbQuery.LastName}",
                            RunnerId = dbQuery.RunnerId,
                            PracticeId = dbQuery.PracticeId,
                        };
                    }

                    viewModelHelper.Workouts?.Add(dbQuery.WorkoutName);

                    firstLoop = false;
                    currentRunnerId = dbQuery.RunnerId;

                    if (currentRow == dbQueriesRecordCount)
                    {
                        selectedViewModel.SelectedViewModelHelpers?.Add(viewModelHelper);
                    }

                    currentRow++;
                }
            }

             
            return View(selectedViewModel); 
            
        }

        public IActionResult SelectedRunnerHistory(string runnerId)
        {
            if (runnerId == null)
            {
                return RedirectToAction(); // send to an error page in the future
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
                return RedirectToAction(); // send to a page that states the runner has no practice history in the future
            }

            int practiceId = dbQueries.FirstOrDefault().PracticeId;

            if (practiceId == null)
            {
                return RedirectToAction(); // send to an invalid practice id page (null value)
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

            return RedirectToAction(); // send to an error page in the future
        }
    }
}
