using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ENROLLMENTSYSTEMBACKEND.DTOs; 

[ApiController]
[Route("api/courses")]
[Authorize]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableCourses([FromQuery] string studentId, [FromQuery] string semester)
    {
        if (!CanAccessStudentData(studentId)) return Forbid();
        var courses = await _courseService.GetAvailableCoursesAsync(studentId, semester);
        return Ok(courses);
    }

    [HttpPost("enroll")]
    public async Task<IActionResult> EnrollCourse([FromBody] EnrollCourseDto enrollDto)
    {
        if (!CanAccessStudentData(enrollDto.StudentId)) return Forbid();
        await _courseService.EnrollCourseAsync(enrollDto.StudentId, enrollDto.CourseId, enrollDto.Semester);
        return Ok(new { message = "Enrolled successfully" });
    }

    [HttpPost("drop")]
    public async Task<IActionResult> DropCourse([FromQuery] string studentId, [FromQuery] int courseId, [FromQuery] string semester)
    {
        if (!CanAccessStudentData(studentId)) return Forbid();
        await _courseService.DropCourseAsync(studentId, courseId, semester);
        return Ok(new { message = "Dropped successfully" });
    }

    [HttpGet("{id}/prerequisites")]
    public async Task<IActionResult> GetPrerequisites(int id)
    {
        var prerequisites = await _courseService.GetPrerequisitesAsync(id);
        return Ok(prerequisites);
    }

    [HttpGet("prerequisites/graph")]
    public async Task<IActionResult> GetPrerequisiteGraph()
    {
        var graph = await _courseService.GetPrerequisiteGraphAsync();
        return Ok(graph);
    }

    private bool CanAccessStudentData(string studentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = User.FindFirstValue(ClaimTypes.Role);
        return userId == studentId || role == "Admin";
    }
}