using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fazilat.Data;
using Fazilat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Fazilat.Areas.Dashboard.Controllers;

[Area("Dashboard")]
[Authorize(Roles = "Administrator")]
public class BlogController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly FazilatContext _context;

    public BlogController(
        UserManager<ApplicationUser> userManager,
        FazilatContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var fazilatContext = _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Header)
            .Include(p => p.Header.Category)
            .OrderByDescending(p => p.Published);
        return View(await fazilatContext.ToListAsync());
    }

    // GET: Dashboard/Blog/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Header)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    public async Task<IActionResult> Add()
    {
        ViewData["HeaderId"] = await _context.Medias
            .Include(m => m.Category)
            .Where(m => m.Category.NormalizedName == "blog".ToUpper())
            .ToListAsync();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Post post)
    {
        if (!ModelState.IsValid)
        {
            ViewData["HeaderId"] = await _context.Medias
                .Include(m => m.Category)
                .Where(m => m.Category.NormalizedName == "blog".ToUpper())
                .ToListAsync();
     
            return View(post);
        }

        //post.Id = Guid.NewGuid();
        post.AuthorId = _userManager.GetUserId(User);
        _context.Add(post);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Dashboard/Blog/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        ViewData["HeaderId"] = await _context.Medias
            .Include(m => m.Category)
            .Where(m => m.Category.NormalizedName == "blog".ToUpper())
            .ToListAsync();

        return View(post);
    }

    // POST: Dashboard/Blog/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,AuthorId,HeaderId,Title,Body,IsVisible,Published")] Post post)
    {
        if (id != post.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(post);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.Id))
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
        ViewData["AuthorId"] = new SelectList(_context.AspNetUsers, "Id", "Id", post.AuthorId);
        ViewData["HeaderId"] = new SelectList(_context.Medias, "Id", "Extension", post.HeaderId);
        return View(post);
    }

    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Header)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PostExists(Guid id)
    {
        return _context.Posts.Any(e => e.Id == id);
    }
}
