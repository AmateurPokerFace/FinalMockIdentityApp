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
            var userClaimsIdentity = (ClaimsIdentity)User.Identity;
            var userClaim = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<SelectAPracticeViewModel> selectAPracticeViewModels = new List<SelectAPracticeViewModel>();

            if (userClaim != null)
            {
                var dbQueries = (from w in _context.WorkoutInformation
                                 join practices in _context.Practices
                                 on w.PracticeId equals practices.Id
                                 where practices.PracticeIsInProgress && w.RunnerId == userClaim.Value
                                 select new
                                 {
                                     w.Id,
                                     w.PracticeId,
                                     w.RunnerId, 
                                     w.DataWasLogged,
                                     practices.PracticeStartTimeAndDate,
                                     practices.PracticeLocation
                                 });

                foreach (var dbQuery in dbQueries)
                {
                    SelectAPracticeViewModel selectedVm = new SelectAPracticeViewModel
                    {
                        WorkoutInformationId = dbQuery.Id,
                        PracticeId = dbQuery.PracticeId,
                        RunnerId = dbQuery.RunnerId,
                        DataWasLogged = dbQuery.DataWasLogged,
                        PracticeStartTimeAndDate = dbQuery.PracticeStartTimeAndDate,
                        PracticeLocation = dbQuery.PracticeLocation
                    };

                    selectAPracticeViewModels.Add(selectedVm);
                    //currentPracticesViewModel.CurrentPracticesViewModelsHelper.Add(currentViewModel);
                };

                return View(selectAPracticeViewModels);
            }
                return RedirectToAction(); // send to an error page in the future
        }

        public IActionResult LogData(int workoutInfoId, int practiceId)
        {

            if (workoutInfoId != 0 && practiceId != 0)
            {
                var userClaimsIdentity = (ClaimsIdentity)User.Identity;
                var userClaim = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                CurrentPracticesViewModel currentPracticesViewModel = new CurrentPracticesViewModel();

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
                        return RedirectToAction(); // Send to an error page in the future
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
                return View();
            }

            return RedirectToAction(); // Send to an error page in the future
            
        }

        [HttpPost]
        public IActionResult LogData(LogDataViewModel logDataViewModel)
        {
            if (logDataViewModel != null)
            {
                WorkoutInformation workoutInformation = _context.WorkoutInformation
                .Where(w => w.Id == logDataViewModel.WorkoutInformationId).FirstOrDefault();

                if (workoutInformation != null)
                {
                    workoutInformation.Distance = logDataViewModel.Distance;
                    workoutInformation.Pace = logDataViewModel.Pace;
                    workoutInformation.DataWasLogged = true;
                    
                    _context.Update(workoutInformation);
                    _context.SaveChanges();

                    return RedirectToAction("Index"); // send to success page in the future
                }

                return RedirectToAction(); // send to an error page in the future
            }

            return RedirectToAction(); // send to an error page in the future
        }

        public IActionResult EditLoggedData(int workoutInfoId, int practiceId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditLoggedData(int dummy)
        {
            return View();
        }
    }
}
