using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Student")]
    [ApiController]
    [Route("api/timetables")]
    public class TimetablesController : ControllerBase
    {
        private readonly ITimetableService _timetableService;

        public TimetablesController(ITimetableService timetableService)
        {
            _timetableService = timetableService;
        }

        // Retrieves timetable entries for a student.
        [HttpGet]
        public async Task<IActionResult> GetTimetables([FromQuery] string studentId, [FromQuery] string semester)
        {
            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(semester))
            {
                return BadRequest("Student ID and semester are required.");
            }

            try
            {
                var timetables = await _timetableService.GetTimetablesByStudentIdAsync(studentId, semester);
                return Ok(timetables);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        //Adds a new timetable entry for a student.
        [HttpPost]
        public async Task<IActionResult> AddTimetable([FromBody] TimetableDto timetableDto)
        {
            if (timetableDto == null || string.IsNullOrEmpty(timetableDto.StudentId) ||
                string.IsNullOrEmpty(timetableDto.CourseCode) || string.IsNullOrEmpty(timetableDto.Semester))
            {
                return BadRequest("Student ID, course code, and semester are required.");
            }

            // Validate time format (HH:MM)
            if (!IsValidTimeFormat(timetableDto.StartTime) || !IsValidTimeFormat(timetableDto.EndTime))
            {
                return BadRequest("Invalid time format. Use HH:MM (e.g., 09:00).");
            }

            // Validate date format (YYYY-MM-DD)
            if (!IsValidDateFormat(timetableDto.Date))
            {
                return BadRequest("Invalid date format. Use YYYY-MM-DD (e.g., 2025-03-03).");
            }

            try
            {
                var timetable = await _timetableService.AddTimetableAsync(timetableDto);
                return CreatedAtAction(nameof(GetTimetables),
                    new { studentId = timetableDto.StudentId, semester = timetableDto.Semester },
                    timetable);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Invalid course" or "Scheduling conflict"
            }
        }

        // Helper method to validate time format (HH:MM)
        private bool IsValidTimeFormat(string time)
        {
            if (string.IsNullOrEmpty(time))
                return false;
            var regex = new System.Text.RegularExpressions.Regex(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$");
            return regex.IsMatch(time);
        }

        // Helper method to validate date format (YYYY-MM-DD)
        private bool IsValidDateFormat(string date)
        {
            if (string.IsNullOrEmpty(date))
                return false;
            var regex = new System.Text.RegularExpressions.Regex(@"^\d{4}-\d{2}-\d{2}$");
            return regex.IsMatch(date) && DateTime.TryParse(date, out _);
        }
    }
}