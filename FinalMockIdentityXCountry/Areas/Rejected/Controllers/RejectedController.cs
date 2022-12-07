using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalMockIdentityXCountry.Areas.Rejected.Controllers
{
    public class RejectedController : Controller
    {
        [Authorize(Roles = "Rejected")]
        [Area("Rejected")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
