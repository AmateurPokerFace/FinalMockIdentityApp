using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using FinalMockIdentityXCountry.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalMockIdentityXCountry.Areas.Runner.Controllers
{
    public class ForumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        //public IActionResult ViewThread(int messageBoardId)
        //{
        //    ViewThreadViewModel viewThreadViewModel = new ViewThreadViewModel();
        //    viewThreadViewModel.MessageBoard = _context.MessageBoards.Find(messageBoardId);
        //    if (viewThreadViewModel.MessageBoard == null)
        //    {
        //        return RedirectToAction(nameof(Home)); // return a invalid id provided page => Make this a partial view
        //    }

        //    viewThreadViewModel.DatabaseMessageBoardResponses = (IEnumerable<MessageBoardResponse>?)_context.MessageBoardResponses
        //        .OrderByDescending(d => d.ResponseDateTime.Date)
        //        .Where(m => m.MessageBoardId == messageBoardId);

        //    return View(viewThreadViewModel);
        //}
    }
}
