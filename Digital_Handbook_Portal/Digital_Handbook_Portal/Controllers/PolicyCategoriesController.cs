using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Digital_Handbook_Portal.Controllers
{
    public class PolicyCategoriesController : Controller
    {
        private readonly Digital_Handbook_PortalContext _context;

        public PolicyCategoriesController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        // READ ALL
        public async Task<IActionResult> Index()
        {
            var categories = await _context.PolicyCategory.Include(c => c.Policies).ToListAsync();
            return View(categories);
        }

        // READ DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.PolicyCategory
                .Include(c => c.Policies)
                .FirstOrDefaultAsync(m => m.categoryId == id);

            if (category == null) return NotFound();

            return View(category);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PolicyCategory policyCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(policyCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(policyCategory);
        }

        // UPDATE (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.PolicyCategory.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // UPDATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PolicyCategory policyCategory)
        {
            if (id != policyCategory.categoryId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(policyCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.PolicyCategory.Any(e => e.categoryId == policyCategory.categoryId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(policyCategory);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.PolicyCategory
                .Include(c => c.Policies)
                .FirstOrDefaultAsync(m => m.categoryId == id);

            if (category == null) return NotFound();

            return View(category);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.PolicyCategory.FindAsync(id);
            if (category != null)
            {
                _context.PolicyCategory.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}