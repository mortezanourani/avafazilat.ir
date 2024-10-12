using Fazilat.Areas.Dashboard.Models;
using Fazilat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        profile.UserName = user.UserName;
        profile.FirstName = user.FirstName;
        profile.LastName = user.LastName;
        profile.BirthDate = user.BirthDate.ToString();

        SettingsCommunication communication = new SettingsCommunication();
        communication.PhoneNumber = user.PhoneNumber;
        communication.Email = user.Email;

        SettingsViewModel settings = new SettingsViewModel();
        settings.Id = user.Id;
        settings.Profile = profile;
        settings.Communication = communication;

        return View(settings);
    }

    [HttpPost]
    [Route("Dashboard/Settings/")]
    public async Task<IActionResult> Index(SettingsViewModel settings)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(settings.Id);

        if (!ModelState.IsValid)
        {
            if (settings.Profile == null)
            {
                settings.Profile = new SettingsProfile();
            }
            settings.Profile.UserName = user.UserName;
            settings.Profile.FirstName = user.FirstName;
            settings.Profile.LastName = user.LastName;
            settings.Profile.BirthDate = user.BirthDate.ToString();

            if (settings.Communication == null)
            {
                settings.Communication = new SettingsCommunication();
            }
            settings.Communication.PhoneNumber = user.PhoneNumber;
            settings.Communication.Email = user.Email;

            return View(settings);
        }

        bool authorized = await _userManager.CheckPasswordAsync(user, settings.Password);
        if (!authorized)
        {
            ModelState.AddModelError("Password", "رمزعبور وارد شده صحیح نمی باشد. شما مجاز به ثبت تغییرات اعمال شده نیستید.");
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

                return View(settings);
            }
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

            if (!ModelState.IsValid)
            {
                return View(settings);
            }
        }

        if (settings.Security != null)
        {
            var changePassword = await _userManager.ChangePasswordAsync(user, settings.Password, settings.Security.NewPassword);
            if (!changePassword.Succeeded)
            {
                foreach (var error in changePassword.Errors)
                {
                    ModelState.AddModelError("Security.NewPassword", error.Description);
                }
                return View(settings);
            }
        }

        return RedirectToAction("Index");
    }
}
