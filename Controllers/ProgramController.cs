using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ENROLLMENTSYSTEMBACKEND.Services;
using System.Security.Claims;

[ApiController]
[Route("api/program")]
[Authorize]
public class ProgramController : ControllerBase
{
    private readonly IProgramService _programService;

    public ProgramController(IProgramService programService)
    {
        _programService = programService;
    }

    [HttpGet("audit")]
    public async Task<IActionResult> GetProgramAudit([FromQuery] string studentId)
    {
        if (!CanAccessStudentData(studentId)) return Forbid();
        var audit = await _programService.GetProgramAuditAsync(studentId);
        return Ok(audit);
    }

    [HttpPost("academic-plan/generate")]
    public async Task<IActionResult> GenerateAcademicPlan([FromQuery] string studentId)
    {
        if (!CanAccessStudentData(studentId)) return Forbid();
        await _programService.GenerateAcademicPlanAsync(studentId);
        return Ok("Academic plan generated successfully.");
    }

    private bool CanAccessStudentData(string studentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = User.FindFirstValue(ClaimTypes.Role);
        return userId == studentId || role == "Admin";
    }
}