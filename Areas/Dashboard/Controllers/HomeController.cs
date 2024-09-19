using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fazilat.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string role)
        {
            ViewBag.Role = role;
            return View();
        }
    }
}
