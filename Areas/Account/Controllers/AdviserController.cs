using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using System.Linq;
using System;
using Fazilat.Data;
using Fazilat.Models;
using Fazilat.Areas.Account.Models;

namespace Fazilat.Areas.Account.Controllers
{
    [Area("Account")]
    [Authorize(Roles = "Adviser")]
    public class AdviserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdviserController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var adviser = await _userManager.GetUserAsync(User);
            var adviserId = adviser.Id;

            var studentsId = await _context.Advisers
                .Where(a => a.AdviserId == adviserId)
                .Select(a => a.StudentId)
                .ToListAsync();

            var students = await _context.Users
                .Include(u => u.Information)
                .Where(u => studentsId.Contains(u.Id))
                .ToListAsync();

            if(students.Count == 0)
            {
                TempData["StatusMessage"] = "Error: برای شما هیچ داوطلبی تعیین نشده است.";
            }

            return View(students);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string Search)
        {
            var adviser = await _userManager.GetUserAsync(User);
            var adviserId = adviser.Id;

            var adviserStudents = await _context.Advisers
                .Where(a => a.AdviserId == adviserId)
                .Select(a => a.StudentId)
                .ToListAsync();

            var result = _context.Users
                .Include(u => u.Information)
                .AsEnumerable()
                .Where(u => u.UserName == Search
                    || u.PhoneNumber == Search
                    || u.Information.FirstName == Search
                    || u.Information.LastName == Search
                    || u.Information.FullName == Search)
                .Where(u => adviserStudents.Contains(u.Id))
                .ToList();

            if (result.Count == 0)
            {
                TempData["StatusMessage"] = "Error: نتیجه ای یافت نشد.";
                return RedirectToAction();
            }
            return View(result);
        }

        public async Task<IActionResult> Message(string id)
        {
            TempData["StudentId"] = id;

            var messages = await _context.Messages
                .Where(m => m.SenderId == id || m.ReceiverId == id)
                .OrderBy(m => m.Created)
                .Skip(Math.Max(0, _context.Messages.Count() - 10))
                .ToListAsync();

            if(messages.Count == 0)
            {
                TempData["StatusMessage"] = "هنوز پیامی ارسال نشده است.";
            }

            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Message(Message formCollection)
        {
            if (!ModelState.IsValid)
            {
                return View(formCollection);
            }

            var user = await _userManager.GetUserAsync(User);
            Message message = new Message()
            {
                Id = Guid.NewGuid().ToString(),
                SenderId = user.Id,
                ReceiverId = formCollection.ReceiverId,
                Text = formCollection.Text,
                Created = DateTime.Now,
            };
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return RedirectToAction();
        }

        public async Task<IActionResult> Curricula(string id)
        {
            var curricula = await _context.Curricula
                .Where(c => c.UserId == id)
                .OrderByDescending(c => c.StartDate)
                .ToListAsync();
            return View(curricula);
        }

        [HttpPost]
        public async Task<IActionResult> Curricula(CurriculumModel formCollection)
        {
            formCollection.Id = Guid.NewGuid().ToString();
            PersianCalendar persianCalendar = new PersianCalendar();
            formCollection.StartDate = persianCalendar.ToDateTime(
                formCollection.Year,
                formCollection.Month,
                formCollection.Day,
                0, 0, 0, 0);
            try
            {
                await _context.AddAsync(formCollection);
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "عملیات با موفقیت انجام شد.";
                return RedirectToAction("Curricula", new { @id = formCollection.UserId });
            }
            catch
            {
                TempData["StatusMessage"] = "Error: خطایی رخ داده است. لطفا مجددا تلاش نمایید.";
            }
            return View(formCollection);
        }

        public async Task<IActionResult> Curriculum(string id)
        {
            var curriculum = await _context.Curricula
                .Include(c => c.Courses)
                .FirstOrDefaultAsync(c => c.Id == id);
            if(curriculum == null)
            {
                TempData["StatusMessage"] = "Error: برنامه مطالعاتی با این مشخصات وجود ندارد.";
                return RedirectToAction("Curricula");
            }

            PersianCalendar persianCalendar = new PersianCalendar();
            var startDateDay = persianCalendar.GetDayOfMonth(curriculum.StartDate);
            var startDateMonth = persianCalendar.GetMonth(curriculum.StartDate);
            var startDateYear = persianCalendar.GetYear(curriculum.StartDate);
            var curriculumModel = new CurriculumModel()
            {
                Id = curriculum.Id,
                UserId = curriculum.UserId,
                Title = curriculum.Title,
                Description = curriculum.Description,
                StartDate = curriculum.StartDate,
                Day = startDateDay,
                Month = startDateMonth,
                Year = startDateYear,
                Courses = curriculum.Courses,
            };
            return View(curriculumModel);
        }

        [HttpPost]
        public async Task<IActionResult> Curriculum(CurriculumModel formCollection)
        {
            var curriculum = await _context.Curricula
                .FirstOrDefaultAsync(c => c.Id == formCollection.Id);

            PersianCalendar persianCalendar = new PersianCalendar();
            formCollection.StartDate = persianCalendar.ToDateTime(
                formCollection.Year,
                formCollection.Month,
                formCollection.Day,
                0, 0, 0, 0);
            try
            {
                curriculum.Title = formCollection.Title;
                curriculum.Description = formCollection.Description;
                curriculum.StartDate = formCollection.StartDate;
                _context.Attach(curriculum).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "عملیات با موفقیت انجام شد.";
                return RedirectToAction("Curriculum", new { @id = curriculum.Id });
            }
            catch
            {
                TempData["StatusMessage"] = "Error: خطایی رخ داده است. لطفا مجددا تلاش نمایید.";
            }
            return View(formCollection);
        }

        public async Task<IActionResult> Course(string id)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id);
            if (course != null)
            {
                _context.Remove(course);
            }

            var curriculum = await _context.Curricula
                .FirstOrDefaultAsync(c => c.Id == id);
            if (curriculum != null)
            {
                course = new Course()
                {
                    Id = Guid.NewGuid().ToString(),
                    CurriculumId = id,
                    Accomplished = false,
                };
                await _context.AddAsync(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Curriculum", new { id = course.CurriculumId });
        }
    }
}
