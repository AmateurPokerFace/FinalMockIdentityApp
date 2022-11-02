using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.Delete;
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
                return RedirectToAction(); // send to an error page in the future (invalid id provided)
            }

            Practice practice = _context.Practices.Where(p => p.Id == practiceId).FirstOrDefault();

            if (practice == null)
            {
                return RedirectToAction(); // send to an error page in the future
            }

            var dbQueries = (from w in _context.WorkoutInformation
                             join aspnetusers in _context.ApplicationUsers
                             on w.RunnerId equals aspnetusers.Id
                             join workoutTypes in _context.WorkoutTypes
                             on w.WorkoutTypeId equals workoutTypes.Id
                             join p in _context.Practices
                             on w.PracticeId equals p.Id
                             where w.PracticeId == practiceId
                             select new
                             {
                                 w.RunnerId,
                                 w.PracticeId,
                                 workoutTypes.WorkoutName,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                                 p.PracticeStartTimeAndDate,
                                 p.PracticeEndTimeAndDate,
                                 p.PracticeLocation
                             });


            if (dbQueries.Count() > 0 == false)
            {
                return RedirectToAction(); // send to an error page in the future
            }

            List<SelectedViewModel> selectedViewModels = new List<SelectedViewModel>();
            
            SelectedViewModel selectedViewModel = new SelectedViewModel ();

            string runnerId = dbQueries.FirstOrDefault().RunnerId;

            bool multipleLoops = false; // will be used to add the last object instance to the selectedViewModels List

            foreach (var dbQuery in dbQueries)
            {
                if (runnerId != dbQuery.RunnerId) // will not execute on first loop. runnerId was populated using dbQueriesFirstOrDefault().RunnerId.
                {
                    selectedViewModels.Add(selectedViewModel);
                    multipleLoops = true; 
                    selectedViewModel = new SelectedViewModel();
                }

                selectedViewModel.PracticeId = dbQuery.PracticeId;
                selectedViewModel.RunnerId = dbQuery.RunnerId;
                selectedViewModel.RunnersName = $"{dbQuery.FirstName} {dbQuery.LastName}";
                selectedViewModel.PracticeWorkouts.Add(dbQuery.WorkoutName);
                selectedViewModel.PracticeStartTime = TimeOnly.FromDateTime(dbQuery.PracticeStartTimeAndDate);
                selectedViewModel.PracticeEndingTime = TimeOnly.FromDateTime(dbQuery.PracticeEndTimeAndDate);
                selectedViewModel.PracticeLocation = dbQuery.PracticeLocation == null ? "" : dbQuery.PracticeLocation;
                runnerId = dbQuery.RunnerId;

            }

            if (multipleLoops)
            {
                selectedViewModels.Add(selectedViewModel);
            }
             
            return View(selectedViewModels); 
            
        }

        public IActionResult SelectedRunnerHistory(string runnerId)
        {
            if (runnerId == null)
            {
                return RedirectToAction(); // send to an error page in the future
            }

            SelectedRunnerHistoryViewModel selectedRunnerHistoryViewModel = new SelectedRunnerHistoryViewModel();

            return View();
        }
    }
}
