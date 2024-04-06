using Fazilat.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Fazilat.Models;

namespace Fazilat.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([Bind("Username,FirstName,LastName,PhoneNumber,Password,PasswordRetype")] AccountSignUpViewModel signUpModel)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser user = new ApplicationUser();
            await _userManager.SetUserNameAsync(user, signUpModel.Username);
            await _userManager.SetPhoneNumberAsync(user, signUpModel.PhoneNumber);
            var result = await _userManager.CreateAsync(user, signUpModel.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        ModelState.AddModelError(string.Empty, "خطایی رخ داده است.");
        return View(signUpModel);
    }

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn([Bind("Username,Password")] AccountSignInViewModel signInModel)
    {
        var user = await _userManager.Users.Where(
            u => u.UserName == signInModel.Username || u.PhoneNumber == signInModel.Username)
            .FirstOrDefaultAsync();
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "کاربری با این مشخصات ثبت نام نکرده است.");
        }
        else
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, signInModel.Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Dashboard", new { @area = "Account" });
            }

            ModelState.AddModelError(string.Empty, "خطایی رخ داده است.");
        }
        return View(signInModel);
    }

    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(AccountChangePasswordViewModel changePasswordModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, changePasswordModel.OldPassword, changePasswordModel.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { @area = "Panel" });
                }
            }
        }

        ModelState.AddModelError(string.Empty, "خطایی رخ داده است.");
        return View(changePasswordModel);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
