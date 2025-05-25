using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ENROLLMENTSYSTEMBACKEND.Services;
using System.Security.Claims;

[ApiController]
[Route("api/fees")]
[Authorize]
public class FeeController : ControllerBase
{
    private readonly IFeeService _feeService;

    public FeeController(IFeeService feeService)
    {
        _feeService = feeService;
    }

    [HttpGet("{studentId}")]
    public async Task<IActionResult> GetFees(string studentId)
    {
        if (!CanAccessStudentData(studentId)) return Forbid();
        var fees = await _feeService.GetFeesByStudentIdAsync(studentId);
        return Ok(fees);
    }

    private bool CanAccessStudentData(string studentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = User.FindFirstValue(ClaimTypes.Role);
        return userId == studentId || role == "Admin";
    }
}