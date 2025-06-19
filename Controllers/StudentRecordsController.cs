using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [ApiController]
    [Route("api/student-records")]
    [Authorize(Roles = "Admin")]
    public class StudentRecordsController : ControllerBase
    {
        private readonly IStudentRecordService _studentRecordService;
        private readonly ILogger<StudentRecordsController> _logger;

        public StudentRecordsController(
            IStudentRecordService studentRecordService,
            ILogger<StudentRecordsController> logger)
        {
            _studentRecordService = studentRecordService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentRecords([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 5;
                if (pageSize > 50) pageSize = 50;

                var records = await _studentRecordService.GetStudentRecordsAsync(page, pageSize);
                return Ok(records);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving student records");
                return StatusCode(500, "An error occurred while retrieving student records");
            }
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentById(string studentId)
        {
            try
            {
                var student = await _studentRecordService.GetStudentByIdAsync(studentId);
                if (student == null)
                {
                    return NotFound($"Student with ID {studentId} not found");
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving student");
                return StatusCode(500, "An error occurred while retrieving student");
            }
        }
    }
}