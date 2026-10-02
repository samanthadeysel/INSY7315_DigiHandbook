using Digital_Handbook_Portal.Models;
using HandbookApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly Digital_Handbook_PortalContext _context;
        private readonly ICloudStorageService _storageService;

        public DoctorsController(Digital_Handbook_PortalContext context, ICloudStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        // GET: api/Doctors
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Doctor>>>> GetDoctors()
        {
            var doctors = await _context.Doctor.ToListAsync();
            return Ok(new ApiResponse<List<Doctor>> { Success = true, Data = doctors });
        }

        // GET: api/Doctors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Doctor>>> GetDoctor(int id)
        {
            var doctor = await _context.Doctor.FindAsync(id);
            if (doctor == null)
            {
                return NotFound(new ApiResponse<Doctor> { Success = false, Message = "Doctor not found." });
            }

            return Ok(new ApiResponse<Doctor> { Success = true, Data = doctor });
        }

        // POST: api/Doctors
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<Doctor>>> CreateDoctor([FromForm] Doctor doctor, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    string uploadedUrl = await _storageService.UploadFileAsync(imageFile, "doctors");
                    doctor.doctorImg = uploadedUrl;
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new ApiResponse<Doctor> { Success = false, Message = $"GCS Image Upload Failed: {ex.Message}" });
                }
            }

            _context.Doctor.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Doctor> { Success = true, Message = "Doctor entry created successfully.", Data = doctor });
        }

        // PUT: api/Doctors/5
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiResponse<Doctor>>> UpdateDoctor(int id, [FromForm] Doctor doctor, IFormFile? imageFile)
        {
            var existingDoctor = await _context.Doctor.FindAsync(id);
            if (existingDoctor == null)
            {
                return NotFound(new ApiResponse<Doctor> { Success = false, Message = "Doctor not found." });
            }

            existingDoctor.fName = doctor.fName;
            existingDoctor.lName = doctor.lName;
            existingDoctor.email = doctor.email;
            existingDoctor.phone = doctor.phone;
            existingDoctor.suiteNumber = doctor.suiteNumber;

            if (imageFile != null && imageFile.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(existingDoctor.doctorImg))
                {
                    await _storageService.DeleteFileAsync(existingDoctor.doctorImg);
                }

                string uploadedUrl = await _storageService.UploadFileAsync(imageFile, "doctors");
                existingDoctor.doctorImg = uploadedUrl;
            }

            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<Doctor> { Success = true, Message = "Doctor entry updated successfully.", Data = existingDoctor });
        }

        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctor.FindAsync(id);
            if (doctor == null)
            {
                return NotFound(new ApiResponse<bool> { Success = false, Message = "Doctor not found.", Data = false });
            }

            if (!string.IsNullOrWhiteSpace(doctor.doctorImg))
            {
                await _storageService.DeleteFileAsync(doctor.doctorImg);
            }

            _context.Doctor.Remove(doctor);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<bool> { Success = true, Message = "Doctor entry deleted successfully.", Data = true });
        }
    }
}