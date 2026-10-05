using BCrypt.Net;
using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        // Hardcoded admin store
        private readonly List<(string Email, string Password, string Name, int Id)> _admins = new()
        {
            ("admin1@pmbeye.co.za", "Admin123!", "Tracy", 1),
            ("admin2@pmbeye.co.za", "Admin123!", "Allison", 2),
            ("admin3@pmbeye.co.za", "Admin123!", "Kelly", 3)
        };

        public AuthController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        // POST: api/Auth/login (Application User Login)
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new ApiResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Email and password are required."
                });
            }

            var user = await _context.User
                .FirstOrDefaultAsync(u => u.email.ToLower() == request.Email.ToLower());

            if (user == null)
            {
                return Unauthorized(new ApiResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
            }

            bool isValidPassword = user.password.StartsWith("$2a$")
                ? BCrypt.Net.BCrypt.Verify(request.Password, user.password)
                : user.password == request.Password;

            if (!isValidPassword)
            {
                return Unauthorized(new ApiResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
            }

            var authData = new AuthResponse
            {
                UserId = user.userId,
                Email = user.email,
                Name = user.email,
                Token = "app-user-jwt-token-" + Guid.NewGuid()
            };

            return Ok(new ApiResponse<AuthResponse>
            {
                Success = true,
                Message = "Login successful.",
                Data = authData
            });
        }

        // POST: api/Auth/admin-login (MVC Web Portal Admin Login)
        [HttpPost("admin-login")]
        public ActionResult<ApiResponse<AuthResponse>> AdminLogin([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new ApiResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Email and password are required."
                });
            }

            var admin = _admins.FirstOrDefault(a =>
                a.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase) &&
                a.Password == request.Password);

            if (admin == default)
            {
                return Unauthorized(new ApiResponse<AuthResponse>
                {
                    Success = false,
                    Message = "Invalid admin credentials."
                });
            }

            var authData = new AuthResponse
            {
                UserId = admin.Id,
                Email = admin.Email,
                Name = admin.Name,
                Token = "admin-session-token-" + Guid.NewGuid()
            };

            return Ok(new ApiResponse<AuthResponse>
            {
                Success = true,
                Message = "Admin login successful.",
                Data = authData
            });
        }
    }
}