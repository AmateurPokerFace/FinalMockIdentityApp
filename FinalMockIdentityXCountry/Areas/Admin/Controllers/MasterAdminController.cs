using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.Utilities;
using FinalMockIdentityXCountry.Models.ViewModels.AdminAreaViewModels.MasterAdminControllerViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinalMockIdentityXCountry.Areas.Admin.Controllers
{
    [Authorize(Roles = "Master Admin")]
    [Area("Admin")]
    public class MasterAdminController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MasterAdminController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MasterAdminPanel()
        {
            var dbQueries = (from aspnetusers in _context.ApplicationUsers
                             join userroles in _context.UserRoles
                             on aspnetusers.Id equals userroles.UserId
                             join roles in _context.Roles
                             on userroles.RoleId equals roles.Id
                             where roles.Name.ToLower() != StaticDetails.Role_Master_Admin.ToLower()
                             select new
                             {
                                 aspnetusers.UserName,
                                 aspnetusers.FirstName,
                                 aspnetusers.LastName,
                                 aspnetusers.Id,
                                 userroles.RoleId,
                                 roles.Name,
                             });


            MasterAdminPanelViewModel masterAdminPanelViewModel = new MasterAdminPanelViewModel { MasterAdminPanelRole = "" };

            ApplicationUser applicationAdminUser = _context.ApplicationUsers.Find(_userManager.GetUserId(User));
            IdentityUser identityAdminUser = _context.Users.Find(_userManager.GetUserId(User));


            if (applicationAdminUser != null )
            {
                masterAdminPanelViewModel.MasterAdminPanelRole = _userManager.IsInRoleAsync(applicationAdminUser, StaticDetails.Role_Master_Admin).Result == true ? StaticDetails.Role_Master_Admin : StaticDetails.Role_Coach;
                
            }
            else if (identityAdminUser != null)
            {
                masterAdminPanelViewModel.MasterAdminPanelRole = _userManager.IsInRoleAsync(identityAdminUser, StaticDetails.Role_Master_Admin).Result == true ? StaticDetails.Role_Master_Admin : StaticDetails.Role_Coach;
            }
            else
            {
                TempData["error"] = "An error occured. You were not found in the database. Please contact an administrator for more assistance.";
                return RedirectToAction("Index");
            }

            

            foreach (var dbQuery in dbQueries)
            {
                MasterAdminPanelViewModelHelper masterAdminPanelViewModelHelper = new MasterAdminPanelViewModelHelper
                {
                    Name = $"{dbQuery.FirstName} {dbQuery.LastName}",
                    RoleId = dbQuery.RoleId,
                    UserId = dbQuery.Id,
                    UserName = dbQuery.UserName,
                    UserRole = dbQuery.Name
                };

                masterAdminPanelViewModel.MasterAdminPanelViewModelHelpers?.Add(masterAdminPanelViewModelHelper);
            }

            return View(masterAdminPanelViewModel);
        }

        public IActionResult ChangeUserRole(string userId, string currentRoleName) 
        {
            if (userId == null || currentRoleName == null)
            {
                TempData["error"] = "Invalid Query Data Provided";
                return RedirectToAction(nameof(MasterAdminPanel));
            }

            var dbQuery = (from userRole in _context.UserRoles
                           join aspnetroles in _context.Roles
                           on userRole.RoleId equals aspnetroles.Id
                           where userRole.UserId == userId
                           && aspnetroles.Name.ToLower() != StaticDetails.Role_Master_Admin.ToLower()
                           select new 
                           {
                               aspnetroles.Name
                           }).FirstOrDefault();

            if (dbQuery == null)
            {
                TempData["error"] = "Invalid user provided.";
                return RedirectToAction(nameof(MasterAdminPanel));
            }

            ChangeUserRoleViewModel changeUserRoleViewModel = new ChangeUserRoleViewModel();
            changeUserRoleViewModel.CurrentRoleName = dbQuery.Name;
            changeUserRoleViewModel.UserId = userId; 
            
            var roles = _context.Roles.Where(r => r.Name.ToLower() != currentRoleName.ToLower());

            if (roles != null && roles.Count() > 0)
            {
                foreach (var role in roles)
                {
                    changeUserRoleViewModel.RolesList?.Add(new SelectListItem { Text = role.Name, Value = role.Name });
                }

                return View(changeUserRoleViewModel);
            }

            TempData["error"] = "There were no other roles found in the database to assign.";
            return RedirectToAction(nameof(MasterAdminPanel));
        }
         

        [HttpPost]
        public async Task<IActionResult> ChangeRole(ChangeUserRoleViewModel changeUserRoleViewModel)
        {
            if (changeUserRoleViewModel == null)
            {
                TempData["error"] = "Invalid Query Data Provided";
                return RedirectToAction(nameof(MasterAdminPanel));
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
                        return RedirectToAction(nameof(MasterAdminPanel));
                    }
                    catch (Exception e)
                    {

                        TempData["error"] = $"{e.Message}";
                        return RedirectToAction(nameof(MasterAdminPanel));
                    }
                    
                }
            }

            TempData["error"] = "Invalid Query Data Provided";
            return RedirectToAction(nameof(MasterAdminPanel));
        }

        public IActionResult EditUser(string userId) 
        {
            if (userId == null)
            {
                TempData["error"] = "Invalid User provided";
                return RedirectToAction(nameof(MasterAdminPanel));
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
            return RedirectToAction("Index"); 
        }

        public IActionResult EditUsersName(string userId)
        {
            if (userId == null)
            {
                TempData["error"] = "Invalid User provided";
                return RedirectToAction(nameof(MasterAdminPanel));
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
            ApplicationUser applicationUser;

            if (ModelState.IsValid)
            {
                applicationUser = _context.ApplicationUsers.Find(editUserViewModel.UserId);
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

                return RedirectToAction(nameof(MasterAdminPanel));
            }

            TempData["error"] = "Invalid Data Provided";

            applicationUser = _context.ApplicationUsers.Find(editUserViewModel.UserId);
            if (applicationUser == null)
            {
                TempData["error"] = "Invalid User Provided";
                return RedirectToAction("Index");
            }

            editUserViewModel.FirstName = applicationUser.FirstName == null ? " " : applicationUser.FirstName;
            editUserViewModel.LastName = applicationUser.LastName == null ? " " : applicationUser.LastName;
            editUserViewModel.OldName = $"{(applicationUser.FirstName == null ? " " : applicationUser.FirstName)} {(applicationUser.LastName == null ? " " : applicationUser.LastName)}";

            return View(editUserViewModel);
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
            ApplicationUser applicationUser;

            if (ModelState.IsValid && editUserUsersNameViewModel.UserName != null)
            {

                applicationUser = _context.ApplicationUsers.Find(editUserUsersNameViewModel.UserId);
                if (applicationUser == null)
                {
                    TempData["error"] = "Invalid User Provided.";
                    return RedirectToAction("Index");
                }

                var userNameAlreadyExistsInDb = _context.ApplicationUsers.Any(x => x.UserName == editUserUsersNameViewModel.UserName);
                if (userNameAlreadyExistsInDb)
                {
                    TempData["error"] = "Duplicate username found";
                    ModelState.AddModelError("UserName", $"A user already exists with the provided username {editUserUsersNameViewModel.UserName}. Please enter a different username");
                    editUserUsersNameViewModel.OldUserName = applicationUser.UserName;
                    return View(editUserUsersNameViewModel);
                }

                applicationUser.UserName = editUserUsersNameViewModel.UserName;
                applicationUser.NormalizedUserName = editUserUsersNameViewModel.UserName.ToUpper();

                _context.ApplicationUsers.Update(applicationUser);
                _context.SaveChanges();

                TempData["success"] = "The User's UserName was updated successfully";

                return RedirectToAction(nameof(MasterAdminPanel));
            }

            TempData["error"] = "Invalid data provided.";
            applicationUser = _context.ApplicationUsers.Find(editUserUsersNameViewModel.UserId);
            if (applicationUser == null)
            {
                TempData["error"] = "Invalid User Provided.";
                return RedirectToAction(nameof(MasterAdminPanel));
            }

            editUserUsersNameViewModel.OldUserName = applicationUser.UserName;
            editUserUsersNameViewModel.UserName = applicationUser.UserName; 

            return View(editUserUsersNameViewModel); 
        }


        public IActionResult ChangeUserPassword(string userId)
        {
            ApplicationUser applicationUser = _context.ApplicationUsers.Find(userId);
            if (applicationUser == null)
            {
                TempData["error"] = "Invalid user provided";
                return RedirectToAction(nameof(MasterAdminPanel));
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
                return RedirectToAction(nameof(MasterAdminPanel));
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
                return RedirectToAction(nameof(MasterAdminPanel));
            }

            if (ModelState.IsValid)
            {
                var newPassword = _userManager.PasswordHasher.HashPassword(user, changeUserPasswordViewModel.NewPassword);
                user.PasswordHash = newPassword;

                _context.ApplicationUsers.Update(user);
                _context.SaveChanges();

                TempData["success"] = "Password updated successfully";
                return RedirectToAction(nameof(MasterAdminPanel));
            }

            changeUserPasswordViewModel.UsersName = $"{user.FirstName} {user.LastName}";

            return View(changeUserPasswordViewModel);
        }

        public IActionResult DeleteUser(string userId) 
        {
            if (userId == null)
            {
                TempData["error"] = "Invalid User provided";
                return RedirectToAction(nameof(MasterAdminPanel));
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
                return RedirectToAction(nameof(MasterAdminPanel));
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
            //throw new Exception("Need to delete foreign key relationship(s). Error is thrown because of reference"); 
            if (deleteUserViewModel.UserId == null)
            {
                TempData["error"] = "Invalid User Provided.";
                return RedirectToAction(nameof(MasterAdminPanel));
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(deleteUserViewModel.UserId);
                if (user == null)
                {
                    TempData["error"] = "Invalid User Provided.";
                    return RedirectToAction(nameof(MasterAdminPanel));
                }
                else
                {
                    var result = await _userManager.DeleteAsync(user);

                    if (result.Succeeded)
                    {
                        TempData["success"] = $"The user {deleteUserViewModel.UsersName} was deleted from the database successfully";
                        return RedirectToAction(nameof(MasterAdminPanel));
                    }

                    TempData["error"] = $"An error occured during the delete process. The user {deleteUserViewModel.UsersName} was not deleted from the database successfully";
                    return RedirectToAction(nameof(MasterAdminPanel));
                }
            }

            ApplicationUser invalidUser = (ApplicationUser)await _userManager.FindByIdAsync(deleteUserViewModel.UserId);
            if (invalidUser == null)
            {
                TempData["error"] = "Invalid User Provided.";
                return RedirectToAction(nameof(MasterAdminPanel));
            }

            deleteUserViewModel.UsersName = $"{invalidUser.FirstName} {invalidUser.LastName}";

            TempData["error"] = "Invalid user provided.";
            return RedirectToAction(nameof(MasterAdminPanel));
        }

    }
}
