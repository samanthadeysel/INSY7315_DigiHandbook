using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Digital_Handbook_Portal.Controllers
{
    public class CommunitiesController : Controller
    {
        private readonly Digital_Handbook_PortalContext _context;

        public CommunitiesController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        // READ ALL
        public async Task<IActionResult> Index()
        {
            return View(await _context.Community.OrderByDescending(c => c.eventDateTime).ToListAsync());
        }

        // READ DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var community = await _context.Community.FirstOrDefaultAsync(m => m.eventId == id);
            if (community == null) return NotFound();

            return View(community);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Community community)
        {
            if (ModelState.IsValid)
            {
                _context.Add(community);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(community);
        }

        // UPDATE (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var community = await _context.Community.FindAsync(id);
            if (community == null) return NotFound();

            return View(community);
        }

        // UPDATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Community community)
        {
            if (id != community.eventId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(community);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Community.Any(e => e.eventId == community.eventId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(community);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var community = await _context.Community.FirstOrDefaultAsync(m => m.eventId == id);
            if (community == null) return NotFound();

            return View(community);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var community = await _context.Community.FindAsync(id);
            if (community != null)
            {
                _context.Community.Remove(community);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}