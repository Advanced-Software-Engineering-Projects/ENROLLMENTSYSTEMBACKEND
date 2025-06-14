using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Student")]
    [ApiController]
    [Route("api/student-forms")]
    public class StudentFormsController : ControllerBase
    {
        private readonly StudentFormsServiceClient _formsServiceClient;
        private readonly ILogger<StudentFormsController> _logger;

        public StudentFormsController(
            StudentFormsServiceClient formsServiceClient,
            ILogger<StudentFormsController> logger)
        {
            _formsServiceClient = formsServiceClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetStudentForms([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required");
            }

            try
            {
                var response = await _formsServiceClient.GetStudentFormsAsync(studentId);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Failed to retrieve forms");
                }
                
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving forms for student {studentId}");
                return StatusCode(500, "An error occurred while retrieving forms");
            }
        }

        [HttpGet("eligibility")]
        public async Task<ActionResult> CheckEligibility([FromQuery] string studentId, [FromQuery] string formType)
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
                var response = await _formsServiceClient.CheckEligibilityAsync(studentId, formType);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Failed to check eligibility");
                }
                
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking eligibility for student {studentId} and form type {formType}");
                return StatusCode(500, "An error occurred while checking eligibility");
            }
        }

        [HttpPost("reconsideration")]
        public async Task<ActionResult> SubmitReconsiderationForm([FromBody] ReconsiderationFormDto formDto)
        {
            if (formDto == null)
            {
                return BadRequest("Form data is required");
            }

            try
            {
                var response = await _formsServiceClient.SubmitReconsiderationFormAsync(formDto);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Failed to submit form");
                }
                
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error submitting reconsideration form for student {formDto.StudentId}");
                return StatusCode(500, "An error occurred while submitting the form");
            }
        }

        [HttpPost("compassionate-aegrotat")]
        public async Task<ActionResult> SubmitCompassionateAegrotatForm([FromBody] CompassionateAegrotatFormDto formDto)
        {
            if (formDto == null)
            {
                return BadRequest("Form data is required");
            }

            try
            {
                var response = await _formsServiceClient.SubmitCompassionateAegrotatFormAsync(formDto);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Failed to submit form");
                }
                
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error submitting compassionate/aegrotat form for student {formDto.StudentId}");
                return StatusCode(500, "An error occurred while submitting the form");
            }
        }

        [HttpPost("completion-programme")]
        public async Task<ActionResult> SubmitCompletionProgrammeForm([FromBody] CompletionProgrammeFormDto formDto)
        {
            if (formDto == null)
            {
                return BadRequest("Form data is required");
            }

            try
            {
                var response = await _formsServiceClient.SubmitCompletionProgrammeFormAsync(formDto);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Failed to submit form");
                }
                
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error submitting completion programme form for student {formDto.StudentId}");
                return StatusCode(500, "An error occurred while submitting the form");
            }
        }
    }
}