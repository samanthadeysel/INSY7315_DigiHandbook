using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Digital_Handbook_Portal.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly Digital_Handbook_PortalContext _context;

        public DoctorsController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        // READ ALL
        public async Task<IActionResult> Index()
        {
            return View(await _context.Doctor.ToListAsync());
        }

        // READ DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctor.FirstOrDefaultAsync(m => m.doctorId == id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // UPDATE (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctor.FindAsync(id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        // UPDATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doctor doctor)
        {
            if (id != doctor.doctorId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Doctor.Any(e => e.doctorId == doctor.doctorId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(doctor);
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctor.FirstOrDefaultAsync(m => m.doctorId == id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctor.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctor.Remove(doctor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}