using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Digital_Handbook_Portal.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly Digital_Handbook_PortalContext _context;

        public ResourcesController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        // READ ALL
        public async Task<IActionResult> Index()
        {
            return View(await _context.Resource.ToListAsync());
        }

        // READ DETAILS
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var resource = await _context.Resource.FirstOrDefaultAsync(m => m.Title == id);
            if (resource == null) return NotFound();

            return View(resource);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Resource resource)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resource);
        }

        // UPDATE (GET)
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var resource = await _context.Resource.FindAsync(id);
            if (resource == null) return NotFound();

            return View(resource);
        }

        // UPDATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Resource resource)
        {
            if (id != resource.Title) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Resource.Any(e => e.Title == resource.Title)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(resource);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var resource = await _context.Resource.FirstOrDefaultAsync(m => m.Title == id);
            if (resource == null) return NotFound();

            return View(resource);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var resource = await _context.Resource.FindAsync(id);
            if (resource != null)
            {
                _context.Resource.Remove(resource);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}