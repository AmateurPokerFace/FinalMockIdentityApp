using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinalMockIdentityXCountry.Areas.Runner.Controllers
{
    [Authorize(Roles = "Master Admin, Coach, Runner")]
    [Area("Runner")]
    public class PaceCalculator : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SelectCalculator()
        {
            return View();
        }

        public IActionResult JackDanielsVDOTCalculator()
        {
            return View();
        }

        public IActionResult OmniCalculator()
        {
            return View();
        }
    }
}
