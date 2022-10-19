using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class ForumController : Controller
    {
        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question

        public ForumController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Home()
        {
            List<MessageBoard> messageBoards = new List<MessageBoard>();
            messageBoards = _context.MessageBoards.OrderByDescending(d => d.PublishedDateTime.Date).ThenBy(d => d.PublishedDateTime.TimeOfDay).ToList();    
            
            return View(messageBoards);
        }


        public IActionResult CreateThread()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateThread(NewForumThreadViewModel newAnnouncementViewModel)
        {
            if (ModelState.IsValid)
            {
                var userClaimsIdentity = (ClaimsIdentity?)User.Identity;
                var userClaim = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                if (userClaim != null)
                {
                    MessageBoard messageBoard = new MessageBoard
                    {
                        CoachId = userClaim.Value,
                        MessageTitle = newAnnouncementViewModel.ThreadTitle,
                        MessageBody = newAnnouncementViewModel.ThreadBody,
                        PublishedDateTime = DateTime.Now, 
                    };

                    messageBoard.CoachName = _context.ApplicationUsers
                        .Where(c => c.Id == userClaim.Value)
                        .Select(c => $"{c.FirstName} {c.LastName}").FirstOrDefault();


                    _context.MessageBoards.Add(messageBoard);
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Home));
                }
            }
            return View();
        }

        public IActionResult ViewThread(int messageBoardId)
        {
            ViewThreadViewModel viewThreadViewModel = new ViewThreadViewModel();
            viewThreadViewModel.MessageBoard = _context.MessageBoards.Find(messageBoardId);
            if (viewThreadViewModel.MessageBoard == null)
            {
                return RedirectToAction(nameof(Home)); // return a invalid id provided page => Make this a partial view
            }

            viewThreadViewModel.DatabaseMessageBoardResponses = (IEnumerable<MessageBoardResponse>?)_context.MessageBoardResponses
                .OrderByDescending(d => d.ResponseDateTime.Date)
                .Where(m => m.MessageBoardId == messageBoardId);
            

            return View(viewThreadViewModel);
        }

        public IActionResult AddThreadComment(int messageBoardId)
        {
            var userClaimsIdentity = (ClaimsIdentity?)User.Identity;
            var userClaim = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userClaim != null) 
            {
                AddThreadCommentViewModel addThreadCommentViewModel = new AddThreadCommentViewModel();
                addThreadCommentViewModel.MessageBoard = _context.MessageBoards.Find(messageBoardId);

                if (addThreadCommentViewModel.MessageBoard == null)
                {
                    return View(); // redirect to not found page
                }

                addThreadCommentViewModel.NewMessageBoardComment = new MessageBoardResponse
                {
                    MessageBoardId = messageBoardId,
                    ResponderId = userClaim.Value,
                    RespondersName = _context.ApplicationUsers
                        .Where(c => c.Id == userClaim.Value)
                        .Select(c => $"{c.FirstName} {c.LastName}").FirstOrDefault(),
                };

                return View(addThreadCommentViewModel);
            }

            return RedirectToAction(nameof(Home)); // throw error in the future
            
        }

        [HttpPost]
        public IActionResult AddThreadComment(MessageBoardResponse NewMessageBoardComment)
        {
            if (ModelState.IsValid) 
            {
                NewMessageBoardComment.ResponseDateTime = DateTime.Now;
                _context.MessageBoardResponses.Add(NewMessageBoardComment);
                _context.SaveChanges();
                return RedirectToAction(nameof(Home)); // create a success page in the future
            }



            return RedirectToAction(nameof(Home)); // create an error page in the future
        }
    }
}
