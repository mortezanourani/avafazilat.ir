﻿using Microsoft.AspNetCore.Mvc;

namespace Fazilat.Areas.Account.Controllers
{
    [Area("Account")]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
