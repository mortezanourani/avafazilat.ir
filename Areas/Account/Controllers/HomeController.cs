using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Fazilat.Areas.Account.Models;
using Fazilat.Models;

namespace Fazilat.Areas.Account.Controllers
{
    [Area("Account")]
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;

        public HomeController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<ApplicationUser> userStore)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<ApplicationUser>)_userStore; 
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(loginModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }

            var user = new ApplicationUser();
            await _userStore.SetUserNameAsync(user, registerModel.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, registerModel.Email, CancellationToken.None);

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync("User");
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index");
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(registerModel);
        }

        public IActionResult ChangePassword()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordModel passwordModel)
        {
            if (!ModelState.IsValid)
            {
                return View(passwordModel);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.ChangePasswordAsync(user, passwordModel.CurrentPassword, passwordModel.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(passwordModel);
        }
    }
}
