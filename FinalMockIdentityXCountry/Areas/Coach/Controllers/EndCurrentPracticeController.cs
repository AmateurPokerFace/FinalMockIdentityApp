using FinalMockIdentityXCountry.Models;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Classes;
using FinalMockIdentityXCountry.Models.DataLayer.Repositories.IRepository.Interfaces;
using FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace FinalMockIdentityXCountry.Areas.Coach.Controllers
{
    [Authorize(Roles = "Master Admin, Coach")]
    [Area("Coach")]
    public class EndCurrentPracticeController : Controller
    {

        private readonly XCountryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // the UserManager object in question
        public EndCurrentPracticeController(XCountryDbContext context, UserManager<IdentityUser> userManager)
        { 
            _context = context;
            _userManager = userManager;
        }

        public IActionResult End(int practiceId = 1)
        {
            //var test = _unitOfWork.Practice.GetAll()
            //    .Join(_unitOfWork.Attendance.GetAll(), p => p.Id, a => a.PracticeId,
            //    (p, a) => new { Practice = p, Attendance = a }).Select(c => c.Practice.CoachId);

            //var test2 = _unitOfWork.Practice.GetAll()
            //    .Join(_unitOfWork.Attendance.GetAll(), p => p.Id, a => a.PracticeId,
            //    (p, a) => new { Practice = p, Attendance = a });

            //usage: var s = repository.GetAll(i => i.Name, false, i => i.NavigationProperty);
            //var test4 = (from a in _context.Attendances
            //               join p in _context.Practices on a.PracticeId equals p.Id into lg
            //               from x in lg.DefaultIfEmpty()
            //               select new
            //               {
            //                   a.AttendanceDate,
            //                   a.Runner,
            //                   a.IsPresent,
            //                   x.PracticeIsInProgress,
            //                   x.CoachId
            //               }).ToList();

            var test5 = _context.Attendances.Include(p => p.PracticeId).ToList();
            var test6 = _context.Attendances.Include(p => p.PracticeId == 1).ToList();

             
            return View(); 
        }
    }
}
