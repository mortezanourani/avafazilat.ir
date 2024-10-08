using Fazilat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fazilat.Areas.Dashboard.Controllers;

[Area("Dashboard")]
[Authorize]
public class SettingsController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public SettingsController(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [Route("Dashboard/Settings/")]
    public async Task<IActionResult> Index()
    {
        ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);
        return View(user);
    }

    [Authorize("Administrator")]
    [Route("Dashboard/Settings/{id?}")]
    public async Task<IActionResult> Index(string id)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }
}
