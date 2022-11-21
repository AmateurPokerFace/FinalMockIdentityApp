using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinalMockIdentityXCountry.Areas.Admin.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question

        public AdminController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Master Admin")]
        public IActionResult MasterAdminPanel()
        {
            return View();
        }

        [Authorize(Roles = "Master Admin, Coach")]
        public IActionResult AdminPanel()
        {
            return View();
        }

        public IActionResult WaitingForApprovalList()
        {
            var dbQueries = (from aspnetusers in _context.ApplicationUsers
                             join userroles in _context.UserRoles
                             on aspnetusers.Id equals userroles.UserId
                             join roles in _context.Roles
                             on userroles.RoleId equals roles.Id
                             where roles.Name.ToLower() == "waiting for approval"
                             select new
                             {
                                 aspnetusers.UserName,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                                 aspnetusers.Id,
                                 userroles.RoleId,
                             });

            if (dbQueries != null && dbQueries.Count() > 0)
            {
                List<WaitingForApprovalListViewModel> waitingForApprovalListViewModels = new List<WaitingForApprovalListViewModel>();

                foreach (var dbQuery in dbQueries)
                {
                    WaitingForApprovalListViewModel waitingForApprovalListVm = new WaitingForApprovalListViewModel
                    {
                        Name = $"{dbQuery.FirstName} {dbQuery.LastName}",
                        UserName = dbQuery.UserName,
                        UserId = dbQuery.Id,
                        RoleId = dbQuery.RoleId,
                    }; 

                    waitingForApprovalListViewModels.Add(waitingForApprovalListVm);
                }

                if (waitingForApprovalListViewModels != null && waitingForApprovalListViewModels.Count > 0)
                {
                    return View(waitingForApprovalListViewModels);
                }
                
            }

            TempData["error"] = "There are no current users waiting for approval";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult CurrentUsersList()
        {
            return View();
        }

        public IActionResult CurrentRunnersList()
        {
            var dbQueries = (from aspnetusers in _context.ApplicationUsers
                             join userroles in _context.UserRoles
                             on aspnetusers.Id equals userroles.UserId
                             join roles in _context.Roles
                             on userroles.RoleId equals roles.Id
                             where roles.Name.ToLower() == "runner"
                             select new
                             {
                                 aspnetusers.UserName,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                                 aspnetusers.Id,
                                 userroles.RoleId,
                             });

            if (dbQueries != null && dbQueries.Count() > 0)
            {
                List<CurrentRunnersListViewModel> currentRunnersListViewModels = new List<CurrentRunnersListViewModel>();

                foreach (var dbQuery in dbQueries)
                {
                    CurrentRunnersListViewModel currentRunnerVm = new CurrentRunnersListViewModel
                    {
                        Name = $"{dbQuery.FirstName} {dbQuery.LastName}",
                        UserName = dbQuery.UserName,
                        UserId = dbQuery.Id,
                        RoleId = dbQuery.RoleId,
                    };

                    currentRunnersListViewModels.Add(currentRunnerVm);
                }

                if (currentRunnersListViewModels != null && currentRunnersListViewModels.Count > 0)
                {
                    return View(currentRunnersListViewModels);
                }

            }

            TempData["error"] = "There are no runners in the database.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditRunnerData(string runnerId)
        {
            if (string.IsNullOrEmpty(runnerId))
            {
                TempData["error"] = "Invalid Runner Provided";
                return RedirectToAction("Index");
            }

            var dbQuery = (from aspnetusers in _context.ApplicationUsers
                             join userroles in _context.UserRoles
                             on aspnetusers.Id equals userroles.UserId
                             join roles in _context.Roles
                             on userroles.RoleId equals roles.Id
                             where roles.Name.ToLower() == "runner" && aspnetusers.Id == runnerId
                             select new
                             {
                                 aspnetusers.UserName,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                                 aspnetusers.Id,
                                 userroles.RoleId,
                             }).FirstOrDefault();

            if (dbQuery != null)
            {
                EditRunnerDataViewModel editRunnerDataViewModel = new EditRunnerDataViewModel
                {
                    Name = $"{dbQuery.FirstName} {dbQuery.LastName}",
                    UserName = dbQuery.UserName,
                    UserId = dbQuery.Id,
                    RoleId = dbQuery.RoleId,
                };
                return View(editRunnerDataViewModel);
            }

            TempData["error"] = "Invalid Runner Provided";
            return RedirectToAction("Index");
        }

        public IActionResult EditRunnerFirstName(string runnerId) 
        {
            ApplicationUser applicationUser = _context.ApplicationUsers.Find(runnerId);

            if (applicationUser == null)
            {
                TempData["error"] = "Invalid runner provided";
                return RedirectToAction("Index");
            }

            var runnerRole = (from userroles in _context.UserRoles
                              join roles in _context.Roles
                              on userroles.RoleId equals roles.Id
                              where roles.Name.ToLower() == "runner" && applicationUser.Id == runnerId
                              select new
                              { }).FirstOrDefault();

            if (runnerRole == null)
            {
                TempData["error"] = "Invalid user provided. User does not have the role of runner";
                return RedirectToAction("Index");
            }

            EditRunnerFirstNameViewModel editRunnerFirstNameViewModel = new EditRunnerFirstNameViewModel
            {
                RunnerId = runnerId,
                CurrentName = $"{applicationUser.FirstName} {applicationUser.LastName}"
            };

            return View(editRunnerFirstNameViewModel);
        }


        [HttpPost]
        public IActionResult EditRunnerFirstName(EditRunnerFirstNameViewModel editRunnerFirstNameViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = _context.ApplicationUsers.Find(editRunnerFirstNameViewModel.RunnerId);
                if (applicationUser == null)
                {
                    TempData["error"] = "Invalid runner provided";
                    return RedirectToAction("Index");
                }

                applicationUser.FirstName = editRunnerFirstNameViewModel.NewFirstName;

                _context.ApplicationUsers.Update(applicationUser);
                _context.SaveChanges();

                TempData["success"] = "The runners' First Name was updated successfully";

                return RedirectToAction(nameof(CurrentRunnersList));
            }

            TempData["error"] = "Invalid runner provided";
            return RedirectToAction("Index");
        }

        public IActionResult EditRunnerLastName(string runnerId)
        {
            ApplicationUser applicationUser = _context.ApplicationUsers.Find(runnerId);

            if (applicationUser == null)
            {
                TempData["error"] = "Invalid runner provided";
                return RedirectToAction("Index");
            }

            var runnerRole = (from userroles in _context.UserRoles
                              join roles in _context.Roles
                              on userroles.RoleId equals roles.Id
                              where roles.Name.ToLower() == "runner" && applicationUser.Id == runnerId
                              select new
                              { }).FirstOrDefault();

            if (runnerRole == null)
            {
                TempData["error"] = "Invalid user provided. User does not have the role of runner";
                return RedirectToAction("Index");
            }

            EditRunnerLastNameViewModel editRunnerLastNameViewModel = new EditRunnerLastNameViewModel
            {
                RunnerId = runnerId,
                CurrentName = $"{applicationUser.FirstName} {applicationUser.LastName}"
            };

            return View(editRunnerLastNameViewModel);
        }

        [HttpPost]
        public IActionResult EditRunnerLastName(EditRunnerLastNameViewModel editRunnerLastNameViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = _context.ApplicationUsers.Find(editRunnerLastNameViewModel.RunnerId);
                if (applicationUser == null)
                {
                    TempData["error"] = "Invalid runner provided";
                    return RedirectToAction("Index");
                }
                 
                applicationUser.LastName = editRunnerLastNameViewModel.NewLastName;

                _context.ApplicationUsers.Update(applicationUser);
                _context.SaveChanges();

                TempData["success"] = "The runners' Last Name was updated successfully";

                return RedirectToAction(nameof(CurrentRunnersList));
            }

            TempData["error"] = "Invalid runner provided";
            return RedirectToAction("Index");
        }

        public IActionResult EditRunnerName(string runnerId)
        {
            ApplicationUser applicationUser = _context.ApplicationUsers.Find(runnerId);

            if (applicationUser == null)
            {
                TempData["error"] = "Invalid runner provided";
                return RedirectToAction("Index");
            }

            var runnerRole = (from userroles in _context.UserRoles
                              join roles in _context.Roles
                              on userroles.RoleId equals roles.Id
                              where roles.Name.ToLower() == "runner" && applicationUser.Id == runnerId
                              select new 
                              {}).FirstOrDefault();

            if (runnerRole == null)
            {
                TempData["error"] = "Invalid user provided. User does not have the role of runner";
                return RedirectToAction("Index");
            }

            EditRunnerNameViewModel editRunnerNameViewModel = new EditRunnerNameViewModel
            {
                RunnerId = runnerId,
                CurrentName = $"{applicationUser.FirstName} {applicationUser.LastName}"
            };

            return View(editRunnerNameViewModel);
        }

        [HttpPost]
        public IActionResult EditRunnerName(EditRunnerNameViewModel editRunnerNameViewModel)
        {
            if (ModelState.IsValid) 
            {
                ApplicationUser applicationUser = _context.ApplicationUsers.Find(editRunnerNameViewModel.RunnerId);
                if (applicationUser == null)
                {
                    TempData["error"] = "Invalid runner provided";
                    return RedirectToAction("Index");
                }

                applicationUser.FirstName = editRunnerNameViewModel.NewFirstName;
                applicationUser.LastName = editRunnerNameViewModel.NewLastName;

                _context.ApplicationUsers.Update(applicationUser);
                _context.SaveChanges();

                TempData["success"] = "The runners' Full Name was updated successfully";
                
                return RedirectToAction(nameof(CurrentRunnersList));
            }

            TempData["error"] = "Invalid runner provided";
            return RedirectToAction("Index");
        }

        public IActionResult EditRunnerUserName(string runnerId)
        {
            ApplicationUser applicationUser = _context.ApplicationUsers.Find(runnerId);

            if (applicationUser == null)
            {
                TempData["error"] = "Invalid runner provided";
                return RedirectToAction("Index");
            }

            var runnerRole = (from userroles in _context.UserRoles
                              join roles in _context.Roles
                              on userroles.RoleId equals roles.Id
                              where roles.Name.ToLower() == "runner" && applicationUser.Id == runnerId
                              select new
                              { }).FirstOrDefault();

            if (runnerRole == null)
            {
                TempData["error"] = "Invalid user provided. User does not have the role of runner";
                return RedirectToAction("Index");
            }

            EditRunnerUserNameViewModel editRunnerUserNameViewModel = new EditRunnerUserNameViewModel
            {
                RunnerId = runnerId,
                CurrentName = $"{applicationUser.FirstName} {applicationUser.LastName}"
            };

            return View(editRunnerUserNameViewModel);
        }

        [HttpPost]
        public IActionResult EditRunnerUserName(EditRunnerUserNameViewModel editRunnerUserNameViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = _context.ApplicationUsers.Find(editRunnerUserNameViewModel.RunnerId);
                if (applicationUser == null)
                {
                    TempData["error"] = "Invalid runner provided";
                    return RedirectToAction("Index");
                }

                applicationUser.UserName = editRunnerUserNameViewModel.NewUserName;
                applicationUser.NormalizedUserName = editRunnerUserNameViewModel.NewUserName.ToUpper();

                _context.ApplicationUsers.Update(applicationUser);
                _context.SaveChanges();

                TempData["success"] = "The runners' UserName was updated successfully";

                return RedirectToAction(nameof(CurrentRunnersList));
            }

            TempData["error"] = "Invalid runner provided";
            return RedirectToAction("Index");
        }
        public IActionResult RemoveCoach()
        {
            return View();
        }

        public IActionResult AddCoach() 
        {
            return View();
        }
    }
}
