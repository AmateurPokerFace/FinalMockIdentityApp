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

        public IActionResult DeleteThread(int messageBoardId)
        {
            var messageBoard = _context.MessageBoards.Find(messageBoardId);
            
            if (messageBoard == null)
            {
                return RedirectToAction(nameof(Home)); // throw an error in the future
            }

            return View(messageBoard);
        }

        [HttpPost] // You will probably need a viewmodel for the httppost method below
        public IActionResult DeleteThread(int messageBoardId, string dummyString)
        {
            var messageBoard = _context.MessageBoards.Find(messageBoardId);
            if (messageBoard == null)
            {
                return RedirectToAction(nameof(Home)); // throw an error in the future
            }

            _context.MessageBoards.Remove(messageBoard);
            _context.SaveChanges(); 
            return RedirectToAction(nameof(Home)); // add a deleted successfully page in the future
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
                };

                return View(addThreadCommentViewModel);
            }

            return RedirectToAction(nameof(Home)); // throw error in the future
            
        }

        [HttpPost]
        public IActionResult AddThreadComment(MessageBoardResponse NewMessageBoardComment)
        {
            if (NewMessageBoardComment.MessageBoardId == 0)
            {
                return RedirectToAction(nameof(Home)); // Add an error in the future
            }

            var messageBoard = _context.MessageBoards.Find(NewMessageBoardComment.MessageBoardId);
            if (messageBoard == null)
            {
                return RedirectToAction(nameof(Home)); // Add an error in the future
            }

            if (ModelState.IsValid) 
            {
                NewMessageBoardComment.ResponseDateTime = DateTime.Now;
                _context.MessageBoardResponses.Add(NewMessageBoardComment);
                _context.SaveChanges();
                return RedirectToAction(nameof(Home)); // create a success page in the future
            }
             
            return RedirectToAction(nameof(Home)); // create an error page in the future
        }

        public IActionResult ThreadCommentReplies(int messageBoardResponseId)
        {

            ThreadCommentViewModel threadCommentViewModel = new ThreadCommentViewModel();
            threadCommentViewModel.MessageBoardResponse = _context.MessageBoardResponses.Find(messageBoardResponseId);

            if (threadCommentViewModel.MessageBoardResponse == null)
            {
                return RedirectToAction(nameof(Home)); // redirect to not found page in the future.
            }

            threadCommentViewModel.RepliesToMessageBoardResponse = _context.RepliesToMessageBoardResponse
                .Where(r => r.MessageBoardResponseId == threadCommentViewModel.MessageBoardResponse.Id); 

            return View(threadCommentViewModel); 
        }

        public IActionResult AddReplyToThreadComment(int messageBoardResponseId) 
        {

            var userClaimsIdentity = (ClaimsIdentity?)User.Identity;
            var userClaim = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim != null)
            {
                AddReplyToThreadCommentViewModel addReplyToThreadCommentVm = new AddReplyToThreadCommentViewModel();
                addReplyToThreadCommentVm.MessageBoardResponse = _context.MessageBoardResponses.Find(messageBoardResponseId);

                if (addReplyToThreadCommentVm.MessageBoardResponse == null)
                {
                    return RedirectToAction(nameof(Home)); // redirect to not found page in the future.
                }

                addReplyToThreadCommentVm.NewReplyToMessageBoardResponse = new ReplyToMessageBoardResponse
                {
                    MessageBoardResponseId = messageBoardResponseId,
                    ReplyerId = userClaim.Value, 
                }; 

                return View(addReplyToThreadCommentVm);
            }

            return RedirectToAction(nameof(Home)); // throw error in the future
        }

        [HttpPost]
        public IActionResult AddReplyToThreadComment(ReplyToMessageBoardResponse NewReplyToMessageBoardResponse)
        {
            if (NewReplyToMessageBoardResponse.MessageBoardResponseId == 0)
            {
                return RedirectToAction(nameof(Home)); // Add an error in the future
            }

            var messageBoardResponse = _context.MessageBoardResponses.Find(NewReplyToMessageBoardResponse.MessageBoardResponseId);
            if (messageBoardResponse == null)
            {
                return RedirectToAction(nameof(Home)); // Add an error in the future
            }

            if (ModelState.IsValid)
            {
                NewReplyToMessageBoardResponse.ReplyDateTime = DateTime.Now;
                _context.RepliesToMessageBoardResponse.Add(NewReplyToMessageBoardResponse);
                _context.SaveChanges();

                return RedirectToAction(nameof(ThreadCommentReplies));
            }


            return RedirectToAction(nameof(Home)); // add error in the future
        }

        public IActionResult DeleteThreadComment(int messageBoardResponseId)
        {
            var messageBoardResponse = _context.MessageBoardResponses.Find(messageBoardResponseId);

            if (messageBoardResponse == null)
            {
                return RedirectToAction("Home"); //throw an error in the future
            }
            
            return View(messageBoardResponse);
        }

        [HttpPost]
        public IActionResult DeleteThreadComment(int messageBoardResponseId, string dummyString)
        {
            var messageBoardResponse = _context.MessageBoardResponses.Find(messageBoardResponseId);
            if (messageBoardResponse == null)
            {
                return RedirectToAction("Home"); //throw an error in the future
            }

            _context.MessageBoardResponses.Remove(messageBoardResponse);
            _context.SaveChanges();

            return RedirectToAction("Home"); // redirect to a success page in the future.
        }

        public IActionResult DeleteThreadReply(int replyToMessageBoardResponseId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteThreadReply(int replyToMessageBoardResponseId, string dummyString)
        {
            return View();
        }
    }
}
