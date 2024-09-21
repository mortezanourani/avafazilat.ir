using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Fazilat.Areas.Dashboard.Models;

namespace Fazilat.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(PanelRole.Key) == null)
            {
                HttpContext.Session.SetString(PanelRole.Key, "User");
            }
            string panelRole = HttpContext.Session.GetString(PanelRole.Key); 
            ViewBag.Role = PanelRole.GetTitle(panelRole);
            return View();
        }

        [HttpPost]
        public IActionResult Index(string role)
        {
            HttpContext.Session.SetString(PanelRole.Key, role);
            return RedirectToAction("Index", "Home", new { area = "Dashboard" });
        }
    }
}
