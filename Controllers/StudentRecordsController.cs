using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/student-records")]
    public class StudentRecordsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentRecordsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        //Gets a specific student by their ID
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


        /// Gets all student records with pagination.
        [HttpGet]
        public async Task<IActionResult> GetAllStudents([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and page size must be positive integers.");
            }

            try
            {
                var students = await _studentService.GetAllStudentsAsync(page, pageSize);
                var totalStudents = await _studentService.GetTotalStudentsCountAsync();
                var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

                return Ok(new
                {
                    Students = students,
                    TotalStudents = totalStudents,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = pageSize
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No students found"
            }
        }
    }
}