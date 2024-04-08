using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fazilat.Models;
using Microsoft.AspNetCore.Authorization;

namespace Fazilat.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Administrator")]
    public class WorkshopsController : Controller
    {
        private readonly FazilatContext _context;

        public WorkshopsController(FazilatContext context)
        {
            _context = context;
        }

        // GET: Panel/Workshops
        public async Task<IActionResult> Index()
        {
            return View(await _context.Workshops.OrderBy(w => w.Grade).ToListAsync());
        }

        // GET: Panel/Workshops/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshop = await _context.Workshops
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workshop == null)
            {
                return NotFound();
            }

            return View(workshop);
        }

        // GET: Panel/Workshops/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Panel/Workshops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Grade,Title,Cost,Capacity,StartDate")] Workshop workshop)
        {
            if (ModelState.IsValid)
            {
                workshop.Id = Guid.NewGuid();
                _context.Add(workshop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workshop);
        }

        // GET: Panel/Workshops/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshop = await _context.Workshops.FindAsync(id);
            if (workshop == null)
            {
                return NotFound();
            }
            return View(workshop);
        }

        // POST: Panel/Workshops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Grade,Title,Cost,Capacity,StartDate")] Workshop workshop)
        {
            if (id != workshop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workshop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkshopExists(workshop.Id))
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
            return View(workshop);
        }

        // GET: Panel/Workshops/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workshop = await _context.Workshops
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workshop == null)
            {
                return NotFound();
            }

            return View(workshop);
        }

        // POST: Panel/Workshops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var workshop = await _context.Workshops.FindAsync(id);
            if (workshop != null)
            {
                _context.Workshops.Remove(workshop);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkshopExists(Guid id)
        {
            return _context.Workshops.Any(e => e.Id == id);
        }
    }
}
