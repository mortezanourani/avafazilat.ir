using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using System;
using Fazilat.Data;
using Fazilat.Models;
using Fazilat.Areas.Account.Models;

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

        public async Task<IActionResult> Advisers(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var adviser = await _context.Users
                    .Include(u => u.Information)
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (adviser != null)
                {
                    try
                    {
                        await _userManager.RemoveFromRoleAsync(adviser, "Adviser");
                        TempData["StatusMessage"] = string.Format("{0} {1} از فهرست مشاوران خارج شد.",
                            adviser.Information.FirstName, adviser.Information.LastName);
                    }
                    catch (Exception exception)
                    {
                        TempData["StatusMessage"] = string.Format("Error: {0}",
                            exception.Message);
                    }
                }
                return RedirectToAction("Advisers", new { id = string.Empty });
            }

            var adviserRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == "Adviser");
            var adviserRoleId = adviserRole.Id;

            var advisersList = await _context.UserRoles
                .Where(ur => ur.RoleId == adviserRoleId)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var advisers = await _context.Users
                .Include(u => u.Information)
                .Where(u => advisersList.Contains(u.Id))
                .OrderBy(u => u.Information.LastName)
                .ToListAsync();

            if (advisers == null)
            {
                TempData["StatusMessage"] = "Error: هیچ مشاوری در سامانه حضور ندارد.";
            }
            return View(advisers);
        }

        public async Task<IActionResult> Users(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var user = await _context.Users
                    .Include(u => u.Information)
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (user != null)
                {
                    try
                    {
                        await _userManager.AddToRoleAsync(user, "Adviser");
                        TempData["StatusMessage"] = string.Format("{0} {1} به فهرست مشاوران اضافه شد.",
                            user.Information.FirstName, user.Information.LastName);
                    }
                    catch (Exception exception)
                    {
                        TempData["StatusMessage"] = string.Format("Error: {0}",
                            exception.Message);
                    }
                }
                return RedirectToAction("Users", new { id = string.Empty });
            }

            var adviserRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == "Adviser");
            var adviserRoleId = adviserRole.Id;
            var advisersList = await _context.UserRoles
                .Where(ur => ur.RoleId == adviserRoleId)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var userRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == "User");
            var userRoleId = userRole.Id;
            var usersList = await _context.UserRoles
                .Where(ur => ur.RoleId == userRoleId)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var users = await _context.Users
                .Include(u => u.Information)
                .Where(u => usersList.Contains(u.Id))
                .Where(u => !advisersList.Contains(u.Id))
                .OrderBy(u => u.Information.LastName)
                .ToListAsync();

            if (users == null)
            {
                TempData["StatusMessage"] = "Error: هیچ کاربری در سامانه ثبت نام نکرده است.";
            }
            return View(users);
        }

        public async Task<IActionResult> AssignAdviser(string id)
        {
            var student = await _context.Users
                .Include(u => u.Information)
                .FirstOrDefaultAsync(u => u.Id == id);

            var studentAdviser = await _context.Advisers
                .FirstOrDefaultAsync(a => a.StudentId == id);
            if(studentAdviser == null)
            {
                studentAdviser = new Adviser()
                {
                    Id = Guid.NewGuid().ToString(),
                    StudentId = id,
                    AdviserId = null,
                };
            }

            var adviser = await _context.Users
                .Include(a => a.Information)
                .FirstOrDefaultAsync(a => a.Id == studentAdviser.AdviserId);

            var adviserRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == "Adviser");
            var adviserRoleId = adviserRole.Id;
            var advisersList = await _context.UserRoles
                .Where(ur => ur.RoleId == adviserRoleId)
                .Select(ur => ur.UserId)
                .ToListAsync();
            var advisers = await _context.Users
                .Include(u => u.Information)
                .Where(u => advisersList.Contains(u.Id))
                .OrderBy(u => u.Information.LastName)
                .ToListAsync();

            var adviserModel = new AdviserAssignmentModel()
            {
                Id = studentAdviser.Id,
                Student = student,
                StudentId = student.Id,
                Adviser = adviser,
                AdviserId = (adviser == null) ? null: adviser.Id,
                Advisers = advisers
            };

            return View(adviserModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssignAdviser(AdviserAssignmentModel formCollection)
        {
            Adviser adviser = await _context.Advisers
                .FirstOrDefaultAsync(a => a.Id == formCollection.Id);
            if(adviser == null)
            {
                adviser = new Adviser()
                {
                    Id = formCollection.Id,
                    StudentId = formCollection.StudentId,
                    AdviserId = formCollection.AdviserId
                };
                await _context.AddAsync(adviser);
            }
            else
            {
                adviser.AdviserId = formCollection.AdviserId;
                _context.Attach(adviser).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            TempData["StatusMessage"] = "عملیات با موفقیت انجام شد.";
            return RedirectToAction("Users");
        }
    }
}
