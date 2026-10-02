using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public UsersController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<User>>>> GetUsers()
        {
            var users = await _context.User.ToListAsync();
            return Ok(new ApiResponse<List<User>> { Success = true, Data = users });
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound(new ApiResponse<User> { Success = false, Message = "User not found." });
            }

            return Ok(new ApiResponse<User> { Success = true, Data = user });
        }

        // POST: api/Users/admin-create (Admin creates application users)
        [HttpPost("admin-create")]
        public async Task<ActionResult<ApiResponse<User>>> AdminCreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<User> { Success = false, Message = "Invalid user payload." });
            }

            bool emailExists = await _context.User.AnyAsync(u => u.email.ToLower() == user.email.ToLower());
            if (emailExists)
            {
                return BadRequest(new ApiResponse<User> { Success = false, Message = "A user with this email address already exists." });
            }

            // Securely hash initial password before saving
            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<User> { Success = true, Message = "User account created successfully.", Data = user });
        }

        // POST: api/Users/login (User login via the Application)
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> UserLogin([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<AuthResponse> { Success = false, Message = "Invalid login payload." });
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.email.ToLower() == request.Email.ToLower());
            if (user == null)
            {
                return Unauthorized(new ApiResponse<AuthResponse> { Success = false, Message = "Invalid email or password." });
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.password);
            if (!isValidPassword)
            {
                return Unauthorized(new ApiResponse<AuthResponse> { Success = false, Message = "Invalid email or password." });
            }

            var authResponse = new AuthResponse
            {
                UserId = user.userId,
                Email = user.email,
                Token = Guid.NewGuid().ToString() // Replace with JWT generator if token generation is added
            };

            return Ok(new ApiResponse<AuthResponse>
            {
                Success = true,
                Message = "Application login successful.",
                Data = authResponse
            });
        }

        // POST: api/Users/admin-login (Admin login endpoint)
        [HttpPost("admin-login")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> AdminLogin([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<AuthResponse> { Success = false, Message = "Invalid admin login payload." });
            }

            // Admin verification logic will go here
            return Ok(new ApiResponse<AuthResponse>
            {
                Success = true,
                Message = "Admin login endpoint ready."
            });
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.userId)
            {
                return BadRequest(new ApiResponse<User> { Success = false, Message = "User ID mismatch." });
            }

            var existingUser = await _context.User.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound(new ApiResponse<User> { Success = false, Message = "User not found." });
            }

            existingUser.email = user.email;

            if (!string.IsNullOrWhiteSpace(user.password) && !user.password.StartsWith("$2a$"))
            {
                existingUser.password = BCrypt.Net.BCrypt.HashPassword(user.password);
            }

            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<User> { Success = true, Message = "User updated successfully.", Data = existingUser });
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound(new ApiResponse<bool> { Success = false, Message = "User not found.", Data = false });
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<bool> { Success = true, Message = "User deleted successfully.", Data = true });
        }
    }
}