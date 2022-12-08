using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalMockIdentityXCountry.Areas.Banned.Controllers
{
    public class BannedController : Controller
    {
        [Authorize(Roles = "Banned user")]
        [Area("Banned")]
        public IActionResult Index()
        { 
            return View();
        }
    }
}
