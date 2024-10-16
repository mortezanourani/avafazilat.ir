﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims;
using Fazilat.Models;
using Fazilat.Areas.Account.Models;
using System;

namespace Fazilat.Areas.Account.Controllers;

[Area("Account")]
public class HomeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly SignInManager<ApplicationUser> _signInManager;

    public HomeController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [Route("Account/")]
    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home", new { area = "Dashboard" });
        }

        AccountViewModel model = new AccountViewModel();
        model.isLoginPage = true;
        return View(model);
    }

    [HttpPost]
    [Route("Account/")]
    public async Task<IActionResult> Index(AccountViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        bool isLoginProcess = model.isLoginPage;
        if (isLoginProcess)
        {
            LoginViewModel loginModel = model.Login;

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == loginModel.PhoneNumber);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "کاربری با این مشخصات یافت نشد.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, true, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "نام کاربری یا رمز عبور نادرست است.");
                return View(model);
            }

            return RedirectToAction("Index", "Home", new { area = "Dashboard" });
        }
        else
        {
            RegisterViewModel registerModel = model.Register;

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == registerModel.PhoneNumber);

            if (user != null)
            {
                ModelState.AddModelError(string.Empty, "کاربری با این مشخصات در سامانه حاضر است.");
                return View(model);
            }

            user = new ApplicationUser();
            user.FirstName = registerModel.FirstName;
            user.LastName = registerModel.LastName;
            user.Registered = DateOnly.FromDateTime(DateTime.Now);
            await _userManager.SetUserNameAsync(user, registerModel.PhoneNumber);
            await _userManager.SetPhoneNumberAsync(user, registerModel.PhoneNumber);
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Expiration,  DateTime.Now.AddMonths(1).ToString()));
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Expired, "Active"));

            await _userManager.AddToRoleAsync(user, "User");

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home", new { area = "Dashboard" });
        }
    }

    [Authorize]
    [HttpPost]
    [Route("Account/SignOut/")]
    public async Task<IActionResult> SignOut()
    {
        HttpContext.Session.Clear();
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home", new { @area = string.Empty });
    }
}
