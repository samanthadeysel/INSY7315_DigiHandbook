using Digital_Handbook_Portal.Models;
using Google.Apis.Util;
using HandbookApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;
        private readonly ICloudStorageService _storageService;

        public ResourcesController(Digital_Handbook_PortalContext context, ICloudStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        // GET: api/Policies
        [HttpGet]
            public async Task<ActionResult<ApiResponse<List<Resource>>>> GetResource()
            {
                try
                {
                    var resources = await _context.Resource
                        .AsNoTracking()
                        .Include(r => r.Id)
                        .OrderBy(r => r.Title)
                        .Select(r => new Resource
                        {
                            Id = r.Id,
                            Title = r.Title,
                            Description = r.Description,
                            Category = r.Category,
                            ResourceUrl = r.ResourceUrl,
                            BreadcrumbPath = r.BreadcrumbPath
                        })
                        .ToListAsync();

                    return Ok(new ApiResponse<List<Resource>> { Success = true, Data = resources });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new ApiResponse<List<Resource>>
                    {
                        Success = false,
                        Message = $"API Internal Error: {ex.Message} | Inner: {ex.InnerException?.Message}"
                    });
                }
            }

            // GET: api/Resources/5
            [HttpGet("{id}")]
            public async Task<ActionResult<ApiResponse<Resource>>> GetResource(int id)
            {
                var resource = await _context.Resource.FindAsync(id);
                if (resource == null)
                {
                    return NotFound(new ApiResponse<Resource> { Success = false, Message = $"Resource with ID {id} was not found." });
                }

                return Ok(new ApiResponse<Resource> { Success = true, Data = resource });
            }

            // POST: api/Resources
            [HttpPost]
            //[Consumes("multipart/form-data")]
            public async Task<IActionResult> CreateResource(Resource resource, IFormFile? uploadFile)
            {
                ModelState.Remove(nameof(Resource.Id));

                if (uploadFile != null && uploadFile.Length > 0)
                {
                    try
                    {
                        string uploadedUrl = await _storageService.UploadFileAsync(uploadFile, "resources");
                        resource.ResourceUrl = uploadedUrl;
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, new ApiResponse<Resource> { Success = false, Message = $"File Upload Failed: {ex.Message}" });
                    }
                }

                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return BadRequest(new ApiResponse<Resource> { Success = false, Message = $"Validation Error: {errors}" });
                }

                resource.Id = 0;

                _context.Resource.Add(resource);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<Resource> { Success = true, Message = "Resource created successfully.", Data = resource });
            }

            // PUT: api/Resources/5
            [HttpPut("{id}")]
            [Consumes("multipart/form-data")]
            public async Task<ActionResult<ApiResponse<Resource>>> UpdateResource(int id, [FromForm] Resource resource, IFormFile? uploadFile)
            {
                if (id != resource.Id)
                {
                    return BadRequest(new ApiResponse<Resource> { Success = false, Message = "Resource ID mismatch." });
                }

                var existingResource = await _context.Resource.FindAsync(id);
                if (existingResource == null)
                {
                    return NotFound(new ApiResponse<Resource> { Success = false, Message = "Resource not found." });
                }

                existingResource.Title = resource.Title;
                existingResource.Category = resource.Category;
                existingResource.Description = resource.Description;
                existingResource.BreadcrumbPath = resource.BreadcrumbPath;

                if (uploadFile != null && uploadFile.Length > 0)
                {
                    if (!string.IsNullOrWhiteSpace(existingResource.ResourceUrl))
                    {
                        await _storageService.DeleteFileAsync(existingResource.ResourceUrl);
                    }

                    string uploadedUrl = await _storageService.UploadFileAsync(uploadFile, "resources");
                    existingResource.ResourceUrl = uploadedUrl;
                }
                else if (!string.IsNullOrWhiteSpace(resource.ResourceUrl))
                {
                    existingResource.ResourceUrl = resource.ResourceUrl;
                }

                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<Resource> { Success = true, Message = "Resource updated successfully.", Data = existingResource });
            }

            // DELETE: api/Resources/5
            [HttpDelete("{id}")]
            public async Task<ActionResult<ApiResponse<bool>>> DeleteResource(int id)
            {
                var resource = await _context.Resource.FindAsync(id);
                if (resource == null)
                {
                    return NotFound(new ApiResponse<bool> { Success = false, Message = "Resource not found.", Data = false });
                }

                if (!string.IsNullOrWhiteSpace(resource.ResourceUrl))
                {
                    await _storageService.DeleteFileAsync(resource.ResourceUrl);
                }

                _context.Resource.Remove(resource);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<bool> { Success = true, Message = "Resource deleted successfully.", Data = true });
            }
        }
    }
