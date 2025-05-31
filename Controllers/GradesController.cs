using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Student")]
    [ApiController]
    [Route("api/grades")]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        //Retrieves academic records for a student, including student details and enrollments.
        [HttpGet("academic-records")]
        public async Task<IActionResult> GetAcademicRecords([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var academicRecords = await _gradeService.GetAcademicRecordsAsync(studentId);
                return Ok(academicRecords);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "Student not found"
            }
        }

       
        //Retrieves GPA trend data for a student across semesters.
        [HttpGet("gpa-trend")]
        public async Task<IActionResult> GetGpaTrend([FromQuery] string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var gpaTrend = await _gradeService.GetGpaTrendAsync(studentId);
                return Ok(gpaTrend);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No GPA data available"
            }
        }
    }
}