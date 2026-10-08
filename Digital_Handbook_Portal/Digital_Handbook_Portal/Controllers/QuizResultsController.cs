
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Digital_Handbook_Portal.Models;

public class QuizResultsController : Controller
{
    private readonly Digital_Handbook_PortalContext _context;

    public QuizResultsController(Digital_Handbook_PortalContext context)
    {
        _context = context;
    }

    // GET: QUIZRESULTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.QuizResult.ToListAsync());
    }

    // GET: QUIZRESULTS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var quizresult = await _context.QuizResult
            .FirstOrDefaultAsync(m => m.Id == id);
        if (quizresult == null)
        {
            return NotFound();
        }

        return View(quizresult);
    }

    // GET: QUIZRESULTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: QUIZRESULTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,UserId,User,QuizId,Quiz,ScorePercentage,CpdPointsAwarded,IsPassed,CompletedAt")] QuizResult quizresult)
    {
        if (ModelState.IsValid)
        {
            _context.Add(quizresult);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(quizresult);
    }

    // GET: QUIZRESULTS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var quizresult = await _context.QuizResult.FindAsync(id);
        if (quizresult == null)
        {
            return NotFound();
        }
        return View(quizresult);
    }

    // POST: QUIZRESULTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,UserId,User,QuizId,Quiz,ScorePercentage,CpdPointsAwarded,IsPassed,CompletedAt")] QuizResult quizresult)
    {
        if (id != quizresult.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(quizresult);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizResultExists(quizresult.Id))
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
        return View(quizresult);
    }

    // GET: QUIZRESULTS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var quizresult = await _context.QuizResult
            .FirstOrDefaultAsync(m => m.Id == id);
        if (quizresult == null)
        {
            return NotFound();
        }

        return View(quizresult);
    }

    // POST: QUIZRESULTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var quizresult = await _context.QuizResult.FindAsync(id);
        if (quizresult != null)
        {
            _context.QuizResult.Remove(quizresult);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool QuizResultExists(int? id)
    {
        return _context.QuizResult.Any(e => e.Id == id);
    }
}
