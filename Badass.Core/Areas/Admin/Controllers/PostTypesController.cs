﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Badass.Core.Data;
using Badass.Core.Models;

namespace Badass.Core.Areas.Admin
{
    
    public class PostTypesController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public PostTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/PostTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PostTypes.ToListAsync());
        }

        // GET: Admin/PostTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postType = await _context.PostTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postType == null)
            {
                return NotFound();
            }

            return View(postType);
        }

        // GET: Admin/PostTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/PostTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] PostType postType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(postType);
        }

        // GET: Admin/PostTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postType = await _context.PostTypes.FindAsync(id);
            if (postType == null)
            {
                return NotFound();
            }
            return View(postType);
        }

        // POST: Admin/PostTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] PostType postType)
        {
            if (id != postType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostTypeExists(postType.Id))
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
            return View(postType);
        }

        // GET: Admin/PostTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postType = await _context.PostTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postType == null)
            {
                return NotFound();
            }

            return View(postType);
        }

        // POST: Admin/PostTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var postType = await _context.PostTypes.FindAsync(id);
            _context.PostTypes.Remove(postType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostTypeExists(int id)
        {
            return _context.PostTypes.Any(e => e.Id == id);
        }
    }
}
