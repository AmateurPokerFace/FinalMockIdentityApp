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

        public IActionResult End(int currentPracticeId)
        {
            Practice? practice = _context.Practices.Where(p => p.PracticeIsInProgress).Where(p => p.Id == currentPracticeId).FirstOrDefault();
            
            if (practice != null)
            {
                practice.PracticeIsInProgress = false;
                return View(practice);
            }

            return View();  // return a view -> No practices match the id that was provided
        }

        [HttpPost]
        public IActionResult End(Practice practice)
        {



            return View();
        }
    }
}
