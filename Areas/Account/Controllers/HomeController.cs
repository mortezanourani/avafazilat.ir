using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.IO;
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
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ApplicationDbContext _context;

        public HomeController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserStore<IdentityUser> userStore,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<IdentityUser>)_userStore;
            _context = context;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            return RedirectToAction("Index", "Dashboard");
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

            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                u.PhoneNumber == loginModel.Username
                || u.UserName == loginModel.Username);
            if (user == null)
            {
                TempData["StatusMessage"] = "Error: کاربری با این مشخصات یافت نشد.";
                return View(loginModel);
            }
            var username = user.UserName;

            var result = await _signInManager.PasswordSignInAsync(username, loginModel.Password, loginModel.RememberMe, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError(string.Empty, "نام کاربری یا رمز عبور نادرست است.");
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

            var userByUserName = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == registerModel.NationalCode);
            var userByPhoneNumber = await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == registerModel.PhoneNumber);
            if (userByUserName != null || userByPhoneNumber != null)
            {
                TempData["StatusMessage"] = "Error: کاربری با این مشخصات در سامانه حاضر است.";
                return View(registerModel);
            }

            var user = new ApplicationUser();
            await _userManager.SetUserNameAsync(user, registerModel.NationalCode);
            await _userManager.SetPhoneNumberAsync(user, registerModel.PhoneNumber);

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync("User");
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }

                UserInformation userInfo = new UserInformation();
                userInfo.UserId = user.Id;
                userInfo.FirstName = registerModel.FirstName;
                userInfo.LastName = registerModel.LastName;
                await _context.AddAsync(userInfo);
                await _context.SaveChangesAsync();

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("PersonalInfo", "Home");
            }

            foreach (var error in result.Errors)
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
                TempData.Add("StatusMessage", "Error: نام کاربری وارد شده یافت نشد. شما در حال فعالیت برای تغییر رمز عبور فرد دیگری هستید.");
                return RedirectToAction("Login");
            }

            var result = await _userManager.ChangePasswordAsync(user, passwordModel.CurrentPassword, passwordModel.NewPassword);
            if (result.Succeeded)
            {
                TempData.Add("StatusMessage", "رمز عبور با موفقیت تغییر کرد.");
                return RedirectToAction();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(passwordModel);
        }

        public async Task<IActionResult> PersonalInfo(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            var user = await _userManager.GetUserAsync(User);
            if (!string.IsNullOrEmpty(id))
            {
                user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == id);
            }

            var userInfo = await _context.Information
                .FirstOrDefaultAsync(i => i.UserId == user.Id);

            PersianCalendar persianCalendar = new PersianCalendar();
            DateTime birthDate = (userInfo.BirthDate != null)
                ? (DateTime)userInfo.BirthDate
                : DateTime.Now;
            var birthDateDay = persianCalendar.GetDayOfMonth(birthDate);
            var birthDateMonth = persianCalendar.GetMonth(birthDate);
            var birthDateYear = persianCalendar.GetYear(birthDate);

            string nationalCode = await _userManager.GetUserNameAsync(user);
            string phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            InformationModel personalInfo = new InformationModel()
            {
                UserId = user.Id,
                NationalCode = nationalCode,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Day = birthDateDay,
                Month = birthDateMonth,
                Year = birthDateYear,
                PhoneNumber = phoneNumber,
                Province = userInfo.Province,
                BirthCertificate = userInfo.BirthCertificate,
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

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == personalInfo.UserId);

            PersianCalendar persianCalendar = new PersianCalendar();
            var birthDate = persianCalendar.ToDateTime(
                personalInfo.Year,
                personalInfo.Month,
                personalInfo.Day,
                0, 0, 0, 0);

            if (personalInfo.BirthCertificateFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await personalInfo.BirthCertificateFile.CopyToAsync(memoryStream);
                    if (memoryStream.Length < 1048576)
                    {
                        personalInfo.BirthCertificate = memoryStream.ToArray();
                    }
                    else
                    {
                        TempData.Add("StatusMessage", "Error: حجم فایل انتخاب شده بیش از یک مگابایت است.");
                        return View(personalInfo);
                    }
                }
            }

            var userInfo = new UserInformation()
            {
                UserId = user.Id,
                FirstName = personalInfo.FirstName,
                LastName = personalInfo.LastName,
                BirthDate = birthDate,
                Province = personalInfo.Province,
                BirthCertificate = personalInfo.BirthCertificate
            };

            try
            {
                await _userManager.SetPhoneNumberAsync(user, personalInfo.PhoneNumber);
                _context.Attach(userInfo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                TempData.Add("StatusMessage", "مشخصات فردی با موفقیت به روز رسانی شد.");
            }
            catch (Exception exception)
            {
                TempData.Add("StatusMessage", exception.Message);
            }
            return RedirectToAction();
        }
    }
}
