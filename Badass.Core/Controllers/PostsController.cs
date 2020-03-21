using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badass.Core.Data;
using Badass.Core.Models;
using Badass.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Badass.Core.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostsController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Posts.Include(p => p.CreatedBy).Include(p => p.PostType).Include(p => p.UpdatedBy);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        [HttpGet]
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

            var postModel = new PostCommentViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                Comments = _context.Comments.Where(x => x.PostId == post.Id),
                CreatedBy = post.CreatedBy,
                CreatedDate = post.CreatedDate,
                PostType=post.PostType,
                PhotoPath=post.PhotoPath
            };
            if (post == null)
            {
                return NotFound();
            }

            return View(postModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(PostCommentViewModel postModel)
        {
            if (ModelState.IsValid)
            {
                
                var user = await _userManager.GetUserAsync(User);
                var comment = new Comment();
                comment.Body = postModel.CommentBody;
                comment.PostId = postModel.Id;
                comment.CreatedDate = DateTime.Now;
                comment.CreatedByUserId = user.Id;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details));
            }
            return View(postModel);
        }
    }
}