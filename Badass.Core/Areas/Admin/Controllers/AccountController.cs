using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badass.Core.Areas.Admin.ViewModels;
using Badass.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Badass.Core.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var model = new List<UserViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userRole = new UserViewModel
                {
                    UserId = user.Id,
                    UserName = user.Email
                };
                foreach (var role in _roleManager.Roles)
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRole.RoleName = role.Name;
                        userRole.RoleId = role.Id;
                    }
                }
                model.Add(userRole);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");

            if (id == null)
            {
                return NotFound();
            }
            //var role = await _userManager.FindByIdAsync(id);
            var userViewModel = new UserViewModel();
            var user= await _userManager.FindByIdAsync(id);
            if (userViewModel.Equals(user))
            {
                userViewModel.UserName = user.UserName;
                userViewModel.UserId = user.Id;
                

            }

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string userId, [Bind("Email")] IdentityUser user, string roleId)
        {
            if (userId != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _userManager.AddToRoleAsync(user, roleId);
                    await _userManager.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string Id)
        {

            var user = await _userManager.FindByIdAsync(Id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction("index", "account");

        }

        private bool UserExists(string id)
        {
            return _userManager.Users.Any(e => e.Id == id);
        }
    }
}