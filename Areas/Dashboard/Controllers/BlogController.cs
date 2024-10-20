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

namespace Fazilat.Areas.Dashboard.Controllers;

[Area("Dashboard")]
[Authorize(Roles = "Administrator")]
public class BlogController : Controller
{
    private readonly FazilatContext _context;

    public BlogController(FazilatContext context)
    {
        _context = context;
    }

    // GET: Dashboard/Blog
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

    // GET: Dashboard/Blog/Create
    public IActionResult Add()
    {
        ViewData["HeaderId"] = new SelectList(_context.Medias, "Id", "FileName");
        return View();
    }

    // POST: Dashboard/Blog/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add([Bind("Id,HeaderId,Title,Body,IsVisible,Published")] Post post)
    {
        if (ModelState.IsValid)
        {
            post.Id = Guid.NewGuid();
            _context.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["AuthorId"] = new SelectList(_context.AspNetUsers, "Id", "Id", post.AuthorId);
        ViewData["HeaderId"] = new SelectList(_context.Medias, "Id", "Extension", post.HeaderId);
        return View(post);
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
        ViewData["AuthorId"] = new SelectList(_context.AspNetUsers, "Id", "Id", post.AuthorId);
        ViewData["HeaderId"] = new SelectList(_context.Medias, "Id", "Extension", post.HeaderId);
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

    // GET: Dashboard/Blog/Delete/5
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

    // POST: Dashboard/Blog/Delete/5
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
