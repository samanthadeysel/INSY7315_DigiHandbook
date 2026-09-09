
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Digital_Handbook_Portal.Models;

public class CommunitiesController : Controller
{
    private readonly Digital_Handbook_PortalContext _context;

    public CommunitiesController(Digital_Handbook_PortalContext context)
    {
        _context = context;
    }

    // GET: COMMUNITYS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Community.ToListAsync());
    }

    // GET: COMMUNITYS/Details/5
    public async Task<IActionResult> Details(int? eventid)
    {
        if (eventid == null)
        {
            return NotFound();
        }

        var community = await _context.Community
            .FirstOrDefaultAsync(m => m.eventId == eventid);
        if (community == null)
        {
            return NotFound();
        }

        return View(community);
    }

    // GET: COMMUNITYS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: COMMUNITYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("eventId,title,description,eventDateTime,location")] Community community)
    {
        if (ModelState.IsValid)
        {
            _context.Add(community);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(community);
    }

    // GET: COMMUNITYS/Edit/5
    public async Task<IActionResult> Edit(int? eventid)
    {
        if (eventid == null)
        {
            return NotFound();
        }

        var community = await _context.Community.FindAsync(eventid);
        if (community == null)
        {
            return NotFound();
        }
        return View(community);
    }

    // POST: COMMUNITYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? eventid, [Bind("eventId,title,description,eventDateTime,location")] Community community)
    {
        if (eventid != community.eventId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(community);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityExists(community.eventId))
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
        return View(community);
    }

    // GET: COMMUNITYS/Delete/5
    public async Task<IActionResult> Delete(int? eventid)
    {
        if (eventid == null)
        {
            return NotFound();
        }

        var community = await _context.Community
            .FirstOrDefaultAsync(m => m.eventId == eventid);
        if (community == null)
        {
            return NotFound();
        }

        return View(community);
    }

    // POST: COMMUNITYS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? eventid)
    {
        var community = await _context.Community.FindAsync(eventid);
        if (community != null)
        {
            _context.Community.Remove(community);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CommunityExists(int? eventid)
    {
        return _context.Community.Any(e => e.eventId == eventid);
    }
}
