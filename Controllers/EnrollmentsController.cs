using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Student")]
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        //Retrieves enrolled courses for a student.
        [HttpGet("enrolled")]
        public async Task<IActionResult> GetEnrolledCourses([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            var enrolledCourses = await _enrollmentService.GetEnrolledCoursesAsync(studentId);
            return Ok(enrolledCourses);
        }

        //Retrieves dropped courses for a student.
        [HttpGet("dropped")]
        public async Task<IActionResult> GetDroppedCourses([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            var droppedCourses = await _enrollmentService.GetDroppedCoursesAsync(studentId);
            return Ok(droppedCourses);
        }

        //Retrieves available courses for a student based on their program and prerequisites.
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableCourses([FromQuery] string studentId, [FromQuery] string program)
        {
            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(program))
            {
                return BadRequest("Student ID and program are required.");
            }

            var availableCourses = await _enrollmentService.GetAvailableCoursesAsync(studentId, program);
            return Ok(availableCourses);
        }

        //Retrieves the prerequisite graph for a specific course.
        [HttpGet("prerequisites/{courseCode}")]
        public async Task<IActionResult> GetPrerequisiteGraph([FromRoute] string courseCode)
        {
            if (string.IsNullOrEmpty(courseCode))
            {
                return BadRequest("Course code is required.");
            }

            var prerequisites = await _enrollmentService.GetPrerequisiteGraphAsync(courseCode);
            return Ok(prerequisites);
        }

        /// Enrolls a student in a course.
        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll([FromBody] EnrollmentDto enrollmentDto)
        {
            if (enrollmentDto == null || string.IsNullOrEmpty(enrollmentDto.StudentId) ||
                string.IsNullOrEmpty(enrollmentDto.CourseCode) || string.IsNullOrEmpty(enrollmentDto.Semester))
            {
                return BadRequest("Student ID, course code, and semester are required.");
            }

            try
            {
                await _enrollmentService.EnrollAsync(enrollmentDto.StudentId, enrollmentDto.CourseCode, enrollmentDto.Semester);
                return Ok(new { Message = $"Successfully enrolled in {enrollmentDto.CourseCode}" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Prerequisites not met" or "Course not available"
            }
        }

       
        //Drops a student from a course.
        [HttpDelete("drop")]
        public async Task<IActionResult> Drop([FromQuery] string studentId, [FromQuery] string courseCode)
        {
            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(courseCode))
            {
                return BadRequest("Student ID and course code are required.");
            }

            try
            {
                await _enrollmentService.DropAsync(studentId, courseCode);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Course not enrolled"
            }
        }
    }
}