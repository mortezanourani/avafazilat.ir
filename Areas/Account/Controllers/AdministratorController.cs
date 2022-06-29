using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using System;
using Fazilat.Data;
using Fazilat.Models;

namespace Fazilat.Areas.Account.Controllers
{
    [Area("Account")]
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdministratorController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Adviser(string id)
        {
            var admin = await _userManager.GetUserAsync(User);

            if (id != null)
            {
                var adviser = await _context.Users
                    .Include(u => u.Information)
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (adviser != null)
                {
                    try
                    {
                        await _userManager.AddToRoleAsync(adviser, "Adviser");
                        TempData["StatusMessage"] = string.Format("{0} {1} promoted to adviser.",
                            adviser.Information.FirstName, adviser.Information.LastName);
                    }
                    catch (Exception exception)
                    {
                        TempData["StatusMessage"] = string.Format("Error: {0}.",
                            exception.Message);
                    }
                }
                return RedirectToAction("Adviser", new { id = string.Empty });
            }

            var adviserRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == "Adviser");
            var adviserRoleId = adviserRole.Id;

            var advisers = await _context.UserRoles
                .Where(ur => ur.RoleId == adviserRoleId)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var accounts = await _context.Users
                .Include(u => u.Information)
                .Where(u => u.Id != admin.Id)
                .Where(u => u.Information.NationalCode != null)
                .Where(u => !advisers.Contains(u.Id))
                .OrderBy(u => u.Information.LastName)
                .ToListAsync();

            if (accounts.Count == 0)
            {
                TempData["StatusMessage"] = "Error: There is no other completed account.";
            }

            return View(accounts);
        }
    }
}
