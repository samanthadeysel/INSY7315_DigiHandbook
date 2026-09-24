
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Digital_Handbook_Portal.Models;

public class QuizsController : Controller
{
    private readonly Digital_Handbook_PortalContext _context;

    public QuizsController(Digital_Handbook_PortalContext context)
    {
        _context = context;
    }

    // GET: QUIZS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Quiz.ToListAsync());
    }

    // GET: QUIZS/Details/5
    public async Task<IActionResult> Details(int? quizid)
    {
        if (quizid == null)
        {
            return NotFound();
        }

        var quiz = await _context.Quiz
            .FirstOrDefaultAsync(m => m.quizId == quizid);
        if (quiz == null)
        {
            return NotFound();
        }

        return View(quiz);
    }

    // GET: QUIZS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: QUIZS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("quizId,title,questions,answers,score,totalQuestions,estimateTime")] Quiz quiz)
    {
        if (ModelState.IsValid)
        {
            _context.Add(quiz);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(quiz);
    }

    // GET: QUIZS/Edit/5
    public async Task<IActionResult> Edit(int? quizid)
    {
        if (quizid == null)
        {
            return NotFound();
        }

        var quiz = await _context.Quiz.FindAsync(quizid);
        if (quiz == null)
        {
            return NotFound();
        }
        return View(quiz);
    }

    // POST: QUIZS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? quizid, [Bind("quizId,title,questions,answers,score,totalQuestions,estimateTime")] Quiz quiz)
    {
        if (quizid != quiz.quizId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(quiz);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(quiz.quizId))
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
        return View(quiz);
    }

    // GET: QUIZS/Delete/5
    public async Task<IActionResult> Delete(int? quizid)
    {
        if (quizid == null)
        {
            return NotFound();
        }

        var quiz = await _context.Quiz
            .FirstOrDefaultAsync(m => m.quizId == quizid);
        if (quiz == null)
        {
            return NotFound();
        }

        return View(quiz);
    }

    // POST: QUIZS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? quizid)
    {
        var quiz = await _context.Quiz.FindAsync(quizid);
        if (quiz != null)
        {
            _context.Quiz.Remove(quiz);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool QuizExists(int? quizid)
    {
        return _context.Quiz.Any(e => e.quizId == quizid);
    }
}
