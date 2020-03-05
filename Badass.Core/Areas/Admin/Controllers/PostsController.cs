using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Badass.Core.Data;
using Badass.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Badass.Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Posts
        public async Task<IActionResult> Index(int? filterPostId)
        {
            ViewData["PostTypeId"] = new SelectList(_context.PostTypes, "Id", "Title");

            
            if (filterPostId != null)
            {
                var postss = _context.Posts.Include(x => x.PostType).Include(x => x.CreatedBy).Include(x => x.UpdatedBy).Where(x => x.PostTypeId == filterPostId);
                return View(await postss.ToListAsync());
            }
            var posts = _context.Posts.Include(x => x.PostType).Include(x => x.CreatedBy).Include(x => x.UpdatedBy);
            return View(await posts.ToListAsync());
        }

        // GET: Admin/Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.CreatedBy)
                .Include(p => p.PostType)
                .Include(p => p.UpdatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Admin/Posts/Create
        public IActionResult Create()
        {
            ViewData["PostTypeId"] = new SelectList(_context.PostTypes, "Id", "Title");
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostTypeId,Title,Body,Status")] Post post)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                post.CreatedByUserId = user.Id;
                post.CreatedDate = DateTime.Now;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", post.CreatedByUserId);
            ViewData["PostTypeId"] = new SelectList(_context.PostTypes, "Id", "Id", post.PostTypeId);
            ViewData["UpdatedByUserId"] = new SelectList(_context.Users, "Id", "Id", post.UpdatedByUserId);
            return View(post);
        }

        // GET: Admin/Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }
            ViewData["PostTypeId"] = new SelectList(_context.PostTypes, "Id", "Title", post.PostTypeId);
  
            return View(post);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostTypeId,Title,Body,Status, CreatedDate, CreatedByUserId")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    post.UpdatedByUserId = user.Id;
                    post.UpdatedDate = DateTime.Now;
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", post.CreatedByUserId);
            ViewData["PostTypeId"] = new SelectList(_context.PostTypes, "Id", "Id", post.PostTypeId);
            ViewData["UpdatedByUserId"] = new SelectList(_context.Users, "Id", "Id", post.UpdatedByUserId);
            return View(post);
        }

        // GET: Admin/Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.CreatedBy)
                .Include(p => p.PostType)
                .Include(p => p.UpdatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
