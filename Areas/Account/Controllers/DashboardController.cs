using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Fazilat.Areas.Account.Models;
using Fazilat.Models;
using Fazilat.Data;

namespace Fazilat.Areas.Account.Controllers
{
    [Area("Account")]
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

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public async Task<IActionResult> EducationalFile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }

            var user = await _userManager.GetUserAsync(User);

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

            var user = await _userManager.GetUserAsync(User);

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
                    TempData.Add("StatusMessage", "Information updated successfully.");
                }
            }
            catch (Exception exception)
            {
                TempData.Add("StatusMessage", string.Format("Error: {0}", exception.Message));
            }
            return RedirectToAction();
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
