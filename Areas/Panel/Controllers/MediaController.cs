using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fazilat.Models;
using Fazilat.Areas.Panel.Models;
using Fazilat.Data;

namespace Fazilat.Areas.Panel.Controllers
{
    [Area("Panel")]
    public class MediaController : Controller
    {
        private readonly FazilatContext _context;

        public MediaController(FazilatContext context)
        {
            _context = context;
        }

        //// GET: Panel/Media
        //public async Task<IActionResult> Index()
        //{
        //    var fazilatContext = _context.Medias.Include(m => m.Category);
        //    return View(await fazilatContext.ToListAsync());
        //}

        //// GET: Panel/Media/Details/5
        //public async Task<IActionResult> Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var media = await _context.Medias
        //        .Include(m => m.Category)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (media == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(media);
        //}

        // GET: Panel/Media/Create
        public IActionResult Add()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "PersianName");
            return View();
        }

        // POST: Panel/Media/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Id,File,FileName,Extension,CategoryId,Uploaded")] AddMediaViewModel media)
        {
            if (ModelState.IsValid)
            {
                media.Id = Guid.NewGuid();
                media.FileName = media.Id.ToString().Replace("-", string.Empty);
                media.Extension = Path.GetExtension(media.File.FileName);
                bool Uploaded = await UploadFile(media);
                _context.Add(media);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Categories", new { @id = media.CategoryId });
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "PersianName", media.CategoryId);
            return View(media);
        }

        // GET: Panel/Media/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var media = await _context.Medias.FindAsync(id);
            if (media == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "PersianName", media.CategoryId);
            return View(media);
        }

        // POST: Panel/Media/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FileName,Extension,CategoryId,Uploaded")] Media media)
        {
            if (id != media.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(media);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MediaExists(media.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Categories", new { @id = media.CategoryId });
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "PersianName", media.CategoryId);
            return View(media);
        }

        // GET: Panel/Media/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var media = await _context.Medias
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (media == null)
            {
                return NotFound();
            }

            return View(media);
        }

        // POST: Panel/Media/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var media = await _context.Medias.FindAsync(id);
            if (media != null)
            {
                if (DeleteFile(media))
                {
                    _context.Medias.Remove(media);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Categories", new { @id = media.CategoryId });
        }

        private bool MediaExists(Guid id)
        {
            return _context.Medias.Any(e => e.Id == id);
        }

        private async Task<bool> UploadFile(AddMediaViewModel media)
        {
            if (media == null)
            {
                return false;
            }
            if (media.File.Length > 524288)
            {
                return false;
            }
            string fileName = string.Join("", media.FileName, media.Extension);
            string dirPath = Path.Combine("images", media.CategoryId.ToString());
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string filePath = Path.Combine(dirPath, fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await media.File.CopyToAsync(stream);
            }
            return true;
        }

        private bool DeleteFile(Media media)
        {
            if (media == null)
            {
                return false;
            }
            string fileName = string.Join("", media.FileName, media.Extension);
            string dirPath = Path.Combine("images", media.CategoryId.ToString());
            string filePath = Path.Combine(dirPath, fileName);
            System.IO.File.Delete(filePath);
            return true;
        }
    }
}
