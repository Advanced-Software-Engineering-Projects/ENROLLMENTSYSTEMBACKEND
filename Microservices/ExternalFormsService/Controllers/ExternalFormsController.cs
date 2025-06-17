using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExternalFormsService.Services;
using ExternalFormsService.DTOs;
using Microsoft.Extensions.Logging;

namespace ExternalFormsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExternalFormsController : ControllerBase
    {
        private readonly IExternalFormService _formService;
        private readonly ILogger<ExternalFormsController> _logger;

        public ExternalFormsController(
            IExternalFormService formService,
            ILogger<ExternalFormsController> logger)
        {
            _formService = formService;
            _logger = logger;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitForm([FromBody] ExternalFormSubmissionDto submission)
        {
            try
            {
                var result = await _formService.SubmitFormAsync(
                    submission.StudentId,
                    submission.FormType,
                    new FormDataDto
                    {
                        FormType = submission.FormType,
                        Fields = submission.FormData,
                        Attachments = submission.Attachments,
                        Status = submission.Status,
                        Comments = submission.Comments
                    });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting form for student: {StudentId}", submission.StudentId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{formId}")]
        public async Task<IActionResult> GetForm(string formId)
        {
            try
            {
                var form = await _formService.GetFormByIdAsync(formId);
                if (form == null)
                {
                    return NotFound();
                }
                return Ok(form);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving form: {FormId}", formId);
                return StatusCode(500, "An error occurred while retrieving the form.");
            }
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentForms(string studentId)
        {
            try
            {
                var forms = await _formService.GetStudentFormsAsync(studentId);
                return Ok(forms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving forms for student: {StudentId}", studentId);
                return StatusCode(500, "An error occurred while retrieving the forms.");
            }
        }

        [HttpPut("{formId}/status")]
        public async Task<IActionResult> UpdateFormStatus(string formId, [FromBody] FormStatusDto statusUpdate)
        {
            try
            {
                var result = await _formService.UpdateFormStatusAsync(
                    formId,
                    statusUpdate.Status,
                    statusUpdate.Comments,
                    statusUpdate.UpdatedBy);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating form status: {FormId}", formId);
                return StatusCode(500, "An error occurred while updating the form status.");
            }
        }

        [HttpDelete("{formId}")]
        public async Task<IActionResult> DeleteForm(string formId)
        {
            try
            {
                var result = await _formService.DeleteFormAsync(formId);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting form: {FormId}", formId);
                return StatusCode(500, "An error occurred while deleting the form.");
            }
        }
    }
} 