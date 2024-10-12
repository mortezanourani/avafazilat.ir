using Fazilat.Areas.Dashboard.Models;
using Fazilat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
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

    [HttpGet]
    [Route("Dashboard/Settings/")]
    public async Task<IActionResult> Index()
    {
        ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

        SettingsProfile profile = new SettingsProfile();
        profile.Id = user.Id;
        profile.UserName = user.UserName;
        profile.FirstName = user.FirstName;
        profile.LastName = user.LastName;
        profile.BirthDate = user.BirthDate.ToString();

        SettingsCommunication communication = new SettingsCommunication();
        communication.PhoneNumber = user.PhoneNumber;
        communication.Email = user.Email;

        SettingsViewModel settings = new SettingsViewModel();
        settings.Profile = profile;
        settings.Communication = communication;

        return View(settings);
    }

    [HttpPost]
    [Route("Dashboard/Settings/")]
    public async Task<IActionResult> Index(SettingsViewModel settings)
    {
        ApplicationUser user = await _userManager.FindByNameAsync(User.Identity.Name);

        SettingsProfile profile = new SettingsProfile();
        profile.Id = user.Id;
        profile.UserName = user.UserName;
        profile.FirstName = user.FirstName;
        profile.LastName = user.LastName;
        profile.BirthDate = user.BirthDate.ToString();

        SettingsCommunication communication = new SettingsCommunication();
        communication.PhoneNumber = user.PhoneNumber;
        communication.Email = user.Email;

        SettingsViewModel model = new SettingsViewModel();
        model.Profile = profile;
        model.Communication = communication;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        return RedirectToAction("Index");
    }
}
