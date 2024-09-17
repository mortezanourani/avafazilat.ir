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

namespace Fazilat.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize(Roles = "Administrator")]
    public class ConferenceController : Controller
    {
        private readonly FazilatContext _context;

        public ConferenceController(FazilatContext context)
        {
            _context = context;
        }

        // GET: Panel/Conference
        public async Task<IActionResult> Index()
        {
            return View(await _context.Landing.OrderBy(l => l.Submitted).ToListAsync());
        }
    }
}
