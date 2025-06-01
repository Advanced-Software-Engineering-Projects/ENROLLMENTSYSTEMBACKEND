using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GradeRecheckMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GradeRecheckController : ControllerBase
    {
        private static readonly Dictionary<string, string> _applications = new Dictionary<string, string>();

        [HttpPost("apply")]
        public IActionResult ApplyGradeRecheck([FromBody] GradeRecheckRequest request)
        {
            if (string.IsNullOrEmpty(request.StudentId) || string.IsNullOrEmpty(request.CourseId))
            {
                return BadRequest("StudentId and CourseId are required.");
            }

            var key = $"{request.StudentId}_{request.CourseId}";
            _applications[key] = "Pending";

            // TODO: Add notification logic here

            return Ok(new { Message = "Grade recheck application submitted.", Status = "Pending" });
        }

        [HttpGet("status/{studentId}/{courseId}")]
        public IActionResult GetApplicationStatus(string studentId, string courseId)
        {
            var key = $"{studentId}_{courseId}";
            if (_applications.TryGetValue(key, out var status))
            {
                return Ok(new { StudentId = studentId, CourseId = courseId, Status = status });
            }
            return NotFound("Application not found.");
        }
    }

    public class GradeRecheckRequest
    {
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public string Reason { get; set; }
    }
}
