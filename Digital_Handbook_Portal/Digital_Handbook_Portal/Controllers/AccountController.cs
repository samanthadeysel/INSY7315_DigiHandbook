using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Digital_Handbook_Portal.Controllers
{
    public class AccountController : Controller
    {
        private readonly List<(string Email, string Password, string Name)> _admins = new()
        {
            ("admin1@pmbeye.co.za", "Admin123!", "Tracy"),
            ("admin2@pmbeye.co.za", "Admin123!", "Allison"),
            ("admin3@pmbeye.co.za", "Admin123!", "Kelly")
        };

        // GET: Account/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Please enter both email and password.";
                return View();
            }

            var admin = _admins.FirstOrDefault(a =>
                a.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                a.Password == password);

            if (admin != default)
            {
                HttpContext.Session.SetString("AdminEmail", admin.Email);
                HttpContext.Session.SetString("AdminName", admin.Name);

                return RedirectToAction("Index", "Users");
            }

            ViewBag.ErrorMessage = "Invalid admin credentials.";
            return View();
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}