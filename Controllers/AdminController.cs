using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ENROLLMENTSYSTEMBACKEND.Services;
using ENROLLMENTSYSTEMBACKEND.DTOs;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var dashboard = await _adminService.GetDashboardDataAsync();
        return Ok(dashboard);
    }

    [HttpGet("students")]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _adminService.GetAllStudentsAsync();
        return Ok(students);
    }

    [HttpGet("holds")]
    public async Task<IActionResult> GetHolds([FromQuery] string studentId)
    {
        var holds = await _adminService.GetHoldsAsync(studentId);
        return Ok(holds);
    }

    [HttpPost("holds")]
    public async Task<IActionResult> AddHold([FromBody] HoldDto hold)
    {
        await _adminService.AddHoldAsync(hold);
        return Ok(new { message = "Hold added successfully" });
    }

    [HttpPost("registration/open")]
    public async Task<IActionResult> OpenRegistration([FromBody] RegistrationPeriodDto period)
    {
        await _adminService.OpenRegistrationAsync(period);
        return Ok(new { message = "Registration period opened" });
    }

    [HttpPost("registration/close")]
    public async Task<IActionResult> CloseRegistration()
    {
        await _adminService.CloseRegistrationAsync();
        return Ok(new { message = "Registration period closed" });
    }
}