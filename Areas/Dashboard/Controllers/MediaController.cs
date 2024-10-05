using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fazilat.Data;
using Fazilat.Models;
using Microsoft.IdentityModel.Tokens;
using Fazilat.Areas.Dashboard.Models;
using System.IO;
using System.Web;

namespace Fazilat.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class MediaController : Controller
    {
        private readonly FazilatContext _context;

        public MediaController(FazilatContext context)
        {
            _context = context;
        }

        // GET: Dashboard/Media/Details/5
        public async Task<IActionResult> Details(Guid? id)
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

        [Route("Dashboard/Media/Add/{category?}")]
        public async Task<IActionResult> Add(string category)
        {
            if (category.IsNullOrEmpty())
            {
                return NotFound();
            }

            Category album = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == category);
            if (album == null)
            {
                return NotFound();
            }

            AddMediaViewModel model = new AddMediaViewModel();
            model.CategoryId = album.Id;
            model.Category = album;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Dashboard/Media/Add/{category?}")]
        public async Task<IActionResult> Add(string category, [Bind("Id,File,CategoryId")] AddMediaViewModel media)
        {
            if (!ModelState.IsValid)
            {
                media.Category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == category);
                return View(media);
            }

            media.Id = Guid.NewGuid();
            media.FileName = media.Id.ToString().Replace("-", string.Empty);
            media.Extension = Path.GetExtension(media.File.FileName);
            bool Uploaded = await UploadFile(media);
            if (!Uploaded)
            {
                return View(media);
            }
            _context.Add(media);
            await _context.SaveChangesAsync();

            Category album = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == media.CategoryId);
            return RedirectToAction("Index", "Album", new { @name = album.Name });
        }

        // GET: Dashboard/Media/Delete/5
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

        // POST: Dashboard/Media/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var media = await _context.Medias.FindAsync(id);
            if (media != null)
            {
                _context.Medias.Remove(media);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == media.CategoryId);
            var categoryName = category.NormalizedName.ToLower();
            string dirPath = Path.Combine("wwwroot/images", categoryName);
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
    }
}
