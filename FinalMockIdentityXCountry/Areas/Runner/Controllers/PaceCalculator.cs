using FinalMockIdentityXCountry.Models.Utilities;
using FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels;
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

        public IActionResult CustomCalculator() 
        {
            CustomCalculatorViewModel model = new CustomCalculatorViewModel
            { 
                Minutes = 0,
                Distance = 0,
                Hours = 0,
                Seconds = 0,
                PaceString = "Enter values for calculation"
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CustomCalculator(CustomCalculatorViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var pace = StaticPaceCalculator.CalculatePace(model.Hours, model.Minutes, model.Seconds, model.Distance);

                model.PaceString = pace.Item1 > 0 ? $"{pace.Item1} Minutes" : $"{pace.Item1} Minute";
                model.PaceString += pace.Item2 > 0 ? $" and {pace.Item2} Seconds per mile" : $" per mile";
                
                return View(model);
            }

            return View();
        }
    }
}
