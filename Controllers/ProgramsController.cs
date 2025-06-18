using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Student")]
    [ApiController]
    [Route("api/programs")]
    public class ProgramsController : ControllerBase
    {
        private readonly IProgramService _programService;
        private readonly ILogger<ProgramsController> _logger;

        public ProgramsController(IProgramService programService, ILogger<ProgramsController> logger)
        {
            _programService = programService;
            _logger = logger;
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

        //Get Student Courses by StudnetId
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentById(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }
            try
            {
                var student = await _programService.GetStudentByIdAsync(studentId);
                if (student == null)
                {
                    return NotFound("Student not found.");
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving student by ID: {StudentId}", studentId);
                return StatusCode(500, "Internal server error while retrieving student.");
            }

        }
}
}
