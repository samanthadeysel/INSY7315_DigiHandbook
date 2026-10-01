using HandbookApi.DTOs;
using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public DoctorsController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Doctor>>>> GetDoctors()
        {
            var doctors = await _context.Doctor.ToListAsync();
            return Ok(new ApiResponse<List<Doctor>> { Success = true, Data = doctors });
        }
    }
}