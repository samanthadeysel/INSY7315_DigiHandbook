
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Digital_Handbook_Portal.Models;

public class DoctorsController : Controller
{
    private readonly Digital_Handbook_PortalContext _context;

    public DoctorsController(Digital_Handbook_PortalContext context)
    {
        _context = context;
    }

    // GET: DOCTORS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Doctor.ToListAsync());
    }

    // GET: DOCTORS/Details/5
    public async Task<IActionResult> Details(int? doctorid)
    {
        if (doctorid == null)
        {
            return NotFound();
        }

        var doctor = await _context.Doctor
            .FirstOrDefaultAsync(m => m.doctorId == doctorid);
        if (doctor == null)
        {
            return NotFound();
        }

        return View(doctor);
    }

    // GET: DOCTORS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DOCTORS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("doctorId,doctorImg,fName,lName,email,phone,suiteNumber")] Doctor doctor)
    {
        if (ModelState.IsValid)
        {
            _context.Add(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(doctor);
    }

    // GET: DOCTORS/Edit/5
    public async Task<IActionResult> Edit(int? doctorid)
    {
        if (doctorid == null)
        {
            return NotFound();
        }

        var doctor = await _context.Doctor.FindAsync(doctorid);
        if (doctor == null)
        {
            return NotFound();
        }
        return View(doctor);
    }

    // POST: DOCTORS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? doctorid, [Bind("doctorId,doctorImg,fName,lName,email,phone,suiteNumber")] Doctor doctor)
    {
        if (doctorid != doctor.doctorId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(doctor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(doctor.doctorId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(doctor);
    }

    // GET: DOCTORS/Delete/5
    public async Task<IActionResult> Delete(int? doctorid)
    {
        if (doctorid == null)
        {
            return NotFound();
        }

        var doctor = await _context.Doctor
            .FirstOrDefaultAsync(m => m.doctorId == doctorid);
        if (doctor == null)
        {
            return NotFound();
        }

        return View(doctor);
    }

    // POST: DOCTORS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? doctorid)
    {
        var doctor = await _context.Doctor.FindAsync(doctorid);
        if (doctor != null)
        {
            _context.Doctor.Remove(doctor);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DoctorExists(int? doctorid)
    {
        return _context.Doctor.Any(e => e.doctorId == doctorid);
    }
}
