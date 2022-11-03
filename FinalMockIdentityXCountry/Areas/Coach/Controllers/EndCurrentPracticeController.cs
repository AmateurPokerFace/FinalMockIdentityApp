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
            Practice? practice = _context.Practices.Where(p => p.PracticeIsInProgress).Where(p => p.Id == currentPracticeId).FirstOrDefault();
            
            if (practice != null)
            {
                return View(practice);
            }

            return RedirectToAction();  // send to an error page in the future. return a view -> No practices match the id that was provided
        }

        [HttpPost]
        public IActionResult End(int id, string dummyString)
        {
            if (id == 0)
            {
                return RedirectToAction();  // send to an error page in the future.
            }

            Practice practice = _context.Practices.Find(id);

            if (practice == null)
            {
                return RedirectToAction();  // send to an error page in the future.
            }

            practice.PracticeEndTimeAndDate = DateTime.Now;
            practice.PracticeIsInProgress = false;
            _context.Practices.Update(practice);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index)); // send to a page showing each runner who hasn't signed out.
        }
    }
}
