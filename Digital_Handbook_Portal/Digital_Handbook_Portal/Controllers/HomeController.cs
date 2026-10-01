using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Digital_Handbook_Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly Digital_Handbook_PortalContext _context;

        public HomeController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalPolicies = await _context.Policy.CountAsync();
            ViewBag.TotalQuizzes = await _context.Quiz.CountAsync();
            ViewBag.TotalDoctors = await _context.Doctor.CountAsync();
            ViewBag.TotalEvents = await _context.Community.CountAsync();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}