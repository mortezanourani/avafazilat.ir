using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Fazilat.Areas.Account.Models;
using Fazilat.Models;
using Fazilat.Data;

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
        private readonly ApplicationDbContext _context;

        public HomeController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<ApplicationUser> userStore,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
            _context = context;
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
                TempData.Add("StatusMessage", "Error: User not found. Make sure you typed your username correctly.");
                return RedirectToAction("Login");
            }

            var result = await _userManager.ChangePasswordAsync(user, passwordModel.CurrentPassword, passwordModel.NewPassword);
            if (result.Succeeded)
            {
                TempData.Add("StatusMessage", "Password has been changed.");
                return RedirectToAction();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(passwordModel);
        }

        public async Task<IActionResult> PersonalInfo()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            var user = await _userManager.GetUserAsync(User);

            var userInfo = await _context.Information
                .FirstOrDefaultAsync(i => i.UserId == user.Id);
            if (userInfo == null)
            {
                userInfo = new UserInformation();
                userInfo.UserId = user.Id;
                await _context.AddAsync(userInfo);
                await _context.SaveChangesAsync();
            }

            PersianCalendar persianCalendar = new PersianCalendar();
            DateTime birthDate = (DateTime) userInfo.BirthDate;
            var day = persianCalendar.GetDayOfMonth(birthDate);
            var month = persianCalendar.GetMonth(birthDate);
            var year = persianCalendar.GetYear(birthDate);

            InformationModel personalInfo = new InformationModel()
            {
                NationalCode = userInfo.NationalCode,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Day = day,
                Month = month,
                Year = year
            };
            return View(personalInfo);
        }

        [HttpPost]
        public async Task<IActionResult> PersonalInfo(InformationModel personalInfo)
        {
            if (!ModelState.IsValid)
            {
                return View(personalInfo);
            }

            var user = await _userManager.GetUserAsync(User);

            PersianCalendar persianCalendar = new PersianCalendar();
            var birthDate = persianCalendar.ToDateTime(
                personalInfo.Year,
                personalInfo.Month,
                personalInfo.Day,
                0, 0, 0, 0);

            var userInfo = new UserInformation()
            {
                UserId = user.Id,
                NationalCode = personalInfo.NationalCode,
                FirstName = personalInfo.FirstName,
                LastName = personalInfo.LastName,
                BirthDate = birthDate,
            };

            try
            {
                _context.Attach(userInfo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                TempData.Add("StatusMessage", "Information updated successfully.");
                return RedirectToAction();
            }
            catch (Exception exception)
            {
                TempData.Add("StatusMessage", exception.Message);
                return RedirectToAction();
            }

            return View();
        }
    }
}
