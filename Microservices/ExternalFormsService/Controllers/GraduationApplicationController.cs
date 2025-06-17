using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ExternalFormsService.Services;
using ExternalFormsService.DTOs;

namespace ExternalFormsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GraduationApplicationController : ControllerBase
    {
        private readonly IGraduationApplicationService _applicationService;
        private readonly ILogger<GraduationApplicationController> _logger;

        public GraduationApplicationController(
            IGraduationApplicationService applicationService,
            ILogger<GraduationApplicationController> logger)
        {
            _applicationService = applicationService;
            _logger = logger;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> SubmitApplication([FromBody] GraduationApplicationDto application)
        {
            try
            {
                var result = await _applicationService.SubmitGraduationApplicationAsync(application);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting graduation application for student: {StudentId}", application.StudentId);
                return StatusCode(500, "An error occurred while processing your application.");
            }
        }

        [HttpGet("{applicationId}")]
        public async Task<IActionResult> GetApplicationStatus(string applicationId)
        {
            try
            {
                var result = await _applicationService.GetApplicationStatusAsync(applicationId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving application status: {ApplicationId}", applicationId);
                return StatusCode(500, "An error occurred while retrieving the application status.");
            }
        }

        [HttpPut("{applicationId}/process")]
        [Authorize(Roles = "Admin,Registrar")]
        public async Task<IActionResult> ProcessApplication(string applicationId, [FromBody] ApplicationProcessDto processRequest)
        {
            try
            {
                var result = await _applicationService.ProcessApplicationAsync(
                    applicationId,
                    processRequest.Status,
                    processRequest.Comments);

                if (!result)
                {
                    return NotFound();
                }
                return Ok(new { message = "Application processed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing application: {ApplicationId}", applicationId);
                return StatusCode(500, "An error occurred while processing the application.");
            }
        }
    }

    public class ApplicationProcessDto
    {
        public string Status { get; set; }
        public string Comments { get; set; }
    }
} 