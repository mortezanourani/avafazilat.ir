using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Fazilat.Areas.Account.Models;
using Fazilat.Models;
using Fazilat.Data;

namespace Fazilat.Areas.Account.Controllers
{
    [Area("Account")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Index", "Administrator");
            }

            if (User.IsInRole("Adviser"))
            {
                return RedirectToAction("Index", "Adviser");
            }

            var user = await _userManager.GetUserAsync(User);
            List<Curriculum> curricula = await _context.Curricula
                .Where(c => c.UserId == user.Id)
                .Where(c => c.Courses.Count > 0)
                .OrderByDescending(c => c.StartDate)
                .ToListAsync();
            return View(curricula);
        }

        public async Task<IActionResult> EducationalFile(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (!string.IsNullOrEmpty(id))
            {
                user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == id);
            }

            var educationalFile = await _context.EducationalFiles
                .FirstOrDefaultAsync(ef => ef.UserId == user.Id);
            if(educationalFile == null)
            {
                educationalFile = new EducationalFile();
                educationalFile.UserId = user.Id;
                educationalFile.Grade = "10";
                educationalFile.LastAvg = "10.00";
                await _context.AddAsync(educationalFile);
                await _context.SaveChangesAsync();
            }

            EducationalFileModel viewModel = new EducationalFileModel()
            {
                UserId = user.Id,
                Grade = educationalFile.Grade,
                LastAvg = educationalFile.LastAvg,
                RegistrationFormFileName = educationalFile.RegistrationFormFileName,
                LastWorkbookFileName = educationalFile.LastWorkbookFileName,
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EducationalFile(EducationalFileModel formCollection)
        {
            if (!ModelState.IsValid)
            {
                return View(formCollection);
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == formCollection.UserId);

            var educationalFile = new EducationalFile();
            educationalFile.UserId = user.Id;
            educationalFile.Grade = formCollection.Grade;
            educationalFile.LastAvg = formCollection.LastAvg;
            educationalFile.RegistrationFormFileName = formCollection.RegistrationFormFileName;
            educationalFile.LastWorkbookFileName = formCollection.LastWorkbookFileName;

            if (formCollection.RegistrationFormFile != null)
            {
                string uploadedFileName = await UploadFile(formCollection.RegistrationFormFile, null);
                educationalFile.RegistrationFormFileName = uploadedFileName;
            }

            if (formCollection.LastWorkbookFile != null)
            {
                string uploadedFileName = await UploadFile(
                    formCollection.LastWorkbookFile,
                    formCollection.LastWorkbookFileName);
                educationalFile.LastWorkbookFileName = uploadedFileName;

                if (uploadedFileName != formCollection.LastWorkbookFileName)
                {
                    DeleteFile(formCollection.LastWorkbookFileName);
                }
            }

            try
            {
                _context.Attach(educationalFile).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                if (!TempData.ContainsKey("StatusMessage"))
                {
                    TempData.Add("StatusMessage", "پرونده تحصیلی شما با موفقیت به روز رسانی شد.");
                }
            }
            catch (Exception exception)
            {
                TempData.Add("StatusMessage", string.Format("Error: {0}", exception.Message));
            }
            return RedirectToAction();
        }

        public async Task<IActionResult> Curriculum(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var curriculum = await _context.Curricula
                .Include(c => c.Courses)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (curriculum == null)
            {
                TempData["StatusMessage"] = "Error: There is no curriculum for you.";
            }
            return View(curriculum);
        }

        [HttpPost]
        public async Task<IActionResult> Course(Course formCollection)
        {
            try
            {
                _context.Courses.Update(formCollection);
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = string.Format("{0} با موفقیت به روزرسانی شد", formCollection.Title);
            }
            catch (Exception exception)
            {
                TempData.Add("StatusMessage", string.Format("Error: {0}", exception.Message));
            }
            return RedirectToAction("Curriculum", new { id = formCollection.CurriculumId });
        }

        public async Task<IActionResult> Message()
        {
            List<Message> messages = new List<Message>();

            var user = await _userManager.GetUserAsync(User);
            TempData["User"] = user.Id;
            var adviser = _context.Advisers
                .FirstOrDefault(a => a.StudentId == user.Id);
            if (adviser != null)
            {
                messages = await _context.Messages
                    .Where(m => m.SenderId == user.Id || m.ReceiverId == user.Id)
                    .OrderBy(m => m.Created)
                    .Skip(Math.Max(0, _context.Messages.Count() - 10))
                    .ToListAsync();

                TempData["Adviser"] = adviser.AdviserId;
            }
            else
            {
                TempData["Adviser"] = string.Empty;
                TempData["StatusMessage"] = "Error: مشاوری برای شما تعیین نشده است. جهت تعیین مشاور با دفتر موسسه آوای فضیلت تماس حاصل نمایید.";
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

        public async Task<IActionResult> FinancialFile()
        {
            var user = await _userManager.GetUserAsync(User);

            var financialRecords = await _context.FinancialRecords
                .Where(e => e.UserId == user.Id)
                .ToListAsync();

            var financialFile = new FinancialFileModel()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                FinancialRecords = financialRecords,
            };

            return View(financialFile);
        }

        [HttpPost]
        public async Task<IActionResult> FinancialFile(FinancialRecord record)
        {
            var receiptFile = record.PaymentReceiptFile;

            string randomString = Guid.NewGuid().ToString().Replace("-", "");
            string fileExtention = Path.GetExtension(receiptFile.FileName);
            string fileName = String.Join("", randomString, fileExtention);
            string filePath = Path.Combine(path, fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await receiptFile.CopyToAsync(stream);
            }

            record.PaymentReceipt = fileName;
            await _context.AddAsync(record);
            await _context.SaveChangesAsync();
            TempData["StatusMessage"] = "رسید پرداخت با موفقیت ثبت شد. این پرداخت پس از تایید واحد مالی اعمال می گردد.";
            return RedirectToAction();
        }

        [HttpPost]
        public async Task<IActionResult> FinancialReceipt(string id)
        {
            var financialRecord = await _context.FinancialRecords
                .FirstOrDefaultAsync(f => f.Id == id);

            if (financialRecord == null)
            {
                TempData["StatusMessage"] = "Error: تراکنشی با مشخصات مورد نظر وجود ندارد.";
                return RedirectToAction("FinancialFile", new { id = id });
            }

            return View(financialRecord);
        }

        private readonly string path = Path.GetFullPath("wwwroot/images");

        private void DeleteFile(string fileName)
        {
            if(fileName == null)
            {
                return;
            }

            string filePath = Path.Combine(path, fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            return;
        }

        private async Task<string> UploadFile(IFormFile file, string currentFileName)
        {
            if(file.Length > 524288)
            {
                TempData["StatusMessage"] = "Error: File is too large.";
                return currentFileName;
            }
            string randomString = Guid.NewGuid().ToString().Replace("-", "");
            string fileExtention = Path.GetExtension(file.FileName);
            string fileName = String.Join("", randomString, fileExtention);
            string filePath = Path.Combine(path, fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
    }
}
