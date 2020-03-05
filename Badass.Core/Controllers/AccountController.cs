﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badass.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Badass.Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        
        public IActionResult Register()
        {
            return View("Areas/Identity/Pages/Account/Register.cshtml");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var user = new IdentityUser { UserName = userModel.Email, Email = userModel.Email };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");
                await _signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return View(userModel);
        }
    }
}