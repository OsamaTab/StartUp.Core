﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badass.Core.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Badass.Core.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
            
        }
        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.CreatedBy).Include(p => p.PostType).Include(p => p.UpdatedBy);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}