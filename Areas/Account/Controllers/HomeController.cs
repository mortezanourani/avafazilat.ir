using Microsoft.AspNetCore.Mvc;

namespace Fazilat.Areas.Account.Controllers
{
    [Area("Account")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            return View();
        }
    }
}
