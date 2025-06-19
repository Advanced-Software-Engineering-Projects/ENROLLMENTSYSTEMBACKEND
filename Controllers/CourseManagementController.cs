using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [ApiController]
    [Route("api/course-management")]
    [Authorize(Roles = "Admin")]
    public class CourseManagementController : ControllerBase
    {
        private readonly ICourseManagementService _courseManagementService;

        public CourseManagementController(ICourseManagementService courseManagementService)
        {
            _courseManagementService = courseManagementService;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetRegistrationStatus()
        {
            try
            {
                var status = await _courseManagementService.GetRegistrationStatusAsync();
                return Ok(status);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("open")]
        public async Task<IActionResult> OpenCourseRegistration([FromBody] CourseManagementDto request)
        {
            try
            {
                if (request == null || !request.CourseCodes.Any())
                {
                    return BadRequest("Course codes are required");
                }

                await _courseManagementService.OpenCourseRegistrationAsync(request);
                return Ok(new { message = $"Registration period opened successfully for {string.Join(", ", request.CourseCodes)}" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("close")]
        public async Task<IActionResult> CloseCourseRegistration([FromBody] CloseCourseRegistrationDto request)
        {
            try
            {
                if (request == null || !request.CourseCodes.Any())
                {
                    return BadRequest("Course codes are required");
                }

                await _courseManagementService.CloseCourseRegistrationAsync(request);
                return Ok(new { message = $"Registration closed successfully for {string.Join(", ", request.CourseCodes)}" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetRegistrationMetrics()
        {
            try
            {
                var metrics = await _courseManagementService.GetRegistrationMetricsAsync();
                return Ok(metrics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            try
            {
                var courses = await _courseManagementService.GetAllCoursesAsync();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}