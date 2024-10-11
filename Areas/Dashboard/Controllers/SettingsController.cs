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


        if (TempData.Count > 0)
        {
            foreach (var error in TempData)
            {
                ModelState.AddModelError(string.Empty, error.Value.ToString());
            }
        }
        return View(settings);
    }

    [HttpPost]
    [Route("Dashboard/Settings/UpdateProfile/")]
    public async Task<IActionResult> UpdateProfile([Bind(Prefix = "Profile")] SettingsProfile profile)
    {
        if (!ModelState.IsValid)
        {
            foreach (var modelState in ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    TempData.Add(error.ErrorMessage, error.ErrorMessage);
                }
            }
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("Dashboard/Settings/UpdateCommunication/")]
    public async Task<IActionResult> UpdateCommunication([Bind(Prefix = "Comunication")] SettingsCommunication communication)
    {
        if (!ModelState.IsValid)
        {
            return View("ModelError");
        }

        ModelState.AddModelError(string.Empty, "This is test error.");
        ModelState.AddModelError(string.Empty, "This is second test error.");
        foreach (ModelError error in ModelState.Values.FirstOrDefault().Errors)
        {
            TempData.Add(error.ErrorMessage, error.ErrorMessage);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("Dashboard/Settings/ChangePassword/")]
    public async Task<IActionResult> ChangePassword([Bind(Prefix = "Password")] SettingsPassword password)
    {
        if (!ModelState.IsValid)
        {
            return View("ModelError");
        }

        ModelState.AddModelError(string.Empty, "This is test error.");
        ModelState.AddModelError(string.Empty, "This is second test error.");
        foreach (ModelError error in ModelState.Values.FirstOrDefault().Errors)
        {
            TempData.Add(error.ErrorMessage, error.ErrorMessage);
        }
        return RedirectToAction("Index");
    }

    //[Authorize(Roles = "Administrator")]
    //[Route("Dashboard/Settings/{id?}")]
    //public async Task<IActionResult> Index(string id)
    //{
    //    ApplicationUser user = await _userManager.FindByIdAsync(id);
    //    if (user == null)
    //    {
    //        return NotFound();
    //    }
    //    return View(user);
    //}
}
