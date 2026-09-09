
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Digital_Handbook_Portal.Models;

public class ResourcesController : Controller
{
    private readonly Digital_Handbook_PortalContext _context;

    public ResourcesController(Digital_Handbook_PortalContext context)
    {
        _context = context;
    }

    // GET: RESOURCES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Resource.ToListAsync());
    }

    // GET: RESOURCES/Details/5
    public async Task<IActionResult> Details(string? title)
    {
        if (title == null)
        {
            return NotFound();
        }

        var resource = await _context.Resource
            .FirstOrDefaultAsync(m => m.Title == title);
        if (resource == null)
        {
            return NotFound();
        }

        return View(resource);
    }

    // GET: RESOURCES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: RESOURCES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,resourceType,linkUrl")] Resource resource)
    {
        if (ModelState.IsValid)
        {
            _context.Add(resource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(resource);
    }

    // GET: RESOURCES/Edit/5
    public async Task<IActionResult> Edit(string? title)
    {
        if (title == null)
        {
            return NotFound();
        }

        var resource = await _context.Resource.FindAsync(title);
        if (resource == null)
        {
            return NotFound();
        }
        return View(resource);
    }

    // POST: RESOURCES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? title, [Bind("Title,resourceType,linkUrl")] Resource resource)
    {
        if (title != resource.Title)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(resource);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceExists(resource.Title))
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
        return View(resource);
    }

    // GET: RESOURCES/Delete/5
    public async Task<IActionResult> Delete(string? title)
    {
        if (title == null)
        {
            return NotFound();
        }

        var resource = await _context.Resource
            .FirstOrDefaultAsync(m => m.Title == title);
        if (resource == null)
        {
            return NotFound();
        }

        return View(resource);
    }

    // POST: RESOURCES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? title)
    {
        var resource = await _context.Resource.FindAsync(title);
        if (resource != null)
        {
            _context.Resource.Remove(resource);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ResourceExists(string? title)
    {
        return _context.Resource.Any(e => e.Title == title);
    }
}
