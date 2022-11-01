using Microsoft.AspNetCore.Mvc;

namespace FinalMockIdentityXCountry.Areas.Runner.Controllers
{
    public class ForumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
