using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AcademicTranscriptService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TranscriptController : ControllerBase
    {
        private const string TranscriptDataFile = "transcripts.json";

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetTranscript(string studentId)
        {
            if (!System.IO.File.Exists(TranscriptDataFile))
            {
                return NotFound("Transcript data not found.");
            }

            var json = await System.IO.File.ReadAllTextAsync(TranscriptDataFile);
            var transcripts = JsonSerializer.Deserialize<List<Transcript>>(json);

            var studentTranscript = transcripts?.Find(t => t.StudentId == studentId);
            if (studentTranscript == null)
            {
                return NotFound("Transcript not found for the student.");
            }

            // For simplicity, return JSON. PDF generation can be added here.
            return Ok(studentTranscript);
        }
    }

    public class Transcript
    {
        public string StudentId { get; set; }
        public List<CourseGrade> CourseGrades { get; set; }
    }

    public class CourseGrade
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string Grade { get; set; }
        public string Semester { get; set; }
    }
}
