using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Student")]
    [ApiController]
    [Route("api/forms")]
    public class FormsController : ControllerBase
    {
        private readonly IFormService _formService;
        private readonly IAutoFillService _autoFillService;
        private readonly ExternalFormIntegrationServiceClient _externalFormClient;
        private readonly GradeRecheckServiceClient _gradeRecheckServiceClient;
        private readonly ILogger<FormsController> _logger;

        public FormsController(
            IFormService formService,
            IAutoFillService autoFillService,
            ExternalFormIntegrationServiceClient externalFormClient,
            GradeRecheckServiceClient gradeRecheckServiceClient,
            ILogger<FormsController> logger)
        {
            _formService = formService;
            _autoFillService = autoFillService;
            _externalFormClient = externalFormClient;
            _gradeRecheckServiceClient = gradeRecheckServiceClient;
            _logger = logger;
        }

        //Gets all form submissions for a student with optional filtering by form type.
        [HttpGet("list")]
        public async Task<ActionResult<List<FormSubmissionDto>>> GetForms(string? studentId, string? formType)
        {
            var forms = await _formService.GetFormsAsync(studentId, formType);
            return Ok(forms);
        }

        // New endpoint to get external forms from microservice
        [HttpGet("external")]
        public async Task<IActionResult> GetExternalForms()
        {
            var response = await _externalFormClient.GetFormsAsync();
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to get external forms");
            }
            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        // New endpoint to apply for external form
        [HttpPost("apply")]
        public async Task<IActionResult> ApplyForForm([FromBody] FormApplicationDto application)
        {
            try
            {
                var formData = new FormDataDto
                {
                    FormType = application.FormType,
                    Fields = new Dictionary<string, string>
                    {
                        { "StudentId", application.StudentId },
                        { "FormType", application.FormType }
                    },
                    Attachments = new List<string>(),
                    Status = "Pending",
                    Comments = "Form submitted"
                };

                var result = await _externalFormClient.ApplyForFormAsync(
                    application.StudentId,
                    application.FormType,
                    formData
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying for form");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        //Gets all form submissions for a student.
        [HttpGet("all")]
        public async Task<IActionResult> GetForms([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var forms = await _formService.GetFormsAsync(studentId);
                return Ok(forms);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No forms found"
            }
        }

       
        //Submits a form (Reconsideration or Compassionate/Aegrotat).
        [HttpPost]
        public async Task<IActionResult> SubmitForm([FromForm] FormSubmissionDto formDto)
        {
            if (formDto == null)
            {
                return BadRequest("Form data is required.");
            }
            if (string.IsNullOrEmpty(formDto.StudentId))
            {
                return BadRequest("Student ID is required.");
            }
            if (string.IsNullOrEmpty(formDto.FormType))
            {
                return BadRequest("Form type is required.");
            }

            try
            {
                var submittedForm = await _formService.SubmitFormAsync(formDto);
                return CreatedAtAction(nameof(GetForms), new { studentId = formDto.StudentId }, submittedForm);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Invalid form data"
            }
        }

        // New endpoint to apply for grade recheck
        [HttpPost("graderecheck/apply")]
        public async Task<IActionResult> ApplyGradeRecheck([FromBody] GradeRecheckApplicationDto application)
        {
            if (application == null)
            {
                return BadRequest("Application data is required.");
            }
            if (string.IsNullOrEmpty(application.StudentId))
            {
                return BadRequest("Student ID is required.");
            }
            if (string.IsNullOrEmpty(application.CourseId))
            {
                return BadRequest("Course ID is required.");
            }
            if (string.IsNullOrEmpty(application.Reason))
            {
                return BadRequest("Reason is required.");
            }

            var response = await _gradeRecheckServiceClient.ApplyGradeRecheckAsync(application.StudentId, application.CourseId, application.Reason);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to apply for grade recheck");
            }
            return Ok("Grade recheck application submitted successfully.");
        }

        [HttpPost("upload-avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAvatar([FromForm] UploadAvatarRequest request)
        {
            var avatar = request.Avatar;
            if (avatar == null || avatar.Length == 0)
            {
                return BadRequest("Avatar file is required.");
            }

            try
            {
                using (var stream = avatar.OpenReadStream())
                {
                    await Task.Delay(100); // Simulate async work
                }

                return Ok("Avatar uploaded successfully.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("auto-fill/{formType}")]
        public async Task<ActionResult<FormAutoFillDataDto>> GetAutoFillData(string formType)
        {
            try
            {
                var studentId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(studentId))
                {
                    return Unauthorized("Student ID not found in token");
                }

                var autoFillData = await _autoFillService.GetAutoFillDataAsync(studentId, formType);
                return Ok(autoFillData);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting auto-fill data for form type {FormType}", formType);
                return StatusCode(500, "An error occurred while getting auto-fill data");
            }
        }

        [HttpGet("field-mappings/{formType}")]
        public async Task<ActionResult<Dictionary<string, string>>> GetFormFieldMappings(string formType)
        {
            try
            {
                var mappings = await _autoFillService.GetFormFieldMappingsAsync(formType);
                return Ok(mappings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting field mappings for form type {FormType}", formType);
                return StatusCode(500, "An error occurred while getting field mappings");
            }
        }

        [HttpPost("validate-auto-fill")]
        public async Task<ActionResult<bool>> ValidateAutoFillData([FromBody] FormAutoFillDataDto data)
        {
            try
            {
                var studentId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(studentId))
                {
                    return Unauthorized("Student ID not found in token");
                }

                var isValid = await _autoFillService.ValidateAutoFillDataAsync(studentId, data);
                return Ok(isValid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating auto-fill data");
                return StatusCode(500, "An error occurred while validating auto-fill data");
            }
        }
    }
}
