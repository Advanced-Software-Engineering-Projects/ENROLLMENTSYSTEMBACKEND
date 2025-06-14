using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ENROLLMENTSYSTEMBACKEND.Services;
using ENROLLMENTSYSTEMBACKEND.DTOs;
using System.Security.Claims;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GradeRecheckController : ControllerBase
    {
        private readonly IGradeRecheckService _gradeRecheckService;
        private readonly IHoldService _holdService;

        public GradeRecheckController(
            IGradeRecheckService gradeRecheckService,
            IHoldService holdService)
        {
            _gradeRecheckService = gradeRecheckService;
            _holdService = holdService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> CreateRequest([FromBody] GradeRecheckRequestDto request)
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized();

            if (!await _holdService.CanAccessServiceAsync(studentId, "grade_recheck"))
                return Forbid("Student is on hold and cannot request grade rechecks");

            var result = await _gradeRecheckService.CreateRecheckRequestAsync(
                studentId,
                request.CourseId,
                request.Reason);

            return Ok(result);
        }

        [HttpGet("student/requests")]
        public async Task<IActionResult> GetStudentRequests()
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized();

            var requests = await _gradeRecheckService.GetStudentRecheckRequestsAsync(studentId);
            return Ok(requests);
        }

        [HttpGet("student/notifications")]
        public async Task<IActionResult> GetStudentNotifications()
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized();

            var notifications = await _gradeRecheckService.GetStudentGradeNotificationsAsync(studentId);
            return Ok(notifications);
        }

        [HttpPost("notifications/{notificationId}/read")]
        public async Task<IActionResult> MarkNotificationAsRead(string notificationId)
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized();

            await _gradeRecheckService.MarkNotificationAsReadAsync(notificationId);
            return Ok();
        }

        [HttpGet("admin/pending")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingRequests()
        {
            var requests = await _gradeRecheckService.GetAllPendingRecheckRequestsAsync();
            return Ok(requests);
        }

        [HttpPut("admin/request/{requestId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRequestStatus(
            string requestId,
            [FromBody] string status)
        {
            var result = await _gradeRecheckService.UpdateRecheckRequestStatusAsync(requestId, status);
            return Ok(result);
        }
    }
}