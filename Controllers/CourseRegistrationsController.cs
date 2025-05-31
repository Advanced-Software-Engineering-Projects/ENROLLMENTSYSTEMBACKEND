using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Student")]
    [ApiController]
    [Route("api/course-registrations")]
    public class CourseRegistrationsController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseRegistrationsController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableCourses([FromQuery] string studentId)
        {
            var courses = await _courseService.GetAvailableCoursesAsync(studentId);
            return Ok(courses);
        }

        [HttpPost("register-course")]
        public async Task<IActionResult> RegisterCourse([FromBody] CourseRegistrationDto registrationDto)
        {
            if (registrationDto == null || string.IsNullOrEmpty(registrationDto.StudentId) || string.IsNullOrEmpty(registrationDto.CourseId))
            {
                return BadRequest("Invalid registration data.");
            }
            var result = await _courseService.RegisterCourseAsync(registrationDto);
            if (!result)
            {
                return BadRequest("Registration failed. Please check the course and student details.");
            }
            return Ok("Course registered successfully.");
        }

        [HttpGet("registered")]
        public async Task<IActionResult> GetRegisteredCourses([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }
            var courses = await _courseService.GetRegisteredCoursesAsync(studentId);
            return Ok(courses);
        }

        [HttpDelete("unregister")]
        public async Task<IActionResult> UnregisterCourse([FromBody] CourseRegistrationDto registrationDto)
        {
            if (registrationDto == null || string.IsNullOrEmpty(registrationDto.StudentId) || string.IsNullOrEmpty(registrationDto.CourseId))
            {
                return BadRequest("Invalid unregistration data.");
            }
            var result = await _courseService.UnregisterCourseAsync(registrationDto);
            if (!result)
            {
                return BadRequest("Unregistration failed. Please check the course and student details.");
            }
            return Ok("Course unregistered successfully.");
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetCourseHistory([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }
            var history = await _courseService.GetCourseHistoryAsync(studentId);
            return Ok(history);
        }

        [HttpGet("prerequisites")]
        public async Task<IActionResult> GetCoursePrerequisites([FromQuery] string courseId)
        {
            if (string.IsNullOrEmpty(courseId))
            {
                return BadRequest("Course ID is required.");
            }
            var prerequisites = await _courseService.GetCoursePrerequisitesAsync(courseId);
            return Ok(prerequisites);
        }
    }
}