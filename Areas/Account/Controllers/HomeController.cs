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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly FazilatContext _context;

        public HomeController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IUserStore<ApplicationUser> userStore,
            FazilatContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = (IUserEmailStore<ApplicationUser>)_userStore;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
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

        [Route("Account/Profile/")]
        public async Task<IActionResult> PersonalInfo(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            var user = await _userManager.GetUserAsync(User);
            if (!string.IsNullOrEmpty(id))
            {
                user = await _userManager.FindByIdAsync(id);
            }

            var userInfo = await _context.UserInformations
                .FirstOrDefaultAsync(i => i.UserId == user.Id);

            PersianCalendar persianCalendar = new PersianCalendar();
            DateTime birthDate = (userInfo.BirthDate != null)
                ? userInfo.BirthDate.Value.ToDateTime(TimeOnly.MinValue)
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

            var user = await _userManager.FindByIdAsync(personalInfo.UserId);

            PersianCalendar persianCalendar = new PersianCalendar();
            var birthDate = DateOnly.FromDateTime(persianCalendar.ToDateTime(
                personalInfo.Year,
                personalInfo.Month,
                personalInfo.Day,
                0, 0, 0, 0));

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
