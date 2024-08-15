using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fazilat.Models;
using Fazilat.Data;

namespace Fazilat.Controllers
{
    public class LearnersController : Controller
    {
        private readonly FazilatContext _context;

        public LearnersController(FazilatContext context)
        {
            _context = context;
        }

        // GET: Learners
        [Route("Learners/Done/{id?}")]
        public async Task<IActionResult> Index(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Learners.FirstOrDefaultAsync(m => m.Id == id);
            return View(model);
        }

        // GET: Learners/Create
        [Route("Workshop/Register/{Workshops?}")]
        public async Task<IActionResult> Create(Guid[] workshops)
        {
            Learner model = new Learner();
            model.Workshops = await _context.Workshops.Where(w => workshops.Contains(w.Id)).ToListAsync();
            return View(model);
        }

        // POST: Learners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Workshop/Register/{Workshops?}")]
        public async Task<IActionResult> Create(Guid[] workshops, [Bind("Id,Name,Phone,ParentPhone,City,District,School,TrackingCode,Registered")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                learner.Id = Guid.NewGuid();
                learner.Workshops = await _context.Workshops.Where(w => workshops.Contains(w.Id)).ToListAsync();
                _context.Add(learner);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Learners", new { @id = learner.Id });
            }
            return View(learner);
        }

        private bool LearnerExists(Guid id)
        {
            return _context.Learners.Any(e => e.Id == id);
        }
    }
}
