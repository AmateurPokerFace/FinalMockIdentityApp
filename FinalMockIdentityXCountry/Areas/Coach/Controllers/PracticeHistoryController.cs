using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.Delete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation.Context;
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

        public IActionResult History()
        {

            return View();
        }
    }
}
