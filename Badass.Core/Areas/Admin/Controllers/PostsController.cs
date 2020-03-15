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
using Badass.Core.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Badass.Core.Areas.Admin.Controllers
{
    public class PostsController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
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
        public async Task<IActionResult> Create([Bind("PostTypeId,Title,Body,Status,Photo")]PostFileViewModel postModel)
        {
            if (ModelState.IsValid)
            {
                var post = new Post();

                string uniqueFileName = FileName(postModel);

                var user = await _userManager.GetUserAsync(User);
                post.CreatedByUserId = user.Id;
                post.Title = postModel.Title;
                post.Body = postModel.Body;
                post.Status = postModel.Status;
                post.PostTypeId = postModel.PostTypeId;
                post.PhotoPath = uniqueFileName;
                post.CreatedDate = DateTime.Now;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(postModel);
        }

        private string FileName(PostFileViewModel postModel)
        {
            string uniqueFileName = null;
            if (postModel.Photo != null)
            {
                string upladeFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + '_' + postModel.Photo.FileName;
                string filePath = Path.Combine(upladeFolder, uniqueFileName);
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    postModel.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        // GET: Admin/Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var post = await _context.Posts.FindAsync(id);

            PostFileViewModel postView = new PostFileViewModel
            {
                Id = post.Id,
                Body=post.Body,
                CreatedBy=post.CreatedBy,
                PostTypeId=post.PostTypeId,
                Title=post.Title,
                Status=post.Status,
                PhotoPath=post.PhotoPath,
                CreatedByUserId=post.CreatedByUserId,
                CreatedDate=post.CreatedDate
            };
            

            if (post == null)
            {
                return NotFound();
            }
            ViewData["PostTypeId"] = new SelectList(_context.PostTypes, "Id", "Title", post.PostTypeId);
  
            return View(postView);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostTypeId,Title,Body,Status, CreatedDate, CreatedByUserId , Photo ,PhotoPath")] PostFileViewModel post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName=null;
                    if (post.Photo != null)
                    {
                        if (post.PhotoPath != null)
                        {
                            string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images" , post.PhotoPath);
                            System.IO.File.Delete(filePath); 
                        }
                        uniqueFileName = FileName(post);
                    }
                    var user = await _userManager.GetUserAsync(User);
                    post.UpdatedByUserId = user.Id;
                    post.UpdatedDate = DateTime.Now;
                    post.PhotoPath = uniqueFileName;
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
