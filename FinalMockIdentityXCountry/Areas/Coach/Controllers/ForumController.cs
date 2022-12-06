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
        private readonly UserManager<IdentityUser> _userManager; 

        public ForumController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index() 
        {
            return View();
        }

        public IActionResult Home()
        {
            List<MessageBoard> messageBoards = new List<MessageBoard>();
            //messageBoards = _context.MessageBoards.OrderByDescending(d => d.PublishedDateTime.Date).ThenBy(d => d.PublishedDateTime.TimeOfDay).ToList();
            messageBoards = _context.MessageBoards.OrderByDescending(d => d.PublishedDateTime).ToList();
            
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
                var userClaim = userClaimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

                if (userClaim != null)
                {
                    MessageBoard messageBoard = new MessageBoard
                    {
                        CoachId = userClaim.Value,
                        MessageTitle = newAnnouncementViewModel.ThreadTitle,
                        MessageBody = newAnnouncementViewModel.ThreadBody,
                        PublishedDateTime = DateTime.Now, 
                    };

                    if (messageBoard != null)
                    {
                        _context.MessageBoards.Add(messageBoard);
                        _context.SaveChanges();
                        TempData["success"] = "The thread was created successfully";

                        return RedirectToAction(nameof(Home));
                    }

                    TempData["error"] = "An error occured while trying to save the record to the database. Please try again or contact an administrator for assistance.";
                    return RedirectToAction(nameof(Home));
                }
            }
            
            TempData["error"] = "The thread was not created successfully. Invalid data was provided";

            return View(newAnnouncementViewModel);
        }

        public IActionResult ViewThread(int messageBoardId)
        {
            if (messageBoardId == 0)
            {
                TempData["error"] = "Invalid message board id provided";
                return RedirectToAction(nameof(Home));
            }

            var messageBoard = (from m in _context.MessageBoards
                                join aspnetusers in _context.ApplicationUsers
                                on m.CoachId equals aspnetusers.Id
                                where m.Id == messageBoardId
                                select new
                                {
                                    m.MessageBody,
                                    m.PublishedDateTime,
                                    m.MessageTitle,
                                    m.Id,
                                    aspnetusers.FirstName,
                                    aspnetusers.LastName 
                                }).FirstOrDefault();

            if (messageBoard == null)
            {
                TempData["error"] = "Invalid message board id provided. The message was not found in the database.";
                return RedirectToAction(nameof(Home));
            }
            
            ViewThreadViewModel viewThreadViewModel = new ViewThreadViewModel 
            {
                MessageBoardBody = messageBoard.MessageBody,
                MessageBoardPublishedDateTime = messageBoard.PublishedDateTime,
                MessageBoardTitle = messageBoard.MessageTitle,
                MessageBoardId = messageBoard.Id,
                MessageBoardsAuthorName = $"{messageBoard.FirstName} {messageBoard.LastName}"
            };

            var messageBoardResponsesQueries = (from m in _context.MessageBoardResponses
                                                join aspnetusers in _context.ApplicationUsers
                                                on m.ResponderId equals aspnetusers.Id
                                                where m.MessageBoardId == messageBoardId
                                                select new
                                                {
                                                    m.Id,
                                                    m.ResponseDateTime,
                                                    m.Response,
                                                    aspnetusers.FirstName,
                                                    aspnetusers.LastName
                                                });

            if (messageBoardResponsesQueries != null && messageBoardResponsesQueries.Count() > 0)
            {
                foreach (var messageBoardResponse in messageBoardResponsesQueries)
                {
                    ViewThreadModelHelper vmHelper = new ViewThreadModelHelper
                    { 
                        MessageBoardResponseId = messageBoardResponse.Id,
                        MessageBoardResponse = messageBoardResponse.Response,
                        MessageBoardResponseDateTime = messageBoardResponse.ResponseDateTime,
                        MessageBoardRespondersName = $"{messageBoardResponse.FirstName} {messageBoardResponse.LastName}"
                    };

                    if (vmHelper != null)
                    {
                        viewThreadViewModel.DatabaseMessageBoardResponses.Add(vmHelper);
                    }
                }
            }

            if (viewThreadViewModel.DatabaseMessageBoardResponses != null && viewThreadViewModel.DatabaseMessageBoardResponses.Count > 0)
            {
                viewThreadViewModel.DatabaseMessageBoardResponses = viewThreadViewModel.DatabaseMessageBoardResponses
                                                                    .OrderByDescending(d => d.MessageBoardResponseDateTime).ToList();
            }

            if (viewThreadViewModel != null)
            {
                return View(viewThreadViewModel);
            }

            TempData["error"] = "Invalid data submitted. The message board wasn't found in the database.";
            return RedirectToAction(nameof(Home));
        }

        public IActionResult DeleteThread(int messageBoardId)
        {
            var messageBoard = _context.MessageBoards.Find(messageBoardId);
            
            if (messageBoard == null)
            {
                TempData["error"] = "Invalid data provided. The provided message board id is invalid.";
                return RedirectToAction(nameof(Home));
            }

            return View(messageBoard);
        }

        [HttpPost] // You will probably need a viewmodel for the httppost method below
        public IActionResult DeleteThread(int messageBoardId, string dummyString)
        {
            if (messageBoardId == 0)
            {
                TempData["error"] = "Invalid message board id provided";
                return RedirectToAction(nameof(Home));
            }

            var messageBoard = _context.MessageBoards.Find(messageBoardId);

            if (messageBoard == null)
            {
                TempData["error"] = "Invalid message board id provided. The message was not found in the database";
                return RedirectToAction(nameof(Home)); 
            }

            _context.MessageBoards.Remove(messageBoard);
            _context.SaveChanges();
            TempData["success"] = "The thread was deleted successfully";

            return RedirectToAction(nameof(Home)); 
        }

        public IActionResult AddThreadComment(int messageBoardId)
        {
            var userClaimsIdentity = (ClaimsIdentity?)User.Identity;
            var userClaim = userClaimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            
            if (userClaim != null) 
            { 
                var dbQuery = (from m in _context.MessageBoards
                               where m.Id == messageBoardId
                               select new 
                               {
                                   m.Id,
                                   m.MessageTitle,
                                   m.MessageBody,
                                   m.PublishedDateTime
                               }).FirstOrDefault();

                AddThreadCommentViewModel addThreadCommentViewModel = new AddThreadCommentViewModel();
                
                if (dbQuery != null)
                {
                    
                    addThreadCommentViewModel.MessageBoardId = dbQuery.Id;
                    addThreadCommentViewModel.MessageBoardMessageBody= dbQuery.MessageBody;
                    addThreadCommentViewModel.MessageBoardMessageTitle= dbQuery.MessageTitle;
                    addThreadCommentViewModel.MessageBoardPublishedDateTime= dbQuery.PublishedDateTime;
                    addThreadCommentViewModel.NewMessageBoardCommentResponderId = userClaim.Value;
                }
                else
                {
                    TempData["error"] = "Invalid message board id provided";
                    return RedirectToAction(nameof(Home));
                }

                if (addThreadCommentViewModel != null)
                {
                    return View(addThreadCommentViewModel);
                }
                else
                {
                    TempData["error"] = "Invalid message board provided";
                    return RedirectToAction(nameof(Home));
                }
            }

            TempData["error"] = "Invalid user claim. You cannot add a comment to this thread";
            return RedirectToAction(nameof(Home)); // throw error in the future
            
        }

        [HttpPost]
        public IActionResult AddThreadComment(AddThreadCommentViewModel addThreadCommentViewModel)
        {
            if (ModelState.IsValid)
            {
                MessageBoardResponse messageBoardResponse = new MessageBoardResponse
                {
                    MessageBoardId = addThreadCommentViewModel.MessageBoardId,
                    ResponderId = addThreadCommentViewModel.NewMessageBoardCommentResponderId,
                    Response = addThreadCommentViewModel.NewNessageBoardCommentResponse,
                    ResponseDateTime = DateTime.Now
                };

                bool messageBoardResponseSaved = false;

                if (messageBoardResponse != null)
                {
                    _context.MessageBoardResponses.Add(messageBoardResponse);
                    _context.SaveChanges();
                    messageBoardResponseSaved = true;
                }
                
                if (messageBoardResponseSaved)
                {
                    TempData["success"] = "The response was added successfully.";
                }

                else
                {
                    TempData["error"] = "The response did not save successfully. Please try again.";
                }

                return RedirectToAction(nameof(Home));
            }

            TempData["error"] = "The data wasn't submitted because it is invalid.";

            return View(addThreadCommentViewModel); 
        }

        public IActionResult ThreadCommentReplies(int messageBoardResponseId)
        {
            if (messageBoardResponseId == 0)
            {
                TempData["error"] = "Invalid message board response id provided";
                return RedirectToAction(nameof(Home));
            }

            
             
            var originalMessageBoardResponseQuery = (from m in _context.MessageBoardResponses
                                                     join aspnetusers in _context.ApplicationUsers
                                                     on m.ResponderId equals aspnetusers.Id
                                                     where m.Id == messageBoardResponseId
                                                     select new
                                                     {
                                                         m.Response,
                                                         m.MessageBoardId,
                                                         m.ResponseDateTime,
                                                         aspnetusers.FirstName,
                                                         aspnetusers.LastName
                                                     }).FirstOrDefault();

            if (originalMessageBoardResponseQuery == null)
            {
                TempData["error"] = "Invalid message board response id provided.\n" +
                    "The message board response doesn't exist in the database.";
                return RedirectToAction(nameof(Home));
            }

            ThreadCommentViewModel threadCommentViewModel = new ThreadCommentViewModel 
            {
                OriginalAuthorResponse = originalMessageBoardResponseQuery.Response,
                MessageBoardResponseId = originalMessageBoardResponseQuery.MessageBoardId,
                OriginalAuthorResponseDateTime = originalMessageBoardResponseQuery.ResponseDateTime,
                OriginalAuthorName = $"{originalMessageBoardResponseQuery.FirstName} {originalMessageBoardResponseQuery.LastName}"
            };

            var repliesToMessageBoardResponseQuery = (from r in _context.RepliesToMessageBoardResponse
                                                      join aspnetusers in _context.ApplicationUsers
                                                      on r.ReplyerId equals aspnetusers.Id
                                                      where r.MessageBoardResponseId == messageBoardResponseId
                                                      select new
                                                      {
                                                          r.Reply,
                                                          r.Id,
                                                          r.ReplyDateTime,
                                                          aspnetusers.FirstName,
                                                          aspnetusers.LastName
                                                      });

            if (repliesToMessageBoardResponseQuery != null && repliesToMessageBoardResponseQuery.Count() > 0)
            {
                foreach (var replyToMessageBoardResponse in repliesToMessageBoardResponseQuery)
                {
                    ThreadCommentViewModelHelper ReplyToMessageBoardResponse = new ThreadCommentViewModelHelper
                    {
                        ReplyDateTime= replyToMessageBoardResponse.ReplyDateTime,
                        ReplyToMessageBoardResponse = replyToMessageBoardResponse.Reply,
                        ReplyToMessageBoardResponseId = replyToMessageBoardResponse.Id,
                        ReplyerName = $"{replyToMessageBoardResponse.FirstName} {replyToMessageBoardResponse.LastName}"
                    };

                    if (ReplyToMessageBoardResponse != null)
                    {
                        threadCommentViewModel.RepliesToMessageBoardResponses.Add(ReplyToMessageBoardResponse);
                    }
                }
            }

            if (threadCommentViewModel != null)
            {
                return View(threadCommentViewModel);
            }

            TempData["error"] = "There were no thread comment replies found";
            return RedirectToAction(nameof(Home));
        }

        public IActionResult AddReplyToThreadComment(int messageBoardResponseId) 
        {
            if (messageBoardResponseId == 0)
            {
                TempData["error"] = "Invalid message board response id provided";
                return RedirectToAction(nameof(Home));
            }
            var userClaimsIdentity = (ClaimsIdentity?)User.Identity;
            var userClaim = userClaimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (userClaim != null)
            {
                var messageBoardResponsesQuery = (from m in _context.MessageBoardResponses
                                                  join aspnetusers in _context.ApplicationUsers
                                                  on m.ResponderId equals aspnetusers.Id
                                                  where m.Id == messageBoardResponseId
                                                  select new
                                                  {
                                                      m.Response,
                                                      m.ResponseDateTime,
                                                      m.Id,
                                                      aspnetusers.FirstName,
                                                      aspnetusers.LastName,
                                                  }).FirstOrDefault();

                if (messageBoardResponsesQuery == null)
                {
                    TempData["error"] = "Invalid message board response id provided";
                    return RedirectToAction(nameof(Home));
                }
                
                AddReplyToThreadCommentViewModel addReplyToThreadCommentViewModel = new AddReplyToThreadCommentViewModel
                {
                    NewReplyerId = userClaim.Value,
                    OriginalMessageBoardResponse = messageBoardResponsesQuery.Response,
                    OriginalMessageBoardResponseDateTime = messageBoardResponsesQuery.ResponseDateTime,
                    OriginalMessageBoardResponseId = messageBoardResponsesQuery.Id,
                    OriginalMessageBoardResponseAuthorName = $"{messageBoardResponsesQuery.FirstName} {messageBoardResponsesQuery.LastName}"
                };

                if (addReplyToThreadCommentViewModel == null)
                {
                    TempData["error"] = "Invalid message board response provided. The message board response " +
                        "could not be found in the database.";
                    return RedirectToAction(nameof(Home));
                }

                return View(addReplyToThreadCommentViewModel);
            }

            return RedirectToAction(nameof(Home)); // throw error in the future
        }

        [HttpPost]
        public IActionResult AddReplyToThreadComment(AddReplyToThreadCommentViewModel addReplyToThreadCommentViewModel)
        {
            if (ModelState.IsValid)
            {
                ReplyToMessageBoardResponse replyToMessageBoardResponse = new ReplyToMessageBoardResponse
                {
                    ReplyDateTime = DateTime.Now,
                    Reply = addReplyToThreadCommentViewModel.NewReplyToMessageBoardResponse,
                    MessageBoardResponseId = addReplyToThreadCommentViewModel.OriginalMessageBoardResponseId,
                    ReplyerId = addReplyToThreadCommentViewModel.NewReplyerId
                };

                if (replyToMessageBoardResponse != null)
                {
                    _context.RepliesToMessageBoardResponse.Add(replyToMessageBoardResponse);
                    _context.SaveChanges();
                    TempData["success"] = "Your reply was added successfully";
                    return RedirectToAction(nameof(Home));
                }
            }

            var errors = ModelState.Select(x => x.Value?.Errors)
                          .Where(y => y?.Count > 0)
                          .ToList();


            TempData["error"] = "The data wasn't submitted because it is invalid.";

            return View(addReplyToThreadCommentViewModel); // add error in the future
        }

        public IActionResult DeleteThreadComment(int messageBoardResponseId)
        {
            if (messageBoardResponseId == 0)
            {
                TempData["error"] = "Invalid message board response id provided.";
                return RedirectToAction("Home");
            }
            var messageBoardResponse = _context.MessageBoardResponses.Find(messageBoardResponseId);

            if (messageBoardResponse == null)
            {
                TempData["error"] = "Invalid message board response id provided. The message board response" +
                    "was not found in the database";
                return RedirectToAction("Home"); //throw an error in the future
            }
            
            return View(messageBoardResponse);
        }

        [HttpPost]
        public IActionResult DeleteThreadComment(int messageBoardResponseId, string dummyString)
        {
            if (messageBoardResponseId == 0)
            {
                TempData["error"] = "Invalid message board response id provided.";
                return RedirectToAction("Home");
            }
            var messageBoardResponse = _context.MessageBoardResponses.Find(messageBoardResponseId);

            if (messageBoardResponse == null)
            {
                TempData["error"] = "Invalid message board response id provided. The message board response" +
                    "was not found in the database";
                return RedirectToAction("Home");  //throw an error in the future
            }

            _context.MessageBoardResponses.Remove(messageBoardResponse);
            _context.SaveChanges();
            TempData["success"] = "The comment was deleted successfully";

            return RedirectToAction("Home"); // redirect to a success page in the future.
        }

        public IActionResult DeleteThreadReply(int replyToMessageBoardResponseId)
        {
            if (replyToMessageBoardResponseId == 0)
            {
                TempData["error"] = "Invalid message board reply id provided";
                return RedirectToAction(nameof(Home)); 
            }
            var dbQuery = (from r in _context.RepliesToMessageBoardResponse
                           join aspnetusers in _context.ApplicationUsers
                           on r.ReplyerId equals aspnetusers.Id
                           where r.Id == replyToMessageBoardResponseId
                           select new
                           {
                               r.Id,
                               r.Reply,
                               r.ReplyDateTime,
                               aspnetusers.FirstName,
                               aspnetusers.LastName, 
                           }).FirstOrDefault(); 

            if (dbQuery == null)
            {
                TempData["error"] = "Invalid message board reply id provided. The reply could was not found in the database"; 
                return RedirectToAction(nameof(Home)); // send to an error page in the future
            }

            DeleteThreadReplyViewModel deleteThreadReplyViewModel = new DeleteThreadReplyViewModel
            {
                ReplyToMessageBoardId = dbQuery.Id,
                RunnerName = $"{dbQuery.FirstName} {dbQuery.LastName}",
                Reply = dbQuery.Reply,
                ReplyDateTime = dbQuery.ReplyDateTime
            };

            return View(deleteThreadReplyViewModel);
        }

        [HttpPost]
        public IActionResult DeleteThreadReply(int ReplyToMessageBoardId, string dummyString)
        {
            if (ReplyToMessageBoardId == 0)
            {
                TempData["error"] = "Invalid message board reply id provided";
                return RedirectToAction(nameof(Home));
            }

            ReplyToMessageBoardResponse replyToMessageBoardResponse = _context.RepliesToMessageBoardResponse.Find(ReplyToMessageBoardId);

            if (replyToMessageBoardResponse == null)
            {
                TempData["error"] = "Invalid message board reply id provided. The reply was not found in the database";
                return RedirectToAction(nameof(Home)); // send to an invalid page in the future
            }

            _context.RepliesToMessageBoardResponse.Remove(replyToMessageBoardResponse);
            _context.SaveChanges();
            TempData["success"] = "The reply was deleted successfully";

            return RedirectToAction("Home"); // send to a delete successful page in the future 
        }
    }
}
