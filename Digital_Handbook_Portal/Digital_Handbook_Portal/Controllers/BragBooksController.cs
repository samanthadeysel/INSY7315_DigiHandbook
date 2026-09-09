
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Digital_Handbook_Portal.Models;

public class BragBooksController : Controller
{
    private readonly Digital_Handbook_PortalContext _context;

    public BragBooksController(Digital_Handbook_PortalContext context)
    {
        _context = context;
    }

    // GET: BRAGBOOKS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.BragBook.ToListAsync());
    }

    // GET: BRAGBOOKS/Details/5
    public async Task<IActionResult> Details(int? bragid)
    {
        if (bragid == null)
        {
            return NotFound();
        }

        var bragbook = await _context.BragBook
            .FirstOrDefaultAsync(m => m.bragId == bragid);
        if (bragbook == null)
        {
            return NotFound();
        }

        return View(bragbook);
    }

    // GET: BRAGBOOKS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: BRAGBOOKS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("bragId,content,senderType,recipientName,datePosted")] BragBook bragbook)
    {
        if (ModelState.IsValid)
        {
            _context.Add(bragbook);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(bragbook);
    }

    // GET: BRAGBOOKS/Edit/5
    public async Task<IActionResult> Edit(int? bragid)
    {
        if (bragid == null)
        {
            return NotFound();
        }

        var bragbook = await _context.BragBook.FindAsync(bragid);
        if (bragbook == null)
        {
            return NotFound();
        }
        return View(bragbook);
    }

    // POST: BRAGBOOKS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? bragid, [Bind("bragId,content,senderType,recipientName,datePosted")] BragBook bragbook)
    {
        if (bragid != bragbook.bragId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(bragbook);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BragBookExists(bragbook.bragId))
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
        return View(bragbook);
    }

    // GET: BRAGBOOKS/Delete/5
    public async Task<IActionResult> Delete(int? bragid)
    {
        if (bragid == null)
        {
            return NotFound();
        }

        var bragbook = await _context.BragBook
            .FirstOrDefaultAsync(m => m.bragId == bragid);
        if (bragbook == null)
        {
            return NotFound();
        }

        return View(bragbook);
    }

    // POST: BRAGBOOKS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? bragid)
    {
        var bragbook = await _context.BragBook.FindAsync(bragid);
        if (bragbook != null)
        {
            _context.BragBook.Remove(bragbook);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BragBookExists(int? bragid)
    {
        return _context.BragBook.Any(e => e.bragId == bragid);
    }
}
