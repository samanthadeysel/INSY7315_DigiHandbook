using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Digital_Handbook_Portal.Controllers
{
    public class PoliciesController : Controller
    {
        private readonly Digital_Handbook_PortalContext _context;
        private readonly IWebHostEnvironment _environment;

        public PoliciesController(Digital_Handbook_PortalContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // READ ALL
        public async Task<IActionResult> Index()
        {
            var policies = await _context.Policy.Include(p => p.Category).ToListAsync();
            return View(policies);
        }

        // READ DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var policy = await _context.Policy
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.policyId == id);

            if (policy == null) return NotFound();

            return View(policy);
        }

        // CREATE (GET)
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.PolicyCategory.ToListAsync(), "categoryId", "categoryName");
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Policy policy, IFormFile? pdfFile)
        {
            if (pdfFile != null && pdfFile.Length > 0)
            {
                policy.fileUrl = await SaveUploadedFileAsync(pdfFile);
            }

            if (ModelState.IsValid)
            {
                _context.Add(policy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _context.PolicyCategory.ToListAsync(), "categoryId", "categoryName", policy.categoryId);
            return View(policy);
        }

        // UPDATE (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var policy = await _context.Policy.FindAsync(id);
            if (policy == null) return NotFound();

            ViewBag.Categories = new SelectList(await _context.PolicyCategory.ToListAsync(), "categoryId", "categoryName", policy.categoryId);
            return View(policy);
        }

        // UPDATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Policy policy, IFormFile? pdfFile)
        {
            if (id != policy.policyId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (pdfFile != null && pdfFile.Length > 0)
                    {
                        policy.fileUrl = await SaveUploadedFileAsync(pdfFile);
                    }

                    _context.Update(policy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Policy.Any(e => e.policyId == policy.policyId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _context.PolicyCategory.ToListAsync(), "categoryId", "categoryName", policy.categoryId);
            return View(policy);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var policy = await _context.Policy
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.policyId == id);

            if (policy == null) return NotFound();

            return View(policy);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policy = await _context.Policy.FindAsync(id);
            if (policy != null)
            {
                _context.Policy.Remove(policy);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveUploadedFileAsync(IFormFile pdfFile)
        {
            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = $"{Guid.NewGuid()}_{pdfFile.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await pdfFile.CopyToAsync(fileStream);
            }

            return $"/uploads/{uniqueFileName}";
        }
    }
}