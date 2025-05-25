using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ENROLLMENTSYSTEMBACKEND.Services;
using System.Security.Claims;
using ENROLLMENTSYSTEMBACKEND.DTOs;

[ApiController]
[Route("api/forms")]
public class FormsController : ControllerBase
{
    private readonly IFormService _formService;

    public FormsController(IFormService formService)
    {
        _formService = formService;
    }

    [HttpPost("{formType}")]
    [Authorize]
    public async Task<IActionResult> SubmitForm(string formType, [FromBody] FormSubmissionDto formData)
    {
        if (!CanAccessStudentData(formData.StudentId)) return Forbid();
        await _formService.SubmitFormAsync(formType, formData);
        return Ok(new { message = "Form submitted successfully" });
    }

    [HttpGet("admin/forms/{formType}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetFormSubmissions(string formType)
    {
        var submissions = await _formService.GetFormSubmissionsAsync(formType);
        return Ok(submissions);
    }

    private bool CanAccessStudentData(string studentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = User.FindFirstValue(ClaimTypes.Role);
        return userId == studentId || role == "Admin";
    }
}