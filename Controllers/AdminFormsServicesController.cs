using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin-forms-services")]
    public class AdminFormsServicesController : ControllerBase
    {
        private readonly IFormService _formService;
        private readonly IGradeService _gradeService;
        private readonly IEmailService _emailService;

        public AdminFormsServicesController(
            IFormService formService,
            IGradeService gradeService,
            IEmailService emailService)
        {
            _formService = formService;
            _gradeService = gradeService;
            _emailService = emailService;
        }


        //Gets all form submissions, optionally filtered by student ID and form type.
        [HttpGet]
        public async Task<IActionResult> GetAllForms([FromQuery] string? studentId, [FromQuery] string? formType)
        {
            try
            {
                var forms = await _formService.GetFormsAsync(studentId, formType);
                return Ok(forms);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No forms found"
            }
        }

   
        //Updates the grade for a reconsideration form submission.
        [HttpPut("update-grade")]
        public async Task<IActionResult> UpdateGrade([FromBody] UpdateGradeDto updateGradeDto)
        {
            if (updateGradeDto == null || string.IsNullOrEmpty(updateGradeDto.SubmissionId) || string.IsNullOrEmpty(updateGradeDto.NewGrade))
            {
                return BadRequest("Submission ID and new grade are required.");
            }

            try
            {
                var result = await _gradeService.UpdateGradeAsync(updateGradeDto);
                if (result == null)
                {
                    return NotFound("Submission not found.");
                }
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Invalid grade"
            }
        }

      
        //Updates the status for a compassionate/aegrotat form submission.
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusDto updateStatusDto)
        {
            if (updateStatusDto == null || string.IsNullOrEmpty(updateStatusDto.SubmissionId) || string.IsNullOrEmpty(updateStatusDto.Status))
            {
                return BadRequest("Submission ID and status are required.");
            }

            try
            {
                var result = await _formService.UpdateFormStatusAsync(updateStatusDto);
                if (result == null)
                {
                    return NotFound("Submission not found.");
                }
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Invalid status"
            }
        }

        //Gets grades for a student (not used in AdminFormsServices but preserved).
        [HttpGet("grades")]
        public async Task<IActionResult> GetGrades([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var grades = await _gradeService.GetGradesByStudentIdAsync(studentId);
                return Ok(grades);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No grades found"
            }
        }

        //Gets a specific form submission by ID.
        [HttpGet("forms/{formId}")]
        public async Task<IActionResult> GetFormById(string formId)
        {
            if (string.IsNullOrEmpty(formId))
            {
                return BadRequest("Form ID is required.");
            }

            try
            {
                var form = await _formService.GetFormByIdAsync(formId);
                if (form == null)
                {
                    return NotFound("Form not found.");
                }
                return Ok(form);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        //Creates a new form submission (not used in AdminFormsServices but preserved).
        [HttpPost("forms")]
        public async Task<IActionResult> CreateForm([FromBody] CreateFormDto createFormDto)
        {
            if (createFormDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid form data.");
            }

            try
            {
                var form = await _formService.CreateFormAsync(createFormDto);
                return CreatedAtAction(nameof(GetFormById), new { formId = form.SubmissionId }, form);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Failed to create form"
            }
        }

        
        //Resends a notification email for a form submission.
        [HttpPost("resend-email")]
        public async Task<IActionResult> ResendEmail([FromBody] ResendEmailDto resendEmailDto)
        {
            if (resendEmailDto == null || string.IsNullOrEmpty(resendEmailDto.SubmissionId) ||
                string.IsNullOrEmpty(resendEmailDto.FormType) || string.IsNullOrEmpty(resendEmailDto.StudentEmail))
            {
                return BadRequest("Submission ID, form type, and student email are required.");
            }

            try
            {
                await _emailService.SendFormNotificationAsync(resendEmailDto.SubmissionId, resendEmailDto.FormType, resendEmailDto.StudentEmail);
                return Ok(new { Message = "Email sent successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Failed to send email"
            }
        }
    }
}