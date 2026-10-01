using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Digital_Handbook_Portal.Controllers
{
    public class BragBooksController : Controller
    {
        private readonly Digital_Handbook_PortalContext _context;

        public BragBooksController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        // READ ALL
        public async Task<IActionResult> Index()
        {
            return View(await _context.BragBook.OrderByDescending(b => b.datePosted).ToListAsync());
        }

        // READ DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var bragBook = await _context.BragBook.FirstOrDefaultAsync(m => m.bragId == id);
            if (bragBook == null) return NotFound();

            return View(bragBook);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BragBook bragBook)
        {
            if (ModelState.IsValid)
            {
                bragBook.datePosted = DateTime.Now;
                _context.Add(bragBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bragBook);
        }

        // UPDATE (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var bragBook = await _context.BragBook.FindAsync(id);
            if (bragBook == null) return NotFound();

            return View(bragBook);
        }

        // UPDATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BragBook bragBook)
        {
            if (id != bragBook.bragId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bragBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.BragBook.Any(e => e.bragId == bragBook.bragId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bragBook);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var bragBook = await _context.BragBook.FirstOrDefaultAsync(m => m.bragId == id);
            if (bragBook == null) return NotFound();

            return View(bragBook);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bragBook = await _context.BragBook.FindAsync(id);
            if (bragBook != null)
            {
                _context.BragBook.Remove(bragBook);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}