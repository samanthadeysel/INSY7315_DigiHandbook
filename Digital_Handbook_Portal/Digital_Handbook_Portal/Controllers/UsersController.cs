
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Digital_Handbook_Portal.Models;

public class UsersController : Controller
{
    private readonly Digital_Handbook_PortalContext _context;

    public UsersController(Digital_Handbook_PortalContext context)
    {
        _context = context;
    }

    // GET: USERS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.User.ToListAsync());
    }

    // GET: USERS/Details/5
    public async Task<IActionResult> Details(int? userid)
    {
        if (userid == null)
        {
            return NotFound();
        }

        var user = await _context.User
            .FirstOrDefaultAsync(m => m.userId == userid);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // GET: USERS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: USERS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("userId,email,password")] User user)
    {
        if (ModelState.IsValid)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    // GET: USERS/Edit/5
    public async Task<IActionResult> Edit(int? userid)
    {
        if (userid == null)
        {
            return NotFound();
        }

        var user = await _context.User.FindAsync(userid);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // POST: USERS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? userid, [Bind("userId,email,password")] User user)
    {
        if (userid != user.userId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.userId))
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
        return View(user);
    }

    // GET: USERS/Delete/5
    public async Task<IActionResult> Delete(int? userid)
    {
        if (userid == null)
        {
            return NotFound();
        }

        var user = await _context.User
            .FirstOrDefaultAsync(m => m.userId == userid);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // POST: USERS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? userid)
    {
        var user = await _context.User.FindAsync(userid);
        if (user != null)
        {
            _context.User.Remove(user);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UserExists(int? userid)
    {
        return _context.User.Any(e => e.userId == userid);
    }
}
