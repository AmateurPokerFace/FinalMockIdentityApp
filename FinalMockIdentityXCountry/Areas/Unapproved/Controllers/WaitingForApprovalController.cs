using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalMockIdentityXCountry.Areas.Unapproved.Controllers
{
    [Authorize(Roles = "Waiting for approval")]
    [Area("Unapproved")]
    public class WaitingForApprovalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
