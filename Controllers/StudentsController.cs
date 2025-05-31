using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Student")]
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

       
        //Retrieves a student's profile by their ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    return NotFound("Student not found.");
                }
                return Ok(student);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        //Updates a student's profile.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(string id, [FromBody] StudentDto studentDto)
        {
            if (string.IsNullOrEmpty(id) || studentDto == null || id != studentDto.StudentId)
            {
                return BadRequest("Student ID mismatch or invalid request.");
            }

            try
            {
                await _studentService.UpdateStudentAsync(studentDto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Student not found" or "Invalid data"
            }
        }

        //Uploads or updates a student's avatar image.
        [HttpPost("{id}/avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAvatar(string id, [FromForm] UploadAvatarRequest request)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Student ID is required.");
            }

            var file = request.Avatar;
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Invalid file type. Only JPG, JPEG, PNG, and GIF are allowed.");
            }

            // Validate file size (e.g., max 5MB)
            const long maxFileSize = 5 * 1024 * 1024; // 5MB
            if (file.Length > maxFileSize)
            {
                return BadRequest("File size exceeds 5MB limit.");
            }

            try
            {
                var avatarUrl = await _studentService.UploadAvatarAsync(id, file);
                return Ok(new { AvatarUrl = avatarUrl });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Student not found" or "Upload failed"
            }
        }
    }
}