using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using GradeRecheckMicroservice.Services;

namespace GradeRecheckMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GradeRecheckController : ControllerBase
    {
        private static readonly Dictionary<string, string> _applications = new Dictionary<string, string>();
        private readonly NotificationService _notificationService;

        public GradeRecheckController()
        {
            _notificationService = new NotificationService();
        }

        [HttpPost("apply")]
        public IActionResult ApplyGradeRecheck([FromBody] GradeRecheckRequest request)
        {
            if (string.IsNullOrEmpty(request.StudentId) || string.IsNullOrEmpty(request.CourseId))
            {
                return BadRequest("StudentId and CourseId are required.");
            }

            var key = $"{request.StudentId}_{request.CourseId}";
            _applications[key] = "Pending";

            // Send notification on application
            _notificationService.NotifyGradeChange(request.StudentId, request.CourseId, "Grade recheck application submitted.");

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

        // Endpoint to notify grade change
        [HttpPost("notifyGradeChange")]
        public IActionResult NotifyGradeChange([FromBody] GradeChangeNotification notification)
        {
            if (string.IsNullOrEmpty(notification.StudentId) || string.IsNullOrEmpty(notification.CourseId))
            {
                return BadRequest("StudentId and CourseId are required.");
            }

            // Update application status if exists
            var key = $"{notification.StudentId}_{notification.CourseId}";
            if (_applications.ContainsKey(key))
            {
                _applications[key] = notification.NewStatus;
            }

            // Send notification
            _notificationService.NotifyGradeChange(notification.StudentId, notification.CourseId, $"Grade status updated to {notification.NewStatus}.");

            return Ok(new { Message = "Notification sent." });
        }
    }

    public class GradeRecheckRequest
    {
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public string Reason { get; set; }
    }

    public class GradeChangeNotification
    {
        public string StudentId { get; set; }
        public string CourseId { get; set; }
        public string NewStatus { get; set; }
    }
}
