using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/student-records")]
    public class StudentRecordsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IGradeService _gradeService;
        private readonly TranscriptPdfService _transcriptPdfService;

        public StudentRecordsController(IStudentService studentService, IGradeService gradeService, TranscriptPdfService transcriptPdfService)
        {
            _studentService = studentService;
            _gradeService = gradeService;
            _transcriptPdfService = transcriptPdfService;
        }

        //Gets a specific student by their ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    return NotFound("Student not found.");
                }
                return Ok(student);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }


        /// Gets all student records with pagination.
        [HttpGet]
        public async Task<IActionResult> GetAllStudents([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and page size must be positive integers.");
            }

            try
            {
                var students = await _studentService.GetAllStudentsAsync(page, pageSize);
                var totalStudents = await _studentService.GetTotalStudentsCountAsync();
                var totalPages = (int)Math.Ceiling((double)totalStudents / pageSize);

                return Ok(new
                {
                    Students = students,
                    TotalStudents = totalStudents,
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = pageSize
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No students found"
            }
        }

        [HttpGet("{id}/transcript")]
        public async Task<IActionResult> GetTranscriptPdf(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Student ID is required.");
            }

            try
            {
                var transcript = await _gradeService.GetTranscriptAsync(id);
                if (transcript == null || transcript.Enrollments == null || transcript.Enrollments.Count == 0)
                {
                    return NotFound("Transcript not found or no enrollments available.");
                }

                // Calculate GPA for the transcript
                transcript.GPA = transcript.Enrollments.Any() ? transcript.Enrollments.Average(e => GetGradePoint(e.Grade)) : 0.0;

                var pdfBytes = _transcriptPdfService.GenerateTranscriptPdf(transcript);
                return File(pdfBytes, "application/pdf", $"Transcript_{id}.pdf");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        private double GetGradePoint(string grade)
        {
            return grade switch
            {
                "A" => 4.0,
                "B" => 3.0,
                "C" => 2.0,
                "D" => 1.0,
                "F" => 0.0,
                _ => 0.0
            };
        }
    }
}
