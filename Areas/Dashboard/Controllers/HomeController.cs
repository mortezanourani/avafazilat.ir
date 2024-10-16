﻿using Fazilat.Data;
using Fazilat.Models;
using Fazilat.Areas.Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Security.Claims;

namespace Fazilat.Areas.Dashboard.Controllers;

[Area("Dashboard")]
[Authorize]
public class HomeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly FazilatContext _context;

    public HomeController(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        FazilatContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        HomeModel model = new HomeModel();
        model.Panel = await GetPanelRole();
        model.User = await _userManager.GetUserAsync(User);

        if (model.Panel.Level == 0)
        {
            model.Provinces = await _context.Provinces
                .OrderBy(p => p.Name)
                .ToListAsync();

            model.UserRoles = await _roleManager.Roles
                .OrderBy(r => r.Level)
                .ToListAsync();

            List<ApplicationUser> users = await _userManager.Users
                .ToListAsync();
            foreach (ApplicationUser user in users)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                user.Expired = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Expired)?.Value;
            }
            model.Users = users
                .OrderByDescending(u => u.Registered)
                .Take(10)
                .OrderBy(u => u.LastName)
                .ToList();
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult Index(string role)
    {
        HttpContext.Session.SetString(ApplicationKeys.PanleRoleKey, role);
        return RedirectToAction("Index", "Home", new { area = "Dashboard" });
    }

    private async Task<ApplicationRole> GetPanelRole()
    {
        string panelRole = HttpContext.Session.GetString(ApplicationKeys.PanleRoleKey);
        if (string.IsNullOrEmpty(panelRole))
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);

            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            ApplicationRole defaultPanel = await _roleManager.Roles
                .Where(r => userRoles.Contains(r.Name))
                .OrderBy(r => r.Level)
                .FirstOrDefaultAsync();

            HttpContext.Session.SetString(ApplicationKeys.PanleRoleKey, defaultPanel.Id);
            panelRole = defaultPanel.Id;
        }

        ApplicationRole userPanel = await _roleManager.Roles
            .FirstOrDefaultAsync(r => r.Id == panelRole);

        return userPanel;
    }
}
