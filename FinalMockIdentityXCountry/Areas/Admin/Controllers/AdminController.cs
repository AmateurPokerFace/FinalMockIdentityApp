using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.Utilities;
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

                    if (waitingForApprovalListVm != null)
                    {
                        ApproveUserCheckboxOptions approveUserCheckboxOptions = new ApproveUserCheckboxOptions 
                        {
                            UserId = dbQuery.Id,
                            RoleId = dbQuery.RoleId
                        };

                        waitingForApprovalListVm.SelectedApproveUserCheckboxOptions?.Add(approveUserCheckboxOptions);
                       
                        waitingForApprovalListViewModels.Add(waitingForApprovalListVm);
                    } 
                }

                if (waitingForApprovalListViewModels != null && waitingForApprovalListViewModels.Count > 0)
                {
                    return View(waitingForApprovalListViewModels);
                }
                
            }

            TempData["error"] = "There are no current users waiting for approval";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RejectUsers()
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
                List<WaitingForApprovalListViewModel> rejectUsersViewModel = new List<WaitingForApprovalListViewModel>();

                foreach (var dbQuery in dbQueries)
                {
                    WaitingForApprovalListViewModel rejectUsersListVm = new WaitingForApprovalListViewModel
                    {
                        Name = $"{dbQuery.FirstName} {dbQuery.LastName}",
                        UserName = dbQuery.UserName,
                        UserId = dbQuery.Id,
                        RoleId = dbQuery.RoleId,
                    };

                    if (rejectUsersListVm != null)
                    {
                        ApproveUserCheckboxOptions approveUserCheckboxOptions = new ApproveUserCheckboxOptions
                        {
                            UserId = dbQuery.Id,
                            RoleId = dbQuery.RoleId
                        };

                        rejectUsersListVm.SelectedApproveUserCheckboxOptions?.Add(approveUserCheckboxOptions);

                        rejectUsersViewModel.Add(rejectUsersListVm);
                    }
                }

                if (rejectUsersViewModel != null && rejectUsersViewModel.Count > 0)
                {
                    return View(rejectUsersViewModel);
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
         
        public IActionResult EditRunnerName(string runnerId)
        {
            if (runnerId == null)
            {
                TempData["error"] = "Invalid runner provided";
                return RedirectToAction("Index");
            }

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
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                OldName = $"{applicationUser.FirstName} {applicationUser.LastName}"
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

                applicationUser.FirstName = editRunnerNameViewModel.FirstName;
                applicationUser.LastName = editRunnerNameViewModel.LastName;

                _context.ApplicationUsers.Update(applicationUser);
                _context.SaveChanges();

                TempData["success"] = "The runner's Name was updated successfully";
                
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
                UserName = applicationUser.UserName,
                OldUserName = $"{applicationUser.FirstName} {applicationUser.LastName}"
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

                applicationUser.UserName = editRunnerUserNameViewModel.UserName;
                applicationUser.NormalizedUserName = editRunnerUserNameViewModel.UserName.ToUpper();

                _context.ApplicationUsers.Update(applicationUser);
                _context.SaveChanges();

                TempData["success"] = "The runner's UserName was updated successfully";

                return RedirectToAction(nameof(CurrentRunnersList));
            }

            TempData["error"] = "Invalid runner provided";
            return RedirectToAction("Index");
        }

        public IActionResult ChangeUserPassword(string userId)
        {
            ApplicationUser applicationUser = _context.ApplicationUsers.Find(userId);
            if (applicationUser == null)
            {
                TempData["error"] = "Invalid user provided";
                return RedirectToAction("Index");
            }

            ChangeUserPasswordViewModel changeUserPasswordViewModel = new ChangeUserPasswordViewModel
            {
                UsersName = $"{applicationUser.FirstName} {applicationUser.LastName}",
                UserId = applicationUser.Id
            };
            return View(changeUserPasswordViewModel);
        }

        [HttpPost]
        public IActionResult ChangeUserPassword(ChangeUserPasswordViewModel changeUserPasswordViewModel)
        {
            var user = _context.ApplicationUsers.Find(changeUserPasswordViewModel.UserId);

            if (user == null)
            {
                TempData["error"] = "Invalid user provided";
                return RedirectToAction("Index");
            }

            if (changeUserPasswordViewModel.NewPassword == null)
            {
                TempData["error"] = "Invalid password provided";
                return RedirectToAction("Index");
            }

            var newPassword = _userManager.PasswordHasher.HashPassword(user, changeUserPasswordViewModel.NewPassword);
            user.PasswordHash = newPassword;

            _context.ApplicationUsers.Update(user);
            _context.SaveChanges();

            TempData["success"] = "Password updated successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddToRunner(List<WaitingForApprovalListViewModel> waitingForApprovalListViewModels)
        {
            if (waitingForApprovalListViewModels == null)
            {
                TempData["error"] = "Invalid users provided.";
                return RedirectToAction("Index");
            }
             
            int approvedRunners = 0;

            foreach (var waitingForApprovalVm in waitingForApprovalListViewModels)
            {
                foreach (var selectedCheckboxOption in waitingForApprovalVm.SelectedApproveUserCheckboxOptions.Where(i => i.IsSelected))
                {
                    var user = await _userManager.FindByIdAsync(selectedCheckboxOption.UserId);

                    if (user != null)
                    {
                        bool userFoundInRoleNotAssigned = false;

                        if (await _userManager.IsInRoleAsync(user, StaticDetails.Role_Not_Assigned)) // remove the user from the role "Waiting for approval" if true
                        {
                            await _userManager.RemoveFromRoleAsync(user, StaticDetails.Role_Not_Assigned);
                            userFoundInRoleNotAssigned = true;
                        }

                        if (userFoundInRoleNotAssigned)
                        {
                            await _userManager.AddToRoleAsync(user, "Runner");
                            approvedRunners++;
                        }
                    }
                } 
            }

            if (approvedRunners == 0)
            {
                TempData["error"] = "Zero runners were approved.";
                return RedirectToAction("Index");
            }
            else 
            {
                TempData["success"] = approvedRunners == 1 ? $"{approvedRunners} user was approved" : $"{approvedRunners} users were approved";
                return RedirectToAction("Index");
            }

        }
         
        public IActionResult SelectRoleForApprovedUser(string userId) 
        {
            if (userId == null)
            {
                TempData["error"] = $"Invalid user provided";
                return RedirectToAction("Index");
            }



            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsersInRejectedList(List<WaitingForApprovalListViewModel> rejectUsersViewModel)
        {
            if (rejectUsersViewModel == null)
            {
                TempData["error"] = "Invalid users provided.";
                return RedirectToAction("Index");
            }

            int rejectedUsers = 0;

            foreach (var rejectUsersVm in rejectUsersViewModel)
            {
                foreach (var selectedCheckboxOption in rejectUsersVm.SelectedApproveUserCheckboxOptions.Where(i => i.IsSelected))
                {
                    var user = await _userManager.FindByIdAsync(selectedCheckboxOption.UserId);

                    if (user != null)
                    { 

                        if (await _userManager.IsInRoleAsync(user, StaticDetails.Role_Not_Assigned)) // remove the user from the role "Waiting for approval" if true
                        {
                            //await _userManager.RemoveFromRoleAsync(user, StaticDetails.Role_Not_Assigned);
                            await _userManager.DeleteAsync(user);
                            rejectedUsers++;
                        } 
                    }
                }
            }

            if (rejectedUsers == 0)
            {
                TempData["warning"] = "Zero users were rejected.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["success"] = rejectedUsers == 1 ? $"{rejectedUsers} user was rejected and deleted from the database permanently" : $"{rejectedUsers} users were rejected and deleted from the database permanently";
                return RedirectToAction("Index");
            }
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
