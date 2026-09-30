
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Digital_Handbook_Portal.Models;

public class PolicyCategoriesController : Controller
{
    private readonly Digital_Handbook_PortalContext _context;

    public PolicyCategoriesController(Digital_Handbook_PortalContext context)
    {
        _context = context;
    }

    // GET: POLICYCATEGORYS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.PolicyCategory.ToListAsync());
    }

    // GET: POLICYCATEGORYS/Details/5
    public async Task<IActionResult> Details(int? categoryid)
    {
        if (categoryid == null)
        {
            return NotFound();
        }

        var policycategory = await _context.PolicyCategory
            .FirstOrDefaultAsync(m => m.categoryId == categoryid);
        if (policycategory == null)
        {
            return NotFound();
        }

        return View(policycategory);
    }

    // GET: POLICYCATEGORYS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: POLICYCATEGORYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("categoryId,categoryName,subCategory,Policies")] PolicyCategory policycategory)
    {
        if (ModelState.IsValid)
        {
            _context.Add(policycategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(policycategory);
    }

    // GET: POLICYCATEGORYS/Edit/5
    public async Task<IActionResult> Edit(int? categoryid)
    {
        if (categoryid == null)
        {
            return NotFound();
        }

        var policycategory = await _context.PolicyCategory.FindAsync(categoryid);
        if (policycategory == null)
        {
            return NotFound();
        }
        return View(policycategory);
    }

    // POST: POLICYCATEGORYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? categoryid, [Bind("categoryId,categoryName,subCategory,Policies")] PolicyCategory policycategory)
    {
        if (categoryid != policycategory.categoryId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(policycategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PolicyCategoryExists(policycategory.categoryId))
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
        return View(policycategory);
    }

    // GET: POLICYCATEGORYS/Delete/5
    public async Task<IActionResult> Delete(int? categoryid)
    {
        if (categoryid == null)
        {
            return NotFound();
        }

        var policycategory = await _context.PolicyCategory
            .FirstOrDefaultAsync(m => m.categoryId == categoryid);
        if (policycategory == null)
        {
            return NotFound();
        }

        return View(policycategory);
    }

    // POST: POLICYCATEGORYS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? categoryid)
    {
        var policycategory = await _context.PolicyCategory.FindAsync(categoryid);
        if (policycategory != null)
        {
            _context.PolicyCategory.Remove(policycategory);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PolicyCategoryExists(int? categoryid)
    {
        return _context.PolicyCategory.Any(e => e.categoryId == categoryid);
    }
}
