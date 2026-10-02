using Digital_Handbook_Portal.Models;
using HandbookApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PoliciesController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;
        private readonly ICloudStorageService _storageService;

        public PoliciesController(Digital_Handbook_PortalContext context, ICloudStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        // GET: api/Policies
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Policy>>>> GetPolicies()
        {
            var policies = await _context.Policy
                .Include(p => p.Category)
                .OrderBy(p => p.Title)
                .ToListAsync();

            return Ok(new ApiResponse<List<Policy>> { Success = true, Data = policies });
        }

        // GET: api/Policies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Policy>>> GetPolicy(int id)
        {
            var policy = await _context.Policy
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.policyId == id);

            if (policy == null)
            {
                return NotFound(new ApiResponse<Policy> { Success = false, Message = "Policy document not found." });
            }

            return Ok(new ApiResponse<Policy> { Success = true, Data = policy });
        }

        // GET: api/Policies/category/5
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<ApiResponse<List<Policy>>>> GetPoliciesByCategory(int categoryId)
        {
            var policies = await _context.Policy
                .Where(p => p.categoryId == categoryId)
                .Include(p => p.Category)
                .ToListAsync();

            return Ok(new ApiResponse<List<Policy>> { Success = true, Data = policies });
        }

        // POST: api/Policies
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<Policy>>> CreatePolicy([FromForm] Policy policy, IFormFile? pdfFile)
        {
            if (pdfFile != null && pdfFile.Length > 0)
            {
                try
                {
                    string uploadedUrl = await _storageService.UploadFileAsync(pdfFile, "policies");
                    policy.fileUrl = uploadedUrl;
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new ApiResponse<Policy> { Success = false, Message = $"GCS File Upload Failed: {ex.Message}" });
                }
            }

            _context.Policy.Add(policy);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Policy> { Success = true, Message = "Policy document created successfully.", Data = policy });
        }

        // PUT: api/Policies/5
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<Policy>>> UpdatePolicy(int id, [FromForm] Policy policy, IFormFile? pdfFile)
        {
            var existingPolicy = await _context.Policy.FindAsync(id);
            if (existingPolicy == null)
            {
                return NotFound(new ApiResponse<Policy> { Success = false, Message = "Policy not found." });
            }

            existingPolicy.Title = policy.Title;
            existingPolicy.contentSummary = policy.contentSummary;
            existingPolicy.specificCategory = policy.specificCategory;
            existingPolicy.categoryId = policy.categoryId;

            if (pdfFile != null && pdfFile.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(existingPolicy.fileUrl))
                {
                    await _storageService.DeleteFileAsync(existingPolicy.fileUrl);
                }

                string uploadedUrl = await _storageService.UploadFileAsync(pdfFile, "policies");
                existingPolicy.fileUrl = uploadedUrl;
            }

            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<Policy> { Success = true, Message = "Policy document updated successfully.", Data = existingPolicy });
        }

        // DELETE: api/Policies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeletePolicy(int id)
        {
            var policy = await _context.Policy.FindAsync(id);
            if (policy == null)
            {
                return NotFound(new ApiResponse<bool> { Success = false, Message = "Policy not found.", Data = false });
            }

            if (!string.IsNullOrWhiteSpace(policy.fileUrl))
            {
                try
                {
                    await _storageService.DeleteFileAsync(policy.fileUrl);
                }
                catch
                {
                }
            }

            _context.Policy.Remove(policy);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<bool> { Success = true, Message = "Policy and associated document deleted successfully.", Data = true });
        }
    }
}