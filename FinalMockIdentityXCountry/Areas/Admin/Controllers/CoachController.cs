using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.Utilities;
using FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels.CoachControllerViewModels; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace FinalMockIdentityXCountry.Areas.Admin.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Admin")]
    public class CoachController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CoachController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CoachAdminPanel() 
        {
            var dbQueries = (from aspnetusers in _context.ApplicationUsers
                             join userroles in _context.UserRoles
                             on aspnetusers.Id equals userroles.UserId
                             join roles in _context.Roles
                             on userroles.RoleId equals roles.Id
                             where roles.Name.ToLower() != StaticDetails.Role_Master_Admin.ToLower() && roles.Name.ToLower() != StaticDetails.Role_Coach
                             select new
                             {
                                 aspnetusers.UserName,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                                 aspnetusers.Id,
                                 userroles.RoleId,
                                 roles.Name,
                             });


            CoachAdminPanelViewModel coachAdminPanelViewModel = new CoachAdminPanelViewModel { CoachAdminPanelRole = "" };

            ApplicationUser adminUser = _context.ApplicationUsers.Find(_userManager.GetUserId(User));

            coachAdminPanelViewModel.CoachAdminPanelRole = _userManager.IsInRoleAsync(adminUser, StaticDetails.Role_Master_Admin).Result == true ? StaticDetails.Role_Master_Admin : StaticDetails.Role_Coach;

            foreach (var dbQuery in dbQueries)
            {
                CoachAdminPanelViewModelHelper masterAdminPanelViewModelHelper = new CoachAdminPanelViewModelHelper
                {
                    Name = $"{dbQuery.FirstName} {dbQuery.LastName}",
                    RoleId = dbQuery.RoleId,
                    UserId = dbQuery.Id,
                    UserName = dbQuery.UserName,
                    UserRole = dbQuery.Name
                };

                coachAdminPanelViewModel.CoachAdminPanelViewModelViewHelpers?.Add(masterAdminPanelViewModelHelper);
            }

            if (coachAdminPanelViewModel.CoachAdminPanelViewModelViewHelpers != null && coachAdminPanelViewModel.CoachAdminPanelViewModelViewHelpers.Count > 0)
            {
                coachAdminPanelViewModel.CoachAdminPanelViewModelViewHelpers = coachAdminPanelViewModel.CoachAdminPanelViewModelViewHelpers.OrderByDescending(r => r.UserRole).ToList();
            }

            return View(coachAdminPanelViewModel);
        }

        public IActionResult ChangeUserRole(string userId, string currentRoleName)
        {
            if (userId == null || currentRoleName == null)
            {
                TempData["error"] = "Invalid Query Data Provided";
                return RedirectToAction(nameof(CoachAdminPanel));
            }

            var dbQuery = (from userRole in _context.UserRoles
                           join aspnetroles in _context.Roles
                           on userRole.RoleId equals aspnetroles.Id
                           where userRole.UserId == userId
                           select new
                           {
                               aspnetroles.Name
                           }).FirstOrDefault();

            if (dbQuery == null)
            {
                TempData["error"] = "Invalid user provided.";
                return RedirectToAction("Index");
            }

            ChangeUserRoleViewModel changeUserRoleViewModel = new ChangeUserRoleViewModel();
            changeUserRoleViewModel.CurrentRoleName = dbQuery.Name;
            changeUserRoleViewModel.UserId = userId;

            var roles = _context.Roles.Where(r => r.Name.ToLower() != currentRoleName.ToLower());

            foreach (var role in roles)
            {
                changeUserRoleViewModel.RolesList?.Add(new SelectListItem { Text = role.Name, Value = role.Name });
            }

            return View(changeUserRoleViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeRole(ChangeUserRoleViewModel changeUserRoleViewModel)
        {
            if (changeUserRoleViewModel == null)
            {
                TempData["error"] = "Invalid Query Data Provided";
                return RedirectToAction(nameof(CoachAdminPanel));
            }

            var user = await _userManager.FindByIdAsync(changeUserRoleViewModel.UserId);

            if (user != null)
            {
                bool userFoundInRole = false;

                if (await _userManager.IsInRoleAsync(user, changeUserRoleViewModel.CurrentRoleName))
                {
                    await _userManager.RemoveFromRoleAsync(user, changeUserRoleViewModel.CurrentRoleName);
                    userFoundInRole = true;
                }

                if (userFoundInRole)
                {
                    try
                    {
                        await _userManager.AddToRoleAsync(user, changeUserRoleViewModel.SelectedRoleName);
                        TempData["success"] = "Role was updated successfully";
                        return RedirectToAction(nameof(CoachAdminPanel));
                    }
                    catch (Exception e)
                    {

                        TempData["error"] = $"{e.Message}";
                        return RedirectToAction(nameof(CoachAdminPanel));
                    }

                }
            }

            TempData["error"] = "Invalid Query Data Provided";
            return RedirectToAction(nameof(CoachAdminPanel));
        }

        public IActionResult EditUser(string userId)
        {
            if (userId == null)
            {
                TempData["error"] = "Invalid User provided";
                return RedirectToAction(nameof(CoachAdminPanel));
            }

            var dbQuery = (from aspnetusers in _context.ApplicationUsers
                           join userroles in _context.UserRoles
                           on aspnetusers.Id equals userroles.UserId
                           join roles in _context.Roles
                           on userroles.RoleId equals roles.Id
                           where roles.Name.ToLower() != StaticDetails.Role_Master_Admin.ToLower() && aspnetusers.Id == userId
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
                EditUserViewModel editUserViewModel = new EditUserViewModel
                {
                    Name = $"{dbQuery.FirstName} {dbQuery.LastName}",
                    UserName = dbQuery.UserName,
                    UserId = dbQuery.Id,
                    RoleId = dbQuery.RoleId,
                };

                return View(editUserViewModel);
            }

            TempData["error"] = "Invalid User Provided";
            return RedirectToAction(nameof(CoachAdminPanel));
        }

        public IActionResult EditUsersName(string userId)
        {
            if (userId == null)
            {
                TempData["error"] = "Invalid User provided";
                return RedirectToAction(nameof(CoachAdminPanel));
            }

            var dbQuery = (from aspnetusers in _context.ApplicationUsers
                           join userroles in _context.UserRoles
                           on aspnetusers.Id equals userroles.UserId
                           join roles in _context.Roles
                           on userroles.RoleId equals roles.Id
                           where roles.Name.ToLower() != StaticDetails.Role_Master_Admin.ToLower() && roles.Name.ToLower() != StaticDetails.Role_Coach.ToLower() && aspnetusers.Id == userId
                           select new
                           {
                               aspnetusers.FirstName,
                               aspnetusers.LastName,
                               aspnetusers.Id,
                           }).FirstOrDefault();

            if (dbQuery != null)
            {
                EditUsersNameViewModel editUserViewModel = new EditUsersNameViewModel
                {
                    OldName = $"{dbQuery.FirstName} {dbQuery.LastName}",
                    FirstName = dbQuery.FirstName,
                    LastName = dbQuery.LastName,
                    UserId = dbQuery.Id,
                };

                return View(editUserViewModel);
            }

            TempData["error"] = "Invalid User Provided";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EditUsersName(EditUsersNameViewModel editUserViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = _context.ApplicationUsers.Find(editUserViewModel.UserId);
                if (applicationUser == null)
                {
                    TempData["error"] = "Invalid User Provided";
                    return RedirectToAction("Index");
                }

                applicationUser.FirstName = editUserViewModel.FirstName;
                applicationUser.LastName = editUserViewModel.LastName;

                _context.ApplicationUsers.Update(applicationUser);
                _context.SaveChanges();

                TempData["success"] = "The user's Name was updated successfully";

                return RedirectToAction(nameof(CoachAdminPanel));
            }

            TempData["error"] = "Invalid User Provided";
            return RedirectToAction("Index");
        }

        public IActionResult EditUserUsersName(string userId)
        {
            ApplicationUser applicationUser = _context.ApplicationUsers.Find(userId);

            if (applicationUser == null)
            {
                TempData["error"] = "Invalid User Provided";
                return RedirectToAction("Index");
            }

            var dbQuery = (from aspnetusers in _context.ApplicationUsers
                           join userroles in _context.UserRoles
                           on aspnetusers.Id equals userroles.UserId
                           join roles in _context.Roles
                           on userroles.RoleId equals roles.Id
                           where roles.Name.ToLower() != StaticDetails.Role_Master_Admin.ToLower() && aspnetusers.Id == userId
                           select new
                           {
                               aspnetusers.FirstName,
                               aspnetusers.LastName,
                               aspnetusers.Id,
                           }).FirstOrDefault();

            if (dbQuery == null)
            {
                TempData["error"] = "Invalid User Provided.";
                return RedirectToAction("Index");
            }

            EditUserUsersNameViewModel editUserUsersNameViewModel = new EditUserUsersNameViewModel
            {
                UserId = applicationUser.Id,
                UserName = applicationUser.UserName,
                OldUserName = $"{applicationUser.FirstName} {applicationUser.LastName}"
            };

            return View(editUserUsersNameViewModel);
        }

        [HttpPost]
        public IActionResult EditUserUsersName(EditUserUsersNameViewModel editUserUsersNameViewModel)
        {
            if (ModelState.IsValid && editUserUsersNameViewModel.UserName != null)
            {
                ApplicationUser applicationUser = _context.ApplicationUsers.Find(editUserUsersNameViewModel.UserId);
                if (applicationUser == null)
                {
                    TempData["error"] = "Invalid User Provided.";
                    return RedirectToAction("Index");
                }

                applicationUser.UserName = editUserUsersNameViewModel.UserName;
                applicationUser.NormalizedUserName = editUserUsersNameViewModel.UserName.ToUpper();

                _context.ApplicationUsers.Update(applicationUser);
                _context.SaveChanges();

                TempData["success"] = "The User's UserName was updated successfully";

                return RedirectToAction(nameof(CoachAdminPanel));
            }

            TempData["error"] = "Invalid User Provided.";
            return RedirectToAction(nameof(CoachAdminPanel));
        }


        public IActionResult ChangeUserPassword(string userId)
        {
            ApplicationUser applicationUser = _context.ApplicationUsers.Find(userId);
            if (applicationUser == null)
            {
                TempData["error"] = "Invalid user provided";
                return RedirectToAction(nameof(CoachAdminPanel));
            }

            var dbQuery = (from aspnetusers in _context.ApplicationUsers
                           join userroles in _context.UserRoles
                           on aspnetusers.Id equals userroles.UserId
                           join roles in _context.Roles
                           on userroles.RoleId equals roles.Id
                           where roles.Name.ToLower() != StaticDetails.Role_Master_Admin.ToLower() && aspnetusers.Id == userId
                           select new
                           {
                               roles.Name
                           }).FirstOrDefault();

            if (dbQuery == null)
            {
                TempData["error"] = "Invalid User Provided.";
                return RedirectToAction(nameof(CoachAdminPanel));
            }

            ChangeUserPasswordViewModel changeUserPasswordViewModel = new ChangeUserPasswordViewModel
            {
                UsersName = $"{applicationUser.FirstName} {applicationUser.LastName}",
                UserId = applicationUser.Id,
                UserRole = dbQuery.Name
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
                return RedirectToAction(nameof(CoachAdminPanel));
            }

            if (changeUserPasswordViewModel.NewPassword == null)
            {
                TempData["error"] = "Invalid password provided";
                return RedirectToAction(nameof(CoachAdminPanel));
            }

            var newPassword = _userManager.PasswordHasher.HashPassword(user, changeUserPasswordViewModel.NewPassword);
            user.PasswordHash = newPassword;

            _context.ApplicationUsers.Update(user);
            _context.SaveChanges();

            TempData["success"] = "Password updated successfully";
            return RedirectToAction(nameof(CoachAdminPanel));
        }

        public IActionResult DeleteUser(string userId)
        {
            if (userId == null)
            {
                TempData["error"] = "Invalid User provided";
                return RedirectToAction(nameof(CoachAdminPanel));
            }

            var dbQuery = (from aspnetusers in _context.ApplicationUsers
                           join userroles in _context.UserRoles
                           on aspnetusers.Id equals userroles.UserId
                           join roles in _context.Roles
                           on userroles.RoleId equals roles.Id
                           where roles.Name.ToLower() != StaticDetails.Role_Master_Admin.ToLower() && aspnetusers.Id == userId
                           select new
                           {
                               aspnetusers.Id,
                               aspnetusers.FirstName,
                               aspnetusers.LastName
                           }).FirstOrDefault();

            if (dbQuery == null)
            {
                TempData["error"] = "Invalid User Provided.";
                return RedirectToAction(nameof(CoachAdminPanel));
            }

            DeleteUserViewModel deleteUserViewModel = new DeleteUserViewModel
            {
                UsersName = $"{dbQuery.FirstName} {dbQuery.LastName}",
                UserId = dbQuery.Id,
            };

            return View(deleteUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(DeleteUserViewModel deleteUserViewModel)
        {
            throw new Exception("Need to delete foreign key relationship(s). Error is thrown because of reference");
            if (deleteUserViewModel.UserId == null)
            {
                TempData["error"] = "Invalid User Provided.";
                return RedirectToAction(nameof(CoachAdminPanel));
            }

            var user = await _userManager.FindByIdAsync(deleteUserViewModel.UserId);
            if (user == null)
            {
                TempData["error"] = "Invalid User Provided.";
                return RedirectToAction(nameof(CoachAdminPanel));
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    TempData["success"] = $"The user {deleteUserViewModel.UsersName} was deleted from the database successfully";
                    return RedirectToAction(nameof(CoachAdminPanel));
                }

                TempData["error"] = $"An error occured during the delete process. The user {deleteUserViewModel.UsersName} was not deleted from the database successfully";
                return RedirectToAction(nameof(CoachAdminPanel));
            }

        }
    }
}
