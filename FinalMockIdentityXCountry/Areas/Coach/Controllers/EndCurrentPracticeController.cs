using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class EndCurrentPracticeController : Controller
    {

        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question
        public EndCurrentPracticeController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        { 
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index() 
        {
            return View();
        }

        public IActionResult End(int currentPracticeId)
        {
            Practice practice = _context.Practices.Where(p => p.PracticeIsInProgress).Where(p => p.Id == currentPracticeId).FirstOrDefault();
            
            if (practice != null)
            {
                return View(practice);
            }

            TempData["error"] = "Invalid practice provided";
            return RedirectToAction(nameof(Index));  // send to an error page in the future. return a view -> No practices match the id that was provided
        }

        [HttpPost]
        public IActionResult End(int id, string dummyString)
        {
            if (id == 0)
            {
                TempData["error"] = "Invalid practice provided";
                return RedirectToAction(nameof(Index));
            }

            Practice practice = _context.Practices.Find(id);

            if (practice == null)
            {
                TempData["error"] = "Invalid practice provided";
                return RedirectToAction(nameof(Index));
            }

            var runnersNotSignedOut = _context.Attendances.Where(a => a.PracticeId == id && a.IsPresent && a.HasBeenSignedOut == false).ToList();

            if (runnersNotSignedOut != null && runnersNotSignedOut.Count > 0)
            {
                foreach (var runner in runnersNotSignedOut)
                {
                    runner.HasBeenSignedOut = true; 
                    _context.Attendances.Update(runner);
                }
            }

            practice.PracticeEndTimeAndDate = DateTime.Now;
            practice.PracticeIsInProgress = false;
            _context.Practices.Update(practice);
            _context.SaveChanges();

            TempData["success"] = "Practice ended successfully.";
            return RedirectToAction("Index", "Home", new { area = "Welcome" });
        }
    }
}
