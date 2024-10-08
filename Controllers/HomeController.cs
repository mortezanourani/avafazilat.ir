﻿using Fazilat.Models;
using Fazilat.Models.ViewModels;
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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Fazilat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FazilatContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            FazilatContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var slider = await _context.Slides1
                .Include(s => s.Image)
                .Include(s => s.Image.Category)
                .Where(s => s.IsVisible == true)
                .ToListAsync();

            var banners = await _context.Banners
                .Include(b => b.Image)
                .Include(b => b.Image.Category)
                .Where(b => b.IsActive)
                .OrderByDescending(b => b.Position)
                .ToListAsync();

            var blog = await _context.Posts
                .Include(p => p.Header)
                .Include(p => p.Header.Category)
                .Include(p => p.Author)
                .Where(p => p.IsVisible == true)
                .OrderByDescending(p => p.Published)
                .Take(3)
                .ToListAsync();

            var model = new HomeViewModel();
            model.Slider = slider;
            model.Banners = banners;
            model.Blog = blog;

            return View(model);
        }

        [Route("Conference/")]
        public async Task<IActionResult> Conference()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Conference/")]
        public async Task<IActionResult> Conference(Landing model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Error");
                return View(model);
            }

            var phone = await _context.Landing
                .FirstOrDefaultAsync(l => l.Phone == model.Phone);
            if(phone != null)
            {
                ModelState.AddModelError(string.Empty, "این شماره تماس قبلا ثبت شده است.");
                return View(model);
            }

            try
            {
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(model);
            }

            return RedirectToAction("ConferenceDone", "Home");
        }

        [Route("ConferenceDone/")]
        public IActionResult ConferenceDone()
        {
            return View();
        }

        [Route("Blog/{offset?}")]
        public async Task<IActionResult> Blog(int offset)
        {
            int limit = 5;

            var postsCount = _context.Posts
                .Where(p => p.IsVisible)
                .Count();
            int pageCount = (int)Math.Ceiling((double)postsCount / limit);
            int offsetMax = pageCount - 1;

            Post lastPost = await _context.Posts
                .Include(p => p.Header)
                .Include(p => p.Header.Category)
                .Include(p => p.Author)
                .Where(p => p.IsVisible)
                .OrderByDescending(p => p.Published)
                .FirstOrDefaultAsync();

            ICollection<Post> postsList = await _context.Posts
                .Include(p => p.Header)
                .Include(p => p.Header.Category)
                .Include(p => p.Author)
                .Where(p => p.IsVisible)
                .OrderByDescending(p => p.Published)
                .Skip((offset * limit) + 1)
                .Take(5)
                .ToListAsync();

            ICollection<AspNetUser> authorsList = await _context.AspNetUsers
                .Include(u => u.Posts)
                .Where(u => u.Posts
                    .Any(p => p.IsVisible == true))
                .ToListAsync();

            BlogViewModels model = new BlogViewModels();
            model.LastPost = lastPost;
            model.PostsList = postsList;
            model.Offset = offset;
            model.hasPrevious = offset > 0;
            model.hasNext = offset < offsetMax;
            model.AuthorsList = authorsList;

            return View(model);
        }

        [Route("Blog/Post/{id?}")]
        public async Task<IActionResult> Post(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Post model = await _context.Posts
                .Include(p => p.Header)
                .Include(p => p.Header.Category)
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == id);

            if(model == null || !model.IsVisible)
            {
                return NotFound();
            }

            return View(model);
        }

        [Route("Search/{title?}")]
        public async Task<IActionResult> Search(string title)
        {
            if(title == null)
            {
                return NotFound();
            }

            var posts = await _context.BlogPosts
                .Where(m => m.Title.Contains(title))
                .OrderByDescending(m => m.Date)
                .ToListAsync();
            if(posts == null)
            {
                return NotFound();
            }

            return View(posts);
        }

        [Route("About/")]
        public IActionResult AboutUs()
        {
            return View();
        }


        [Route("Reserve/")]
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

            var instruction = await _context.TicketInstructions
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
        [Route("Reserve/")]
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
