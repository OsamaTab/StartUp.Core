using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Badass.Core.Models;
using Badass.Core.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Badass.Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager , IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostingEnvironment = hostingEnvironment;
        }
   
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var model = new ProfileViewModel
            {
                Id = user.Id,
                UserName = user.Email,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PhotoPath = user.PhotoPath

            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {

            if (ModelState.IsValid)
            {
                string uniceName = null;
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (model.Photo != null)
                {
                    if (model.PhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images/profile/", model.PhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    uniceName = FileName(model);
                }

                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.PhotoPath = uniceName;
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("profile", "account");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        private string FileName(ProfileViewModel user)
        {
            string uniqueFileName = null;
            if (user.Photo != null)
            {
                string upladeFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/profile/");
                uniqueFileName = Guid.NewGuid().ToString() + '_' + user.Photo.FileName;
                string filePath = Path.Combine(upladeFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    user.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var user = new ApplicationUser { UserName = userModel.Email, Email = userModel.Email };

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