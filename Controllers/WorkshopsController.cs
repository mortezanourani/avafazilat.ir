using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fazilat.Data;

namespace Fazilat.Controllers
{
    public class WorkshopsController : Controller
    {
        private readonly FazilatContext _context;

        public WorkshopsController(FazilatContext context)
        {
            _context = context;
        }

        // GET: Workshops
        public async Task<IActionResult> Index()
        {
            return View(await _context.Workshops.ToListAsync());
        }
    }
}
