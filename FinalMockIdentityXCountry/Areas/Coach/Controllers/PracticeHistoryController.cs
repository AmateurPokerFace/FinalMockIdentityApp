using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.Delete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation.Context;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public IActionResult Index() 
        {
            return View(); 
        }

        public IActionResult History()
        {
            List<HistoryViewModel> historyViewModels = new List<HistoryViewModel>();
 

            var dbQueries = (from p in _context.Practices
                                  join a in _context.Attendances
                                  on p.Id equals a.PracticeId
                                  where a.IsPresent && p.PracticeIsInProgress == false
                                  group a by new
                                  {
                                      a.PracticeId,
                                      p.PracticeLocation,
                                      p.PracticeStartTimeAndDate
                                  } into matchesFound
                                  select new HistoryViewModel()
                                  {
                                      PracticeId = matchesFound.Key.PracticeId,
                                      PracticeDateTime = matchesFound.Key.PracticeStartTimeAndDate,
                                      PracticeLocation = matchesFound.Key.PracticeLocation,
                                      TotalRunners = matchesFound.Count(),
                                  });

            if (dbQueries.Count() > 0)
            {
                foreach (var dbQuery in dbQueries)
                {
                    HistoryViewModel historyViewModel = new HistoryViewModel
                    {
                        PracticeId = dbQuery.PracticeId,
                        PracticeDateTime = dbQuery.PracticeDateTime,
                        PracticeLocation = dbQuery.PracticeLocation,
                        TotalRunners = dbQuery.TotalRunners
                    };

                    historyViewModels.Add(historyViewModel);
                } 
            }

            if (historyViewModels.Count() > 0)
            {
                historyViewModels = historyViewModels.OrderByDescending(d => d.PracticeDateTime).ToList();
                return View(historyViewModels);
            }

            return RedirectToAction(nameof(Index)); // send to an error page in the future (no practice history found)
        }
    }
}
