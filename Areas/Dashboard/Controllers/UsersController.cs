using Fazilat.Areas.Dashboard.Models;
using Fazilat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fazilat.Areas.Dashboard.Controllers;

[Area("Dashboard")]
[Authorize(Roles = "Administrator")]
public class UsersController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [Route("Dashboard/User/{id?}")]
    public async Task<IActionResult> Index(string id)
    {
        ApplicationUser user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == id);
        if(user == null)
        {
            return NotFound();
        }

        UserSettingsViewModel model = new UserSettingsViewModel();
        SettingsProfile profile = new SettingsProfile();
        profile.UserName = user.UserName;
        profile.FirstName = user.FirstName;
        profile.LastName = user.LastName;
        profile.BirthDate = user.BirthDate.ToString();
        model.Profile = profile;

        SettingsCommunication communication = new SettingsCommunication();
        communication.PhoneNumber = user.PhoneNumber;
        communication.Email = user.Email;
        model.Communication = communication;

        return View(model);
    }
}
