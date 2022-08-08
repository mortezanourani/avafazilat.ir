using Fazilat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fazilat.Data;

namespace Fazilat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var slides = await _context.Slides
                .ToListAsync();
            if(slides.Count == 0)
            {
                return RedirectToAction("Reserve");
            }

            var news = await _context.Blog
                .Where(p => p.isVisible == true)
                .OrderByDescending(p => p.Date)
                .ToListAsync();

            var model = new HomeViewModel()
            {
                Slides = slides,
                News = news.SkipLast(Math.Max(0, news.Count() - 3)).ToList(),
            };
            return View(model);
        }

        public async Task<IActionResult> Blog(string id)
        {
            if(id == null)
            {
                var posts = await _context.Blog
                    .Where(p => p.isVisible == true)
                    .OrderByDescending(p => p.Date)
                    .ToListAsync();

                return View(posts);
            }

            var post = await _context.Blog
                .Where(p => p.isVisible == true)
                .Where(p => p.Id == id)
                .ToListAsync();

            return View(post);
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public async Task<IActionResult> Reserve()
        {
            var tickets = await _context.Tickets
                .Where(t => t.Taken == false)
                .Where(t => t.Reserved == false)
                .OrderBy(t => t.Day)
                .ThenBy(d => d.Hour)
                .ThenBy(h => h.Minute)
                .ToListAsync();

            var sortedTickets = tickets
                .OrderBy(t => Regex.Match(t.Day, @"\d+").Value)
                .ToList();

            var instruction = await _context.TicketInstruction
                .FirstOrDefaultAsync();

            var model = new ReserveViewModel()
            {
                Instruction = instruction,
                Tickets = sortedTickets,
            };

            if (sortedTickets.Count < 1 && instruction.IsActive)
            {
                TempData["StatusMessage"] = "هیچ نوبت خالی برای مشاوره وجود ندارد.";
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Reserve(Meeting model)
        {
            using (var memoryStream = new MemoryStream())
            {
                await model.PaymentFile.CopyToAsync(memoryStream);
                model.Payment = memoryStream.ToArray();
            }
            model.Id = Guid.NewGuid().ToString();
            await _context.AddAsync(model);

            var selectedTicket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == model.TicketId);
            selectedTicket.Reserved = true;
            _context.Attach(selectedTicket).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            TempData["StatusMessage"] = "نوبت درخواستی شما با موفقیت رزرو شد. پس از تایید فیش واریزی توسط واحد امور مالی، پیامک ثبت نوبت درخواستی برای شما ارسال می گردد.";
            return RedirectToAction();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
