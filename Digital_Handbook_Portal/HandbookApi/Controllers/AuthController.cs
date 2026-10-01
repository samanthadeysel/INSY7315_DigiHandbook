using HandbookApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public AuthController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

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
                .FirstOrDefaultAsync(u => u.email.ToLower() == request.Email.ToLower() && u.password == request.Password);

            if (user == null)
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
                Token = "dummy-jwt-token-for-development"
            };

            return Ok(new ApiResponse<AuthResponse>
            {
                Success = true,
                Message = "Login successful.",
                Data = authData
            });
        }
    }
}