using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Student,Admin")]
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        //Get available courses for student
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var studentId = User.Identity?.Name;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized("Student ID is missing.");

            return Ok(await _courseService.GetAvailableCoursesAsync(studentId));
        }

        //Get course details by course code
        [HttpGet("{courseCode}")]
        public async Task<IActionResult> GetCourseDetails(string courseCode)
        {
            var course = await _courseService.GetCourseDetailsAsync(courseCode);
            if (course == null)
                return NotFound("Course not found.");
            return Ok(course);
        }

        //Get registered courses for student
        [HttpGet("registered")]
        public async Task<IActionResult> GetRegisteredCourses()
        {
            var studentId = User.Identity?.Name;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized("Student ID is missing.");

            return Ok(await _courseService.GetRegisteredCoursesAsync(studentId));
        }

        //Register for a course
        [HttpPost("register")]
        public async Task<IActionResult> RegisterCourse([FromBody] string courseCode)
        {
            var studentId = User.Identity?.Name;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized("Student ID is missing.");

            var result = await _courseService.RegisterCourseAsync(studentId, courseCode);
            if (!result)
                return BadRequest("Failed to register for course.");

            return Ok("Course registered successfully.");
        }

        //Drop a course
        [HttpPost("drop")]
        public async Task<IActionResult> DropCourse([FromBody] string courseCode)
        {
            var studentId = User.Identity?.Name;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized("Student ID is missing.");

            var result = await _courseService.DropCourseAsync(studentId, courseCode);
            if (!result)
                return BadRequest("Failed to drop course.");

            return Ok("Course dropped successfully.");
        }

        //Get prerequisites for a course
        [HttpGet("{courseCode}/prerequisites")]
        public async Task<IActionResult> GetPrerequisites(string courseCode)
        {
            var prerequisites = await _courseService.GetPrerequisitesAsync(courseCode);
            return Ok(prerequisites);
        }

        //Add a new course
        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> AddCourse([FromBody] CourseDto course)
        {
            var result = await _courseService.AddCourseAsync(course);
            if (!result)
                return BadRequest("Failed to add course.");

            return Ok("Course added successfully.");
        }

        //Update existing course
        [Authorize(Roles = "Admin")]
        [HttpPut("update/{courseCode}")]
        public async Task<IActionResult> UpdateCourse(string courseCode, [FromBody] CourseDto updatedCourse)
        {
            var result = await _courseService.UpdateCourseAsync(courseCode, updatedCourse);
            if (!result)
                return NotFound("Course not found or update failed.");

            return Ok("Course updated successfully.");
        }

        //Delete a course
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{courseCode}")]
        public async Task<IActionResult> DeleteCourse(string courseCode)
        {
            var result = await _courseService.DeleteCourseAsync(courseCode);
            if (!result)
                return NotFound("Course not found or deletion failed.");

            return Ok("Course deleted successfully.");
        }
    }
}
