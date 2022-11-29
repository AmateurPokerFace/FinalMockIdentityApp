using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.Utilities;
using FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.CurrentPracticeController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class CurrentPracticeController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question

        public CurrentPracticeController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CurrentPractices()
        {
            IEnumerable<Practice> practices = _context.Practices.Where(p => p.PracticeIsInProgress);
            if (practices == null || practices.Count() == 0)
            {
               return View("NoPracticesInProgress");
            }

            return View(practices);
        }

        public IActionResult NoPracticesInProgress()
        {
            return View();
        }

        public IActionResult EditWorkouts()
        {
            return View();
        }

        public IActionResult Current(int currentPracticeId)
        {
            if (currentPracticeId != 0)
            {
                Practice practice = _context.Practices.Find(currentPracticeId);
                if (practice == null) // check to see if a practice exists with the provided id in the database
                {
                    TempData["error"] = "The practice doesn't exist in the database. Please provide a valid practice id.";
                    return RedirectToAction("Index");
                }
                CurrentViewModel currentViewModel = new CurrentViewModel { practiceId = currentPracticeId };

                var dbQueries = (from a in _context.Attendances
                                 join aspnetusers in _context.ApplicationUsers
                                 on a.RunnerId equals aspnetusers.Id
                                 join practices in _context.Practices
                                 on a.PracticeId equals practices.Id
                                 where a.PracticeId == currentPracticeId && a.IsPresent && practices.PracticeIsInProgress
                                 select new
                                 {
                                     aspnetusers.FirstName,
                                     aspnetusers.LastName,
                                     practices.PracticeStartTimeAndDate,
                                     practices.PracticeLocation,
                                 });
                  
                if (dbQueries != null && dbQueries.Count() > 0)
                {
                    foreach (var dbQuery in dbQueries)
                    {
                        currentViewModel.PracticeLocation = dbQuery.PracticeLocation;
                        currentViewModel.PracticeStartTimeAndDate = dbQuery.PracticeStartTimeAndDate;
                        currentViewModel.RunnerName = $"{dbQuery.FirstName} {dbQuery.LastName}";
                        currentViewModel.Runners?.Add(currentViewModel.RunnerName);
                    }

                    return View(currentViewModel);
                }
                else
                {
                    currentViewModel.PracticeLocation = practice.PracticeLocation;
                    currentViewModel.PracticeStartTimeAndDate = practice.PracticeStartTimeAndDate; 
                    return View(currentViewModel);
                }
                
                
            }

            TempData["error"] = "Invalid practice id provided";
            return RedirectToAction("Index");
        }

        public IActionResult Attendance(int practiceId)
        {
            if (practiceId == 0)
            {
                TempData["error"] = "Invalid practice id provided";
                return RedirectToAction("Index"); 
            }
            Practice practice = _context.Practices.Find(practiceId);
           
            if (practice == null)
            {
                TempData["error"] = "Invalid practice id provided";
                return RedirectToAction("Index");
            }

            var dbQueries = (from a in _context.Attendances
                             join aspnetusers in _context.ApplicationUsers
                             on a.RunnerId equals aspnetusers.Id
                             join p in _context.Practices
                             on a.PracticeId equals p.Id
                             where a.IsPresent && a.HasBeenSignedOut == false && a.PracticeId == practiceId
                             select new
                             {
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                                 a.RunnerId,
                                 p.PracticeLocation,
                                 p.PracticeStartTimeAndDate,
                                 p.Id
                             });

            if (dbQueries.Count() < 1 || dbQueries == null)
            {
                if (practice == null)
                {
                    TempData["error"] = "Invalid practice provided";
                    return RedirectToAction("Index"); // send to an invalid page in the future
                }
                else
                {
                    return RedirectToAction("EmptyAttendance",new {practiceId = practiceId});
                }
                
            }

            AttendanceViewModel attendanceViewModel = new AttendanceViewModel
            {
                PracticeLocation = practice?.PracticeLocation == null ? " " : dbQueries.FirstOrDefault()?.PracticeLocation,
                PracticeStartTimeAndDate = practice.PracticeStartTimeAndDate,
                PracticeId = practice.Id
            };

            foreach (var dbQuery in dbQueries)
            {
                AttendanceViewModelHelper attendanceViewModelHelper = new AttendanceViewModelHelper
                {
                    PracticeId = dbQuery.Id,
                    RunnerId = dbQuery.RunnerId,
                    Runner = $"{dbQuery?.FirstName} {dbQuery?.LastName}"
                };

                attendanceViewModel.AttendanceViewModelHelpers?.Add(attendanceViewModelHelper);
            }

            if (attendanceViewModel.AttendanceViewModelHelpers != null && attendanceViewModel.AttendanceViewModelHelpers.Count() > 0)
            {
                return View(attendanceViewModel);
            }

            return RedirectToAction("Index"); // send to an error page in the future
        }

        public IActionResult EmptyAttendance(int practiceId)
        {
            Practice practice = _context.Practices.Where(p => p.PracticeIsInProgress && p.Id == practiceId).FirstOrDefault();
            if (practice == null)
            {
                TempData["error"] = "Invalid practice id provided";
                return RedirectToAction("Index");
            }

            EmptyAttendanceViewModel model = new EmptyAttendanceViewModel(practiceId);

            return View(model);
        }

        public IActionResult AddRunnerToCurrentPractice(int practiceId)
        {
            if (practiceId == 0)
            {
                TempData["error"] = "Invalid practice id provided";
                return RedirectToAction("CurrentPractices");
            }

            Practice practice = _context.Practices.Where(p => p.PracticeIsInProgress && p.Id == practiceId).FirstOrDefault();
            
            if (practice == null)
            {
                TempData["error"] = "Invalid practice id provided. Practice not found.";
                return RedirectToAction("CurrentPractices");
            }

            var dbQueries = (from a in _context.Attendances
                             join p in _context.Practices
                             on a.PracticeId equals p.Id
                             join aspnetusers in _context.ApplicationUsers
                             on a.RunnerId equals aspnetusers.Id
                             where a.IsPresent == false && p.Id == practiceId
                             select new
                             {
                                 p.PracticeStartTimeAndDate,
                                 p.PracticeLocation,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                                 a.Id,
                                 a.RunnerId
                             });

            if (dbQueries == null || dbQueries.Count() < 1)
            {
                TempData["error"] = "There are no runners marked as absent.";
                return RedirectToAction("Index");
            }

            List<AddRunnerToCurrentPracticeViewModel> models = new List<AddRunnerToCurrentPracticeViewModel>();
            
            foreach (var dbQuery in dbQueries)
            {
                AddRunnerToCurrentPracticeViewModel model = new AddRunnerToCurrentPracticeViewModel
                {
                    Name = $"{dbQuery.FirstName} {dbQuery.LastName}",
                    AttendanceId = dbQuery.Id,
                    RunnerId = dbQuery.RunnerId,
                    PracticeLocation = dbQuery.PracticeLocation,
                    PracticeStartTimeAndDate = dbQuery.PracticeStartTimeAndDate
                };

                if (model != null)
                {
                    AddRunnerToCurrentPracticeChkboxOptionViewModel modelChkboxOption = new AddRunnerToCurrentPracticeChkboxOptionViewModel
                    { 
                        AttendanceId = model.AttendanceId,
                        RunnerId = model.RunnerId
                    };

                    model.SelectedCheckboxOptions?.Add(modelChkboxOption);
                    models.Add(model);
                }
            }

            if (models != null && models.Count > 0)
            {
                return View(models);
            }

            TempData["error"] = "There are no runners marked as absent.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult>AddRunnerToCurrentPractice(List<AddRunnerToCurrentPracticeViewModel> models) 
        {
            if (models == null)
            {
                TempData["error"] = "Invalid runners provided.";
                return RedirectToAction("Index");
            }

            int updatedRunners = 0;

            foreach (var model in models)
            {
                if (model.SelectedCheckboxOptions != null)
                {
                    foreach (var selectedCheckboxOption in model.SelectedCheckboxOptions.Where(i => i.IsSelected))
                    {
                        var runner = await _context.Attendances.FindAsync(selectedCheckboxOption.AttendanceId);

                        if (runner != null)
                        {
                            runner.IsPresent = true;
                            _context.Attendances.Update(runner);
                            updatedRunners++;
                        }
                    }
                }
            }

            if (updatedRunners == 0)
            {
                TempData["error"] = "An error occured. No runners were added to the attendance";
                return RedirectToAction("Index");
            }
            else
            {
                await _context.SaveChangesAsync();
                TempData["success"] = updatedRunners == 1 ? $"{updatedRunners} runner was added to the practice" : $"{updatedRunners} runners were added to the practice";
                return RedirectToAction("Index");
            }
        }

        public IActionResult SignRunnerOut(string runnerId, int practiceId)
        {
            if (practiceId == 0)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            } 

            var dbQuery = (from a in _context.Attendances
                           join p in _context.Practices
                           on a.PracticeId equals p.Id
                           join aspnetusers in _context.ApplicationUsers
                           on a.RunnerId equals aspnetusers.Id
                           where a.RunnerId == runnerId && p.Id == practiceId
                           select new
                           {
                               a.Id,
                               a.RunnerId,
                               p.PracticeLocation,
                               p.PracticeStartTimeAndDate,
                               aspnetusers.FirstName,
                               aspnetusers.LastName
                           }).FirstOrDefault();

            if (dbQuery == null)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            SignRunnerOutViewModel signRunnerOutViewModel = new SignRunnerOutViewModel 
            {
                AttendanceId = dbQuery.Id,
                PracticeLocation = dbQuery.PracticeLocation,
                PracticeStartDateTime = dbQuery.PracticeStartTimeAndDate,
                RunnersName = $"{dbQuery.FirstName} {dbQuery.LastName}"
            };

            

            return View(signRunnerOutViewModel);
        }

        [HttpPost]
        public IActionResult SignRunnerOut(int attendanceId)
        {
            if (attendanceId == 0)
            {
                return RedirectToAction("Index"); // send to an error page in the future (invalid id provided)
            }

            Attendance attendance = _context.Attendances.Find(attendanceId);

            if (attendance == null)
            {
                return RedirectToAction("Index"); // send to an error page in the future
            }

            attendance.HasBeenSignedOut = true;
            _context.Attendances.Update(attendance);
            _context.SaveChanges();

            return RedirectToAction(nameof(CurrentPractices)); // Send to a success page in the future
        }

    }
}
