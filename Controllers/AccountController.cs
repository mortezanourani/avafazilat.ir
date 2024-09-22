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

    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Administrator", new { area = "Account" });
        }

        AccountViewModel model = new AccountViewModel();
        model.isLoginPage = true;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(AccountViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        bool isLoginProcess = model.isLoginPage;
        if(isLoginProcess)
        {
            AccountLoginViewModel loginModel = model.Login;

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

            return RedirectToAction("Index", "Administrator", new { area = "Account" });
            //return RedirectToAction("Index", "Dashboard");
        }
        else
        {
            AccountRegisterViewModel registerModel = model.Register;

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

            await _userManager.AddToRoleAsync(user, "User");

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Administrator", new { area = "Account" });
            //return RedirectToAction("Index", "Dashboard");
        }
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
        HttpContext.Session.Clear();
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
