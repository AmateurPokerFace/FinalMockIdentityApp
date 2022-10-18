using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class AnnouncementsController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question
        public AnnouncementsController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Home()
        {
            IEnumerable<MessageBoard> messageBoards = _context.MessageBoards;
            return View(messageBoards);
        }

        public IActionResult NewAnnouncement()
        {     
            return View();
        }

        [HttpPost]
        public IActionResult NewAnnouncement(NewAnnouncementViewModel newAnnouncementViewModel)
        {
            if (ModelState.IsValid)
            {
                var userClaimsIdentity = (ClaimsIdentity)User.Identity;
                var userClaim = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                if (userClaim != null)
                {
                    var userClaimValue = userClaim.Value;
                    MessageBoard messageBoard = new MessageBoard
                    {
                        CoachId = userClaim.Value,
                        MessageTitle = newAnnouncementViewModel.AnnouncementTitle,
                        MessageBody = newAnnouncementViewModel.AnnouncementBody,
                        PublishedDateTime = DateTime.Now
                    };

                    _context.MessageBoards.Add(messageBoard);
                    _context.SaveChanges();
                }
            }
            return View();
        }
    }
}
