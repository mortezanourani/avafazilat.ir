using Fazilat.Data;
using Fazilat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace Fazilat.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public HomeController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }
    
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString(ApplicationKeys.PanleRoleKey) == null)
            {
                HttpContext.Session.SetString(ApplicationKeys.PanleRoleKey, "User");
            }

            ApplicationRole panelRole = await _roleManager.Roles
                .FirstOrDefaultAsync(r => r.Name == HttpContext.Session.GetString(ApplicationKeys.PanleRoleKey));
            ViewBag.Role = panelRole.PersianName;
            return View();
        }

        [HttpPost]
        public IActionResult Index(string role)
        {
            HttpContext.Session.SetString(ApplicationKeys.PanleRoleKey, role);
            return RedirectToAction("Index", "Home", new { area = "Dashboard" });
        }
    }
}
