using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ENROLLMENTSYSTEMBACKEND.Services;
using System.Security.Claims;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TranscriptController : ControllerBase
    {
        private readonly ITranscriptService _transcriptService;
        private readonly IHoldService _holdService;

        public TranscriptController(
            ITranscriptService transcriptService,
            IHoldService holdService)
        {
            _transcriptService = transcriptService;
            _holdService = holdService;
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadTranscript()
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized();

            if (!await _holdService.CanAccessServiceAsync(studentId, "view_transcript"))
                return Forbid("Student is on hold and cannot access transcript");

            try
            {
                var pdfBytes = await _transcriptService.GenerateTranscriptPdfAsync(studentId);
                return File(pdfBytes, "application/pdf", $"transcript_{studentId}.pdf");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("gpa")]
        public async Task<IActionResult> GetGpa()
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized();

            if (!await _holdService.CanAccessServiceAsync(studentId, "view_grades"))
                return Forbid("Student is on hold and cannot view grades");

            var gpa = await _transcriptService.GetStudentGpaAsync(studentId);
            return Ok(new { gpa });
        }
    }
}