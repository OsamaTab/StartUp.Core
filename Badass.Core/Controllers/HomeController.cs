﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Badass.Core.Models;
using Badass.Core.Data;
using Microsoft.EntityFrameworkCore;
using Badass.Core.ViewModels;

namespace Badass.Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var posts = _context.Posts.Include(p => p.CreatedBy).Include(p => p.PostType).Include(p => p.UpdatedBy);
            var viewModel = new HomeViewModel
            {
                ContactUs = new ContactUs(),
                Posts = await posts.Take(2).ToListAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Id,Name,Email,Massage")] ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                contactUs.SendTime = DateTime.Now;
                _context.Add(contactUs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactUs);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
