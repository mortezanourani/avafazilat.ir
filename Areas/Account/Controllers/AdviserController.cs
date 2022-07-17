using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using System;
using Fazilat.Data;
using Fazilat.Models;

namespace Fazilat.Areas.Account.Controllers
{
    [Area("Account")]
    [Authorize(Roles = "Adviser")]
    public class AdviserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdviserController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var adviser = await _userManager.GetUserAsync(User);
            var adviserId = adviser.Id;

            var studentsId = await _context.Advisers
                .Where(a => a.AdviserId == adviserId)
                .Select(a => a.StudentId)
                .ToListAsync();

            var students = await _context.Users
                .Include(u => u.Information)
                .Where(u => studentsId.Contains(u.Id))
                .ToListAsync();

            return View(students);
        }
    }
}
