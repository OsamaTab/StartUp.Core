using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badass.Core.Areas.Admin.ViewModels;
using Badass.Core.Data;
using Badass.Core.Models;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,
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
            var user = await _userManager.FindByIdAsync(id);
            var model = new UserViewModel
            {
                UserId = user.Id,
                UserName = user.Email,

            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string userId,UserViewModel users ,string roleId)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userId);
                var role = await _roleManager.FindByIdAsync(users.RoleId);
                
                user.Email = users.UserName;
                role.Name = users.RoleName;
                await _userManager.AddToRoleAsync(user, role.Name);
                 
                IdentityResult result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "roles");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string Id)
        {

            var user = await _userManager.FindByIdAsync(Id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction("index", "account");

        }
    }
}