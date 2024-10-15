using Fazilat.Areas.Dashboard.Models;
using Fazilat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Configuration;
using System;
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

    [Route("Dashboard/User/{id?}/")]
    public async Task<IActionResult> Index(string id)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id);
        if(user == null)
        {
            return NotFound();
        }

        UserSettingsViewModel model = new UserSettingsViewModel();
        model.Id = user.Id;

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

    [HttpPost]
    [Route("Dashboard/User/{id?}/")]
    public async Task<IActionResult> Index(UserSettingsViewModel settings)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(settings.Id);

        if (!ModelState.IsValid)
        {
            if (settings.Profile == null)
            {
                settings.Profile = new SettingsProfile();
                settings.Profile.UserName = user.UserName;
                settings.Profile.FirstName = user.FirstName;
                settings.Profile.LastName = user.LastName;
                settings.Profile.BirthDate = user.BirthDate.ToString();
            }

            if (settings.Communication == null)
            {
                settings.Communication = new SettingsCommunication();
                settings.Communication.PhoneNumber = user.PhoneNumber;
                settings.Communication.Email = user.Email;
            }

            return View(settings);
        }

        if (settings.Profile != null)
        {
            user.FirstName = settings.Profile.FirstName;
            user.LastName = settings.Profile.LastName;
            if (!settings.Profile.BirthDate.IsNullOrEmpty())
            {
                user.BirthDate = DateOnly.Parse(settings.Profile.BirthDate);
            }
            await _userManager.UpdateAsync(user);

            if (user.UserName != settings.Profile.UserName)
            {
                var setUserName = await _userManager.SetUserNameAsync(user, settings.Profile.UserName);
                if (!setUserName.Succeeded)
                {
                    foreach (var error in setUserName.Errors)
                    {
                        if (error.Code == "DuplicateUserName")
                        {
                            ModelState.AddModelError("Profile.UserName", "نام کاربری وارد شده در سامانه وجود دارد.");
                        }
                    }
                }
            }
        }
        else
        {
            settings.Profile = new SettingsProfile();
            settings.Profile.UserName = user.UserName;
            settings.Profile.FirstName = user.FirstName;
            settings.Profile.LastName = user.LastName;
            settings.Profile.BirthDate = user.BirthDate.ToString();
        }

        if (settings.Communication != null)
        {
            var setPhoneNumber = await _userManager.SetPhoneNumberAsync(user, settings.Communication.PhoneNumber);
            if (!setPhoneNumber.Succeeded)
            {
                foreach (var error in setPhoneNumber.Errors)
                {
                    ModelState.AddModelError("Communication.PhoneNumber", error.Description);
                }
            }

            var setEmail = await _userManager.SetEmailAsync(user, settings.Communication.Email);
            if (!setEmail.Succeeded)
            {
                foreach (var error in setEmail.Errors)
                {
                    ModelState.AddModelError("Communication.Email", error.Description);
                }
            }
        }
        else
        {
            settings.Communication = new SettingsCommunication();
            settings.Communication.PhoneNumber = user.PhoneNumber;
            settings.Communication.Email = user.Email;
        }

        if (settings.Security != null)
        {
            await _userManager.RemovePasswordAsync(user);
            var changePassword = await _userManager.AddPasswordAsync(user, settings.Security.NewPassword);
            if (!changePassword.Succeeded)
            {
                foreach (var error in changePassword.Errors)
                {
                    ModelState.AddModelError("Security.NewPassword", error.Description);
                }
            }
        }

        if (!ModelState.IsValid)
        {
            return View(settings);
        }

        return RedirectToAction("Index", new { @id = settings.Id });
    }
}
