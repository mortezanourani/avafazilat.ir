using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Globalization;
using System.Linq;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Fazilat.Data;
using Fazilat.Models;
using Fazilat.Areas.Account.Models;

namespace Fazilat.Areas.Account.Controllers
{
    [Area("Account")]
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdministratorController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var users = await _context.Users
                .Include(u => u.Information)
                .Where(u => u.Id != userId)
                .OrderBy(u => u.Information.LastName)
                .ToListAsync();

            if (users == null)
            {
                TempData["StatusMessage"] = "Error: هیچ کاربری در سامانه ثبت نام نکرده است.";
                return View();
            }

            var adviserRoleId = _roleManager.Roles
                .Where(r => r.NormalizedName == "ADVISER")
                .Select(r => r.Id)
                .FirstOrDefault();
            var advisers = await _context.UserRoles
                .Where(ur => ur.RoleId == adviserRoleId)
                .Select(ur => ur.UserId)
                .ToListAsync();
            TempData["Advisers"] = advisers;

            return View(users);
        }

        [HttpPost]
        public ActionResult Index(string Search)
        {
            var result = _context.Users
                .Include(u => u.Information)
                .AsEnumerable()
                .Where(u => u.UserName == Search
                    || u.PhoneNumber == Search
                    || u.Information.FirstName == Search
                    || u.Information.LastName == Search
                    || u.Information.FullName == Search)
                .ToList();

            if(result.Count == 0)
            {
                TempData["StatusMessage"] = "Error: نتیجه ای یافت نشد.";
                return RedirectToAction();
            }
            return View(result);
        }

        public async Task<IActionResult> RoleManager(string id)
        {
            var user = await _context.Users
                .Include(u => u.Information)
                .FirstOrDefaultAsync(u => u.Id == id);
            if(user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var roles = await _roleManager.Roles
                .OrderBy(r => r.Name)
                .Select(r => r.Name)
                .ToListAsync();

            var roleManagerModel = new RoleManagerModel()
            {
                User = user,
                Role = userRoles.OrderBy(r => r).FirstOrDefault(),
                Roles = roles
            };

            return View(roleManagerModel);
        }

        [HttpPost]
        public async Task<IActionResult> RoleManager(RoleManagerModel formCollection)
        {
            var user = await _context.Users
                .Include(u => u.Information)
                .FirstOrDefaultAsync(u => u.Id == formCollection.User.Id);
            if (user == null)
            {
                TempData["StatusMessage"] = "کاربری با این مشخصات وجود ندارد.";
                return RedirectToAction("Index");
            }

            try
            {
                await _userManager.AddToRoleAsync(user, formCollection.Role);
                TempData["StatusMessage"] = string.Format("{0} با موفقیت به سطح دسترسی {1} اضافه شد.",
                    user.Information.FullName, formCollection.Role);
            }
            catch (Exception exception)
            {
                TempData["StatusMessage"] = string.Format("Error: {0}",
                    exception.Message);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AssignAdviser(string id)
        {
            var student = await _context.Users
                .Include(u => u.Information)
                .FirstOrDefaultAsync(u => u.Id == id);

            var studentAdviser = await _context.Advisers
                .FirstOrDefaultAsync(a => a.StudentId == id);
            if(studentAdviser == null)
            {
                studentAdviser = new Adviser()
                {
                    Id = Guid.NewGuid().ToString(),
                    StudentId = id,
                    AdviserId = null,
                };
            }

            var adviser = await _context.Users
                .Include(a => a.Information)
                .FirstOrDefaultAsync(a => a.Id == studentAdviser.AdviserId);

            var adviserRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == "Adviser");
            var adviserRoleId = adviserRole.Id;
            var advisersList = await _context.UserRoles
                .Where(ur => ur.RoleId == adviserRoleId)
                .Select(ur => ur.UserId)
                .ToListAsync();
            var advisers = await _context.Users
                .Include(u => u.Information)
                .Where(u => advisersList.Contains(u.Id))
                .OrderBy(u => u.Information.LastName)
                .ToListAsync();

            var adviserModel = new AdviserAssignmentModel()
            {
                Id = studentAdviser.Id,
                Student = student,
                StudentId = student.Id,
                Adviser = adviser,
                AdviserId = (adviser == null) ? null: adviser.Id,
                Advisers = advisers
            };

            return View(adviserModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssignAdviser(AdviserAssignmentModel formCollection)
        {
            Adviser adviser = await _context.Advisers
                .FirstOrDefaultAsync(a => a.Id == formCollection.Id);
            if(adviser == null)
            {
                adviser = new Adviser()
                {
                    Id = formCollection.Id,
                    StudentId = formCollection.StudentId,
                    AdviserId = formCollection.AdviserId
                };
                await _context.AddAsync(adviser);
            }
            else
            {
                adviser.AdviserId = formCollection.AdviserId;
                _context.Attach(adviser).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();

            var student = await _context.Users
                .Include(u => u.Information)
                .FirstOrDefaultAsync(u => u.Id == formCollection.StudentId);

            var studentAdviser = await _context.Users
                .Include(u => u.Information)
                .FirstOrDefaultAsync(u => u.Id == formCollection.AdviserId);

            TempData["StatusMessage"] = string.Format("«{0}» به عنوان مشاور تحصیلی «{1}» تعیین شد.",
                studentAdviser.Information.FullName,
                student.Information.FullName);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> FinancialFiles()
        {
            var users = await _context.Users
                .Include(u => u.Information)
                .Include(u => u.Limitation)
                .OrderBy(u => u.Limitation.Expiration)
                .ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> FinancialFile(string id)
        {
            if(id == null)
            {
                return RedirectToAction("FinancialFiles");
            }

            var user = await _context.Users
                .Include(u => u.FinancialFile)
                .Include(u => u.Limitation)
                .FirstOrDefaultAsync(u => u.Id == id);

            var expiration = (user.Limitation != null)
                ? user.Limitation.Expiration
                : DateTime.Now.AddMonths(1);

            PersianCalendar persianCalendar = new PersianCalendar();
            var year = persianCalendar.GetYear(expiration);
            var month = persianCalendar.GetMonth(expiration);
            year = month == 1 ? --year : year;
            month = month == 1 ? 12 : --month;
            user.Limitation = new UserLimitation()
            {
                UserId = user.Id,
                ExpirationYear = year,
                ExpirationMonth = month,
            };

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> FinancialFile(FinancialRecord record)
        {
            var financialRecord = await _context.FinancialRecords
                .FirstOrDefaultAsync(f => f.Id == record.Id);

            if (financialRecord != null)
            {
                financialRecord.IsApproved = !financialRecord.IsApproved;
                _context.Attach(financialRecord).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
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

        [HttpPost]
        public async Task<IActionResult> Limitation(UserLimitation formCollection)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            formCollection.ExpirationMonth++;
            if(formCollection.ExpirationMonth > 12)
            {
                formCollection.ExpirationMonth = 1;
                formCollection.ExpirationYear++;
            }
            var expiration = new UserLimitation()
            {
                UserId = formCollection.UserId,
                Expiration = persianCalendar.ToDateTime(
                    formCollection.ExpirationYear,
                    formCollection.ExpirationMonth,
                    1, 0, 0, 0, 0),
            };

            var isExits = _context.UsersLimitation
                .Count(l => l.UserId == formCollection.UserId);
            if(isExits > 0)
            {
                _context.Attach(expiration).State = EntityState.Modified;
            }
            else
            {
                await _context.AddAsync(expiration);
            }
            await _context.SaveChangesAsync();
            TempData["StatusMessage"] = "اعتبار حساب کاربری مورد نظر با موفقیت تغییر یافت.";

            return RedirectToAction("FinancialFile", new { id = expiration.UserId });
        }

        public async Task<IActionResult> TicketInstruction()
        {
            var instruction = await _context.TicketInstruction
                .FirstOrDefaultAsync();

            return View(instruction);
        }

        [HttpPost]
        public async Task<IActionResult> TicketInstruction(TicketInstruction instruction)
        {
            if (instruction != null)
            {
                _context.Attach(instruction).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "دستور العمل ثبت درخواست مشاوره انتخاب رشته با موفیت به روزرسانی شد.";
            }

            return RedirectToAction();
        }

        public async Task<IActionResult> Ticket(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var ticket = await _context.Tickets
                    .FirstOrDefaultAsync(t => t.Id == id);
                _context.Remove(ticket);
                await _context.SaveChangesAsync();

                TempData["StatusMessage"] = "نوبت مورد نظر باموفقیت حذف شد.";
                return RedirectToAction("Ticket", "Administrator", new { id = "" });
            }

            var tickets = await _context.Tickets
                .Include(t => t.Meeting)
                .OrderBy(t => t.Day)
                .ThenBy(d => d.Hour)
                .ThenBy(h => h.Minute)
                .ToListAsync();

            var sortedTickets = tickets
                .OrderBy(t => Regex.Match(t.Day, @"\d+").Value)
                .ToList();

            var model = new TicketModel()
            {
                Hour = DateTime.Now.Hour,
                Minute = DateTime.Now.Minute,
                Tickets = sortedTickets,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Ticket(TicketModel ticketModel)
        {
            ticketModel.Id = Guid.NewGuid().ToString();
            ticketModel.Reserved = false;
            ticketModel.Taken = false;

            await _context.AddAsync(ticketModel);
            await _context.SaveChangesAsync();
            return RedirectToAction();
        }

        public async Task<IActionResult> Meeting(string id)
        {
            var meeting = await _context.Meetings
                .Include(m => m.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);

            return View(meeting);
        }

        public async Task<IActionResult> Reject(string id)
        {
            var meeting = await _context.Meetings
                .Include(m => m.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            _context.Remove(meeting);

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == meeting.TicketId);
            ticket.Reserved = false;
            ticket.Taken = false;
            _context.Attach(ticket).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToAction("Ticket");
        }

        public async Task<IActionResult> Confirm(string id)
        {
            var meeting = await _context.Meetings
                .Include(m => m.Ticket)
                .FirstOrDefaultAsync(m => m.Id == id);
            meeting.Confirmed = true;
            _context.Attach(meeting).State = EntityState.Modified;

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == meeting.TicketId);
            ticket.Reserved = true;
            ticket.Taken = true;
            _context.Attach(ticket).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToAction("Ticket");
        }

        public async Task<IActionResult> Slide(string id)
        {
            if(id != null)
            {
                var slide = await _context.Slides
                    .FirstOrDefaultAsync(s => s.Id == id);
                string path = Path.GetFullPath("wwwroot/images/slide");
                string filePath = Path.Combine(path, slide.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                _context.Remove(slide);
                await _context.SaveChangesAsync();
                return RedirectToAction("Slide", new { id = "" });
            }

            var slides = await _context.Slides
                .OrderBy(s => s.Link)
                .ToListAsync();
            var model = new SlideModel()
            {
                Slides = slides
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Slide(SlideModel slideForm)
        {
            var id = Guid.NewGuid().ToString();

            var path = Path.GetFullPath("wwwroot/images/slide");
            var extention = Path.GetExtension(slideForm.ImageFile.FileName);
            var fileName = String.Join("", id.Replace("-", ""), extention);
            var imagePath = Path.Combine(path, fileName);
            using (var stream = System.IO.File.Create(imagePath))
            {
                await slideForm.ImageFile.CopyToAsync(stream);
            }
            var slide = new Slide()
            {
                Id = id,
                Image = fileName,
                Link = slideForm.Link
            };
            await _context.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction();
        }

        public async Task<IActionResult> Blog()
        {
            var posts = await _context.Blog
                .OrderByDescending(p => p.Date)
                .ToListAsync();

            return View(posts);
        }

        [HttpPost]
        public async Task<IActionResult> Blog(string id)
        {
            var post = await _context.Blog
                .FirstOrDefaultAsync(p => p.Id == id);
            if(post != null)
            {
                post.isVisible = !(post.isVisible);
                _context.Attach(post).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction();
        }

        public async Task<IActionResult> Post(string id)
        {
            var post = await _context.Blog
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                post = new BlogPost()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = _userManager.GetUserId(User),
                };
            }

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Post(BlogPost post)
        {
            if(post == null)
            {
                return RedirectToAction();
            }

            var path = Path.GetFullPath("wwwroot/images/blog");
            if (post.ImageFile != null)
            {
                var file = post.ImageFile;
                string randomString = Guid.NewGuid().ToString().Replace("-", "");
                string fileExtention = Path.GetExtension(file.FileName);
                string fileName = String.Join("", randomString, fileExtention);
                string filePath = Path.Combine(path, fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                    post.Image = fileName;
                }
            }

            var oldPost = await _context.Blog
                .FirstOrDefaultAsync(p => p.Id == post.Id);

            if (oldPost != null)
            {
                if(oldPost.Image != null)
                {
                    var filePath = Path.Combine(path, oldPost.Image);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                oldPost.Id = post.Id;
                oldPost.UserId = post.UserId;
                oldPost.Title = post.Title;
                oldPost.Content = post.Content;
                oldPost.Date = post.Date;
                oldPost.Image = post.Image;
                _context.Attach(oldPost).State = EntityState.Modified;
            }
            else
            {
                post.Date = DateTime.Now;
                await _context.AddAsync(post);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Post", new { id = post.Id });
        }
    }
}
