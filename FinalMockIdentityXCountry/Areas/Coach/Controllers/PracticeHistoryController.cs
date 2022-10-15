using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.Delete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class PracticeHistoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly UserManager<IdentityUser> _userManager;
        public PracticeHistoryController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        
        public IActionResult History()
        {
            PracticeHistoryViewModel practiceHistoryViewModel = new PracticeHistoryViewModel();
            //practiceHistoryViewModel.Attendances = _unitOfWork.Attendance.GetAll()
            //    .GroupBy(i => i.PracticeId)
            //    .Select(p => p.First())
            //    .ToList();

            return View(practiceHistoryViewModel);
        }
    }
}
