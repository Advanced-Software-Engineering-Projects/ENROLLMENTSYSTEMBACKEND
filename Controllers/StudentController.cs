using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ENROLLMENTSYSTEMBACKEND.Services;
using System.Security.Claims;
using ENROLLMENTSYSTEMBACKEND.DTOs;

[ApiController]
[Route("api/students")]
[Authorize]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet("{id}/dashboard")]
    public async Task<IActionResult> GetDashboard(string id)
    {
        if (!CanAccessStudentData(id)) return Forbid();
        try
        {
            var dashboard = await _studentService.GetDashboardDataAsync(id);
            return Ok(dashboard);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/grades")]
    public async Task<IActionResult> GetGrades(string id)
    {
        if (!CanAccessStudentData(id)) return Forbid();
        try
        {
            var grades = await _studentService.GetGradesAsync(id);
            return Ok(grades);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/timetable")]
    public async Task<IActionResult> GetTimetable(string id)
    {
        if (!CanAccessStudentData(id)) return Forbid();
        try
        {
            var timetable = await _studentService.GetTimetableAsync(id);
            return Ok(timetable);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfile(string id)
    {
        if (!CanAccessStudentData(id)) return Forbid();
        try
        {
            var profile = await _studentService.GetProfileAsync(id);
            return Ok(profile);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfile(string id, [FromBody] StudentProfileDto profile)
    {
        if (!CanAccessStudentData(id)) return Forbid();
        try
        {
            var updatedProfile = await _studentService.UpdateProfileAsync(id, profile);
            return Ok(updatedProfile);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/academic-records")]
    public async Task<IActionResult> GetAcademicRecords(string id)
    {
        if (!CanAccessStudentData(id)) return Forbid();
        try
        {
            var records = await _studentService.GetAcademicRecordsAsync(id);
            return Ok(records);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    private bool CanAccessStudentData(string studentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = User.FindFirstValue(ClaimTypes.Role);
        return userId == studentId || role == "Admin";
    }
}