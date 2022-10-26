using FinalMockIdentityXCountry.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
            return View();
        }

        public IActionResult LogWorkoutData()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogWorkoutData(int dummy)
        {
            return View();
        }

        public IActionResult EditLoggedWorkoutData()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditLoggedWorkoutData(int dummy)
        {
            return View();
        }
    }
}
