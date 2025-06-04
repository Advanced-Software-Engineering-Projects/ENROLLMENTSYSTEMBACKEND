using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/holds")]
    public class AdminHoldsController : ControllerBase
    {
        private readonly IHoldService _holdService;
        private readonly IStudentService _studentService;

        public AdminHoldsController(IHoldService holdService, IStudentService studentService)
        {
            _holdService = holdService;
            _studentService = studentService;
        }

        // Gets all students for hold management.
        [HttpGet("students")]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return Ok(students);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No students found"
            }
        }

        // Gets holds for a specific student or all holds if studentId is not provided.
        [HttpGet]
        public async Task<IActionResult> GetHolds([FromQuery] string? studentId)
        {
            try
            {
                var holds = await _holdService.GetHoldsAsync(studentId);
                return Ok(holds);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No holds found"
            }
        }

        // Checks if a student has any holds based on their presence in the Holds table.
        [HttpGet("check/{studentId}")]
        public async Task<IActionResult> CheckStudentHold(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var hasHolds = await _holdService.HasHoldsAsync(studentId);
                return Ok(new { StudentId = studentId, HasHolds = hasHolds });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "Student not found"
            }
        }

        // Adds a new hold for a student.
        [HttpPost]
        public async Task<IActionResult> AddHold([FromBody] HoldDto holdDto)
        {
            if (holdDto == null || string.IsNullOrEmpty(holdDto.StudentId) ||
                string.IsNullOrEmpty(holdDto.Service) || string.IsNullOrEmpty(holdDto.Reason))
            {
                return BadRequest("Student ID, service, and reason are required.");
            }

            try
            {
                    var addedHold = await _holdService.AddHoldAsync(holdDto);
                return CreatedAtAction(nameof(GetHolds), new { studentId = holdDto.StudentId }, addedHold);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Invalid student or service"
            }
        }

        // Removes a hold by its ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveHold(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Hold ID is required.");
            }

            try
            {
                await _holdService.RemoveHoldAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "Hold not found"
            }
        }
    }
}