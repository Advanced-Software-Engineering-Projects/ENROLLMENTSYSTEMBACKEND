using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ExternalFormIntegrationServiceClient _externalFormClient;

        public FormsController(IFormService formService, ExternalFormIntegrationServiceClient externalFormClient)
        {
            _formService = formService;
            _externalFormClient = externalFormClient;
        }

        //Gets all form submissions for a student with optional filtering by form type.
        [HttpGet]
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
        [HttpPost("external/apply")]
        public async Task<IActionResult> ApplyForExternalForm([FromBody] FormApplicationDto application)
        {
            var response = await _externalFormClient.ApplyForFormAsync(application.StudentId, application.FormType);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to apply for external form");
            }
            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
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
    }
}
