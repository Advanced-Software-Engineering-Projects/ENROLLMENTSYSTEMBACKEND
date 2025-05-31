using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Student")]
    [ApiController]
    [Route("api/academic-records")]
    public class AcademicRecordsController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public AcademicRecordsController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        
        //Gets a student's academic records, including enrollments and GPA.
        [HttpGet]
        public async Task<IActionResult> GetAcademicRecords([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var records = await _gradeService.GetAcademicRecordsAsync(studentId);
                if (records == null)
                {
                    return NotFound("Academic records not found.");
                }
                return Ok(records);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        //Gets a student's transcript.
        [HttpGet("transcript")]
        public async Task<IActionResult> GetTranscript([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var transcript = await _gradeService.GetTranscriptAsync(studentId);
                if (transcript == null)
                {
                    return NotFound("Transcript not found.");
                }
                return Ok(transcript);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

      
        //Gets a student's GPA.
        [HttpGet("gpa")]
        public async Task<IActionResult> GetGPA([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var gpa = await _gradeService.CalculateGPAAsync(studentId);
                if (gpa < 0)
                {
                    return NotFound("GPA not found.");
                }
                return Ok(new { GPA = gpa });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }


        //Gets a student's program audit.

        [HttpGet("audit")]
        public async Task<IActionResult> GetProgramAudit([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var audit = await _gradeService.GetProgramAuditAsync(studentId);
                if (audit == null)
                {
                    return NotFound("Program audit not found.");
                }
                return Ok(audit);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

      
        //Gets a student's enrollment status.
        [HttpGet("enrollment-status")]
        public async Task<IActionResult> GetEnrollmentStatus([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var status = await _gradeService.GetEnrollmentStatusAsync(studentId);
                if (status == null)
                {
                    return NotFound("Enrollment status not found.");
                }
                return Ok(status);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }


        //Checks a student's graduation eligibility.
        [HttpGet("graduation-eligibility")]
        public async Task<IActionResult> CheckGraduationEligibility([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var isEligible = await _gradeService.CheckGraduationEligibilityAsync(studentId);
                return Ok(new { IsEligible = isEligible });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        
        //Applies for graduation on behalf of a student.
        [HttpPost("graduation-application")]
        public async Task<IActionResult> ApplyForGraduation([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var result = await _gradeService.ApplyForGraduationAsync(studentId);
                if (result == null)
                {
                    return NotFound("Graduation application not found.");
                }
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        //Gets a student's graduation application status.
        [HttpGet("graduation-status")]
        public async Task<IActionResult> GetGraduationStatus([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var status = await _gradeService.GetGraduationStatusAsync(studentId);
                if (status == null)
                {
                    return NotFound("Graduation status not found.");
                }
                return Ok(status);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}