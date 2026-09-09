using HandbookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Digital_Handbook_Portal;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public UsersApiController(Digital_Handbook_PortalContext context) => _context = context;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.email == request.Email && u.password == request.Password);

            if (user == null)
            {
                return Unauthorized(new LoginResponse { IsSuccess = false, Message = "Invalid email or password." });
            }

            return Ok(new LoginResponse
            {
                IsSuccess = true,
                Message = "Authentication successful.",
                SessionId = Guid.NewGuid().ToString(),
                Email = user.email
            });
        }
    }
}
