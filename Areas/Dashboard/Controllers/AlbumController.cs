using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fazilat.Data;
using Fazilat.Models;
using Fazilat.Areas.Dashboard.Models;

namespace Fazilat.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class AlbumController : Controller
    {
        private readonly FazilatContext _context;

        public AlbumController(FazilatContext context)
        {
            _context = context;
        }

        [Route("Dashboard/Album/")]
        public async Task<IActionResult> Index()
        {
            AlbumViewMode album = new AlbumViewMode();
            album.Categories = await _context.Categories
                .ToListAsync();
            album.Category = await _context.Categories
                .Include(c => c.Media)
                .FirstOrDefaultAsync(c => c.NormalizedName == "Uncategorized".ToUpper());

            return View(album);
        }

        [Route("Dashboard/Album/{name?}/")]
        public async Task<IActionResult> Index(string name)
        {
            if (name == null)
            {
                return NotFound();
            }

            AlbumViewMode album = new AlbumViewMode();
            album.Categories = await _context.Categories
                .ToListAsync();
            album.Category = await _context.Categories
                .Include(c => c.Media)
                .FirstOrDefaultAsync(c => c.NormalizedName == name.ToUpper());

            if (album.Category == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Dashboard/Album/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NormalizedName,PersianName")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.Id = Guid.NewGuid();
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Dashboard/Album/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Dashboard/Album/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,NormalizedName,PersianName")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Dashboard/Album/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Dashboard/Album/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(Guid id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
