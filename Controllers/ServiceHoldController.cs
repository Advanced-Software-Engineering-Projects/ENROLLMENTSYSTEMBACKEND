using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [ApiController]
    [Route("api/service-holds")]
    //[Authorize(Roles = "Admin")]
    public class ServiceHoldController : ControllerBase
    {
        private readonly IServiceHoldService _serviceHoldService;
        private readonly ILogger<ServiceHoldController> _logger;

        public ServiceHoldController(
            IServiceHoldService serviceHoldService,
            ILogger<ServiceHoldController> logger)
        {
            _serviceHoldService = serviceHoldService;
            _logger = logger;
        }

        [HttpGet("students")]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _serviceHoldService.GetAllStudentsAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving students");
                return StatusCode(500, "An error occurred while retrieving students");
            }
        }

        [HttpGet("students/{studentId}")]
        public async Task<IActionResult> GetStudentWithHolds(string studentId)
        {
            try
            {
                var studentWithHolds = await _serviceHoldService.GetStudentWithHoldsAsync(studentId);
                if (studentWithHolds == null)
                {
                    return NotFound($"Student with ID {studentId} not found");
                }
                return Ok(studentWithHolds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving student holds");
                return StatusCode(500, "An error occurred while retrieving student holds");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddHold([FromBody] CreateServiceHoldDto holdDto)
        {
            if (holdDto == null || string.IsNullOrEmpty(holdDto.StudentId) || 
                holdDto.ServiceId <= 0 || string.IsNullOrEmpty(holdDto.Reason))
            {
                return BadRequest("Invalid hold data");
            }

            try
            {
                var hold = await _serviceHoldService.AddHoldAsync(holdDto);
                return CreatedAtAction(nameof(GetStudentWithHolds), new { studentId = holdDto.StudentId }, hold);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding hold");
                return StatusCode(500, "An error occurred while adding hold");
            }
        }

        [HttpDelete("{holdId}")]
        public async Task<IActionResult> RemoveHold(string holdId)
        {
            try
            {
                var result = await _serviceHoldService.RemoveHoldAsync(holdId);
                if (!result)
                {
                    return NotFound($"Hold with ID {holdId} not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing hold");
                return StatusCode(500, "An error occurred while removing hold");
            }
        }

        [HttpGet("pdf/{studentId}")]
        public async Task<IActionResult> GenerateHoldsPdf(string studentId)
        {
            try
            {
                var pdfBytes = await _serviceHoldService.GenerateHoldsPdfAsync(studentId);
                return File(pdfBytes, "application/pdf", $"Holds_{studentId}.pdf");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF");
                return StatusCode(500, "An error occurred while generating PDF");
            }
        }
    }
}