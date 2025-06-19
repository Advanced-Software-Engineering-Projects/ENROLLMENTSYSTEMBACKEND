using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ICourseManagementService _courseManagementService;

        public CoursesController(ICourseService courseService, ICourseManagementService courseManagementService)
        {
            _courseService = courseService;
            _courseManagementService = courseManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseManagementService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseById(string courseId)
        {
            var course = await _courseService.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpGet("by-program/{program}")]
        public async Task<IActionResult> GetCoursesByProgram(string program)
        {
            var courses = await _courseService.GetCoursesByProgramAsync(program);
            return Ok(courses);
        }

        [HttpGet("by-year/{program}/{year}")]
        public async Task<IActionResult> GetCoursesByProgramAndYear(string program, int year)
        {
            var courses = await _courseService.GetCoursesByProgramAndYearAsync(program, year);
            return Ok(courses);
        }
    }
}