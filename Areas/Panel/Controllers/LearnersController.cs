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
    public class LearnersController : Controller
    {
        private readonly FazilatContext _context;

        public LearnersController(FazilatContext context)
        {
            _context = context;
        }

        // GET: Panel/Learners
        public async Task<IActionResult> Index()
        {
            return View(await _context.Learners
                .Include(m => m.Workshops)
                .OrderBy(m => m.Registered)
                .ToListAsync());
        }

        // GET: Panel/Learners/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .Include(m => m.Workshops)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // GET: Panel/Learners/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // POST: Panel/Learners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var learner = await _context.Learners
                .Include(l => l.Workshops)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (learner != null)
            {
                learner.Workshops.Clear();
                await _context.SaveChangesAsync();
                _context.Learners.Remove(learner);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnerExists(Guid id)
        {
            return _context.Learners.Any(e => e.Id == id);
        }
    }
}
