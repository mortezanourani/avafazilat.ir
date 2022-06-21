using Microsoft.AspNetCore.Mvc;

namespace Fazilat.Areas.Account.Controllers
{
    [Area("Account")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
