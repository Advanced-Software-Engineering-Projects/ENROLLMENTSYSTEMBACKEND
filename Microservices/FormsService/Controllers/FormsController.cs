using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Microservices.FormsService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Microservices.FormsService.Controllers
{
    [ApiController]
    [Route("api/microservices/forms")]
    public class FormsController : ControllerBase
    {
        private readonly IStudentFormsService _studentFormsService;
        private readonly ILogger<FormsController> _logger;

        public FormsController(
            IStudentFormsService studentFormsService,
            ILogger<FormsController> logger)
        {
            _studentFormsService = studentFormsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<FormSubmissionsResponseDto>> GetStudentForms([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required");
            }

            try
            {
                var forms = await _studentFormsService.GetStudentFormsAsync(studentId);
                return Ok(forms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving forms for student {studentId}");
                return StatusCode(500, "An error occurred while retrieving forms");
            }
        }

        [HttpGet("eligibility")]
        public async Task<ActionResult<bool>> CheckEligibility([FromQuery] string studentId, [FromQuery] string formType)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required");
            }

            if (string.IsNullOrEmpty(formType))
            {
                return BadRequest("Form type is required");
            }

            try
            {
                var isEligible = await _studentFormsService.CheckEligibilityAsync(studentId, formType);
                return Ok(isEligible);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking eligibility for student {studentId} and form type {formType}");
                return StatusCode(500, "An error occurred while checking eligibility");
            }
        }

        [HttpPost("reconsideration")]
        public async Task<ActionResult<FormSubmissionDto>> SubmitReconsiderationForm([FromBody] ReconsiderationFormDto formDto)
        {
            if (formDto == null)
            {
                return BadRequest("Form data is required");
            }

            try
            {
                var result = await _studentFormsService.SubmitReconsiderationFormAsync(formDto);
                return CreatedAtAction(nameof(GetStudentForms), new { studentId = formDto.StudentId }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Invalid reconsideration form submission for student {formDto.StudentId}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error submitting reconsideration form for student {formDto.StudentId}");
                return StatusCode(500, "An error occurred while submitting the form");
            }
        }

        [HttpPost("compassionate-aegrotat")]
        public async Task<ActionResult<FormSubmissionDto>> SubmitCompassionateAegrotatForm([FromBody] CompassionateAegrotatFormDto formDto)
        {
            if (formDto == null)
            {
                return BadRequest("Form data is required");
            }

            try
            {
                var result = await _studentFormsService.SubmitCompassionateAegrotatFormAsync(formDto);
                return CreatedAtAction(nameof(GetStudentForms), new { studentId = formDto.StudentId }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Invalid compassionate/aegrotat form submission for student {formDto.StudentId}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error submitting compassionate/aegrotat form for student {formDto.StudentId}");
                return StatusCode(500, "An error occurred while submitting the form");
            }
        }

        [HttpPost("completion-programme")]
        public async Task<ActionResult<FormSubmissionDto>> SubmitCompletionProgrammeForm([FromBody] CompletionProgrammeFormDto formDto)
        {
            if (formDto == null)
            {
                return BadRequest("Form data is required");
            }

            try
            {
                var result = await _studentFormsService.SubmitCompletionProgrammeFormAsync(formDto);
                return CreatedAtAction(nameof(GetStudentForms), new { studentId = formDto.StudentId }, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, $"Invalid completion programme form submission for student {formDto.StudentId}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error submitting completion programme form for student {formDto.StudentId}");
                return StatusCode(500, "An error occurred while submitting the form");
            }
        }
    }
}