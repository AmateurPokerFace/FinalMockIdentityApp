using Microsoft.AspNetCore.Mvc;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    public class MessageBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
