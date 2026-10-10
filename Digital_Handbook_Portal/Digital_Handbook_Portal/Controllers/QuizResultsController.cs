using Digital_Handbook_Portal.Models;
using Digital_Handbook_Portal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class QuizResultsController : Controller
{
    private readonly Digital_Handbook_PortalContext _context;

    public QuizResultsController(Digital_Handbook_PortalContext context)
    {
        _context = context;
    }

    // GET: QuizResults (User CPD Leaderboard/Summary)
    public async Task<IActionResult> Index()
    {
        var results = await _context.QuizResult
            .Include(q => q.Quiz)
            .Include(q => q.User)
            .ToListAsync();

        var userSummaries = results
            .GroupBy(q => q.UserId)
            .Select(g =>
            {
                var firstUser = g.FirstOrDefault()?.User;
                string userEmail = firstUser?.email ?? g.Key.ToString();

                string derivedName = userEmail.Contains("@")
                    ? string.Join(" ", userEmail.Split('@')[0].Split('.', '_', '-').Select(s => s.Length > 0 ? char.ToUpper(s[0]) + s.Substring(1) : s))
                    : "Staff Member";

                string displayName = !string.IsNullOrWhiteSpace(firstUser?.FullName)
                    ? firstUser.FullName
                    : derivedName;

                return new UserCpdSummaryViewModel
                {
                    UserId = g.Key.ToString(),
                    UserName = displayName,
                    Email = userEmail,
                    TotalCpdPoints = g.Sum(q => q.CpdPointsAwarded),
                    TotalQuizzesAttempted = g.Count(),
                    TotalQuizzesPassed = g.Count(q => q.IsPassed)
                };
            })
            .OrderByDescending(u => u.TotalCpdPoints)
            .ToList();

        return View(userSummaries);
    }

    // GET: QuizResults/UserDetails/1 (All quiz attempts for a specific user)
    public async Task<IActionResult> UserDetails(string id)
    {
        if (string.IsNullOrEmpty(id) || !int.TryParse(id, out int parsedUserId))
            return NotFound();

        var userAttempts = await _context.QuizResult
            .Include(q => q.Quiz)
            .Include(q => q.User)
            .Where(q => q.UserId == parsedUserId)
            .OrderByDescending(q => q.CompletedAt)
            .ToListAsync();

        var selectedUser = userAttempts.FirstOrDefault()?.User;
        string userEmail = selectedUser?.email ?? id;

        string derivedName = userEmail.Contains("@")
            ? string.Join(" ", userEmail.Split('@')[0].Split('.', '_', '-').Select(s => s.Length > 0 ? char.ToUpper(s[0]) + s.Substring(1) : s))
            : "Staff Member";

        ViewData["UserName"] = !string.IsNullOrWhiteSpace(selectedUser?.FullName) ? selectedUser.FullName : derivedName;
        ViewData["UserEmail"] = userEmail;

        return View(userAttempts);
    }
}