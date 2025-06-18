using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Student")]
    [ApiController]
    [Route("api/programs")]
    public class ProgramsController : ControllerBase
    {
        private readonly IProgramService _programService;

        public ProgramsController(IProgramService programService)
        {
            _programService = programService;
        }

     
        //Gets the program audit for a student, including course statuses and completion progress.
        [HttpGet("audit")]
        public async Task<IActionResult> GetProgramAudit([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var audit = await _programService.GetProgramAuditAsync(studentId);
                if (audit == null)
                {
                    return NotFound("Program audit not found.");
                }
                return Ok(audit);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        //Get student by StudentId
        [HttpGet("student/{studentId}")] // Updated route
        public async Task<IActionResult> GetStudentById(string studentId)
        {
            try
            {
                var student = await _programService.GetStudentByIdAsync(studentId);
                if (student == null)
                {
                    return NotFound($"Student with ID {studentId} not found");
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving student");
                return StatusCode(500, "An error occurred while retrieving student");
            }
        }
    }
}