using HandbookApi.DTOs;
using Digital_Handbook_Portal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PoliciesController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;

        public PoliciesController(Digital_Handbook_PortalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Policy>>>> GetPolicies()
        {
            var policies = await _context.Policy.Include(p => p.Category).ToListAsync();
            return Ok(new ApiResponse<List<Policy>> { Success = true, Data = policies });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Policy>>> GetPolicy(int id)
        {
            var policy = await _context.Policy.Include(p => p.Category).FirstOrDefaultAsync(p => p.policyId == id);
            if (policy == null)
            {
                return NotFound(new ApiResponse<Policy> { Success = false, Message = "Policy not found." });
            }

            return Ok(new ApiResponse<Policy> { Success = true, Data = policy });
        }

        [HttpGet("categories")]
        public async Task<ActionResult<ApiResponse<List<PolicyCategory>>>> GetCategories()
        {
            var categories = await _context.PolicyCategory.Include(c => c.Policies).ToListAsync();
            return Ok(new ApiResponse<List<PolicyCategory>> { Success = true, Data = categories });
        }
    }
}