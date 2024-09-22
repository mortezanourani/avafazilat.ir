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
            if (HttpContext.Session.GetString("PanelRole") == null)
            {
                HttpContext.Session.SetString("PanelRole", "User");
            }

            ApplicationRole panelRole = await _roleManager.Roles
                .FirstOrDefaultAsync(r => r.Name == HttpContext.Session.GetString("PanelRole"));
            ViewBag.Role = panelRole.PersianName;
            return View();
        }

        [HttpPost]
        public IActionResult Index(string role)
        {
            HttpContext.Session.SetString("PanelRole", role);
            return RedirectToAction("Index", "Home", new { area = "Dashboard" });
        }
    }
}
