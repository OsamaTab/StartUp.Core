using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Badass.Core.Data;
using Badass.Core.Models;

namespace Badass.Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactUsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactUsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ContactS
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactS.ToListAsync());
        }

        // GET: Admin/ContactS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ContactS = await _context.ContactS
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ContactS == null)
            {
                return NotFound();
            }

            return View(ContactS);
        }

        // GET: Admin/ContactS/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ContactS = await _context.ContactS
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ContactS == null)
            {
                return NotFound();
            }

            return View(ContactS);
        }

        // POST: Admin/ContactS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ContactS = await _context.ContactS.FindAsync(id);
            _context.ContactS.Remove(ContactS);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactSExists(int id)
        {
            return _context.ContactS.Any(e => e.Id == id);
        }
    }
}
