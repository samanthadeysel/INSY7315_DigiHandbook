
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Digital_Handbook_Portal.Models;

public class PoliciesController : Controller
{
    private readonly Digital_Handbook_PortalContext _context;

    public PoliciesController(Digital_Handbook_PortalContext context)
    {
        _context = context;
    }

    // GET: POLICYS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Policy.ToListAsync());
    }

    // GET: POLICYS/Details/5
    public async Task<IActionResult> Details(int? policyid)
    {
        if (policyid == null)
        {
            return NotFound();
        }

        var policy = await _context.Policy
            .FirstOrDefaultAsync(m => m.policyId == policyid);
        if (policy == null)
        {
            return NotFound();
        }

        return View(policy);
    }

    // GET: POLICYS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: POLICYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("policyId,Title,contentSummary,fileUrl")] Policy policy)
    {
        if (ModelState.IsValid)
        {
            _context.Add(policy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(policy);
    }

    // GET: POLICYS/Edit/5
    public async Task<IActionResult> Edit(int? policyid)
    {
        if (policyid == null)
        {
            return NotFound();
        }

        var policy = await _context.Policy.FindAsync(policyid);
        if (policy == null)
        {
            return NotFound();
        }
        return View(policy);
    }

    // POST: POLICYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? policyid, [Bind("policyId,Title,contentSummary,fileUrl")] Policy policy)
    {
        if (policyid != policy.policyId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(policy);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PolicyExists(policy.policyId))
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
        return View(policy);
    }

    // GET: POLICYS/Delete/5
    public async Task<IActionResult> Delete(int? policyid)
    {
        if (policyid == null)
        {
            return NotFound();
        }

        var policy = await _context.Policy
            .FirstOrDefaultAsync(m => m.policyId == policyid);
        if (policy == null)
        {
            return NotFound();
        }

        return View(policy);
    }

    // POST: POLICYS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? policyid)
    {
        var policy = await _context.Policy.FindAsync(policyid);
        if (policy != null)
        {
            _context.Policy.Remove(policy);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PolicyExists(int? policyid)
    {
        return _context.Policy.Any(e => e.policyId == policyid);
    }
}
