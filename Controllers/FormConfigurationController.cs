using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [ApiController]
    [Route("api/form-configuration")]
    [Authorize(Roles = "Admin")]
    public class FormConfigurationController : ControllerBase
    {
        private readonly IFormConfigurationService _formConfigurationService;
        private readonly ILogger<FormConfigurationController> _logger;

        public FormConfigurationController(
            IFormConfigurationService formConfigurationService,
            ILogger<FormConfigurationController> logger)
        {
            _formConfigurationService = formConfigurationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFormSubmissions()
        {
            try
            {
                var submissions = await _formConfigurationService.GetAllFormSubmissionsAsync();
                return Ok(submissions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving form submissions");
                return StatusCode(500, "An error occurred while retrieving form submissions");
            }
        }

        [HttpPut("grade")]
        public async Task<IActionResult> UpdateGrade([FromBody] GradeUpdateDto gradeUpdateDto)
        {
            if (gradeUpdateDto == null || string.IsNullOrEmpty(gradeUpdateDto.NewGrade))
            {
                return BadRequest("Grade update data is required");
            }

            try
            {
                var result = await _formConfigurationService.UpdateGradeAsync(gradeUpdateDto);
                if (result)
                {
                    return Ok(new { success = true, message = "Grade updated successfully" });
                }
                else
                {
                    return BadRequest("Failed to update grade");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating grade");
                return StatusCode(500, "An error occurred while updating grade");
            }
        }

        [HttpPut("status/{formType}")]
        public async Task<IActionResult> UpdateStatus([FromBody] StatusUpdateDto statusUpdateDto, string formType)
        {
            if (statusUpdateDto == null || string.IsNullOrEmpty(statusUpdateDto.Status))
            {
                return BadRequest("Status update data is required");
            }

            if (!new[] { "reconsideration", "compassionateAegrotat", "completionProgramme" }.Contains(formType))
            {
                return BadRequest("Invalid form type");
            }

            try
            {
                var result = await _formConfigurationService.UpdateStatusAsync(statusUpdateDto, formType);
                if (result)
                {
                    return Ok(new { success = true, message = "Status updated successfully" });
                }
                else
                {
                    return BadRequest("Failed to update status");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status");
                return StatusCode(500, "An error occurred while updating status");
            }
        }

        [HttpPost("resend-email")]
        public async Task<IActionResult> ResendEmail([FromBody] EmailResendDto emailResendDto)
        {
            if (emailResendDto == null)
            {
                return BadRequest("Email resend data is required");
            }

            if (!new[] { "reconsideration", "compassionateAegrotat", "completionProgramme" }.Contains(emailResendDto.FormType))
            {
                return BadRequest("Invalid form type");
            }

            try
            {
                var result = await _formConfigurationService.ResendEmailAsync(emailResendDto);
                if (result)
                {
                    return Ok(new { success = true, message = "Email sent successfully" });
                }
                else
                {
                    return BadRequest("Failed to send email");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                return StatusCode(500, "An error occurred while sending email");
            }
        }
    }
}