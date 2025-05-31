//using ENROLLMENTSYSTEMBACKEND.DTOs;
//using ENROLLMENTSYSTEMBACKEND.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace ENROLLMENTSYSTEMBACKEND.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class DashboardController : ControllerBase
//    {
//        private readonly IDashboardService _context;

//        public DashboardController(IDashboardService context)
//        {
//            _context = context;
//        }

//        // Student Dashboard Endpoints
//        [Authorize(Roles = "Student")]
//        [HttpGet("enrolled-courses")]
//        public async Task<ActionResult<List<EnrolledCourse>>> GetEnrolledCourses()
//        {
//            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            if (string.IsNullOrEmpty(studentId))
//                return Unauthorized("Student ID not found.");

//            var enrolledCourses = await _context.Enrollment
//                .Where(e => e.StudentId == studentId && e.Status == "Enrolled")
//                .Select(e => new EnrolledCourse
//                {
//                    CourseId = e.CourseId,
//                    CourseCode = e.Course.CourseCode,
//                    CourseName = e.Course.CourseName,
//                    DueDate = e.Course.DueDate.ToString("yyyy-MM-dd")
//                })
//                .ToListAsync();

//            return Ok(enrolledCourses);
//        }

//        [Authorize(Roles = "Student")]
//        [HttpGet("completed-courses-current-year")]
//        public async Task<ActionResult<int>> GetCompletedCoursesCurrentYear()
//        {
//            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            if (string.IsNullOrEmpty(studentId))
//                return Unauthorized("Student ID not found.");

//            var currentYear = DateTime.Now.Year;
//            var completedCoursesCount = await _context.Enrollments
//                .Where(e => e.StudentId == studentId && e.Status == "Completed" && e.CompletionDate.Year == currentYear)
//                .CountAsync();

//            return Ok(completedCoursesCount);
//        }

//        [Authorize(Roles = "Student")]
//        [HttpGet("total-completed-courses")]
//        public async Task<ActionResult<int>> GetTotalCompletedCourses()
//        {
//            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            if (string.IsNullOrEmpty(studentId))
//                return Unauthorized("Student ID not found.");

//            var totalCompletedCoursesCount = await _context.Enrollments
//                .Where(e => e.StudentId == studentId && e.Status == "Completed")
//                .CountAsync();

//            return Ok(totalCompletedCoursesCount);
//        }

//        [Authorize(Roles = "Student")]
//        [HttpGet("gpa-data")]
//        public async Task<ActionResult<List<GpaData>>> GetGpaData()
//        {
//            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//            if (string.IsNullOrEmpty(studentId))
//                return Unauthorized("Student ID not found.");

//            // Assuming a method or query to calculate GPA per semester exists
//            var gpaData = await _context.Enrollments
//                .Where(e => e.StudentId == studentId && e.Grade != null)
//                .GroupBy(e => e.Semester)
//                .Select(g => new
//                {
//                    Semester = g.Key,
//                    GPA = Math.Round(g.Average(e => ConvertGradeToGpa(e.Grade)), 2)
//                })
//                .OrderBy(g => g.Semester)
//                .ToListAsync();

//            return Ok(gpaData);
//        }

//        private double ConvertGradeToGpa(string grade)
//        {
//            return grade.ToUpper() switch
//            {
//                "A+" => 4.5,
//                "A" => 4.0,
//                "B+" => 3.5,
//                "B" => 3.0,
//                "C+" => 2.5,
//                "C" => 2.0,
//                "D" => 1.5,
//                "E" => 1.0,
//                "F" => 0.0,
//                _ => 0.0,
//            };
//        }

//        // Admin Dashboard Endpoints

//        [Authorize(Roles = "Admin")]
//        [HttpGet("registered-students-count")]
//        public async Task<ActionResult<int>> GetRegisteredStudentsCount()
//        {
//            var count = await _context.Students.CountAsync();
//            return Ok(count);
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpGet("active-courses-count")]
//        public async Task<ActionResult<int>> GetActiveCoursesCount()
//        {
//            var count = await _context.Courses
//                .Where(c => c.IsActive)
//                .CountAsync();
//            return Ok(count);
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpGet("pending-approvals-count")]
//        public async Task<ActionResult<int>> GetPendingApprovalsCount()
//        {
//            var count = await _context.PendingRequests
//                .CountAsync();
//            return Ok(count);
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpGet("pending-requests")]
//        public async Task<ActionResult<List<PendingRequest>>> GetPendingRequests()
//        {
//            var pendingRequests = await _context.PendingRequests
//                .Select(r => new PendingRequest
//                {
//                    Id = r.Id,
//                    StudentId = r.StudentId,
//                    CourseCode = r.CourseCode,
//                    RequestType = r.RequestType,
//                    Date = r.Date.ToString("yyyy-MM-dd")
//                })
//                .ToListAsync();
//            return Ok(pendingRequests);
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpGet("enrollment-data")]
//        public async Task<ActionResult<List<EnrollmentData>>> GetEnrollmentData()
//        {
//            var enrollmentData = await _context.Enrollments
//                .GroupBy(e => e.Semester)
//                .Select(g => new EnrollmentData
//                {
//                    Semester = g.Key,
//                    Students = g.Select(e => e.StudentId).Distinct().Count()
//                })
//                .ToListAsync();
//            return Ok(enrollmentData);
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpGet("completion-rate-data")]
//        public async Task<ActionResult<List<CompletionRateData>>> GetCompletionRateData()
//        {
//            var completionRateData = await _context.Enrollments
//                .GroupBy(e => e.Semester)
//                .Select(g => new CompletionRateData
//                {
//                    Semester = g.Key,
//                    Rate = (int)((double)g.Count(e => e.Status == "Completed") / g.Count() * 100)
//                })
//                .ToListAsync();
//            return Ok(completionRateData);
//        }
//    }
//}

using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // Student Dashboard Endpoints
        [Authorize(Roles = "Student")]
        [HttpGet("enrolled-courses")]
        public async Task<ActionResult<List<EnrolledCourse>>> GetEnrolledCourses()
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized("Student ID not found.");

            var enrolledCourses = await _dashboardService.GetEnrolledCoursesAsync(studentId);
            return Ok(enrolledCourses);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("completed-courses-current-year")]
        public async Task<ActionResult<int>> GetCompletedCoursesCurrentYear()
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized("Student ID not found.");

            var completedCoursesCount = await _dashboardService.GetCompletedCoursesCurrentYearAsync(studentId);
            return Ok(completedCoursesCount);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("total-completed-courses")]
        public async Task<ActionResult<int>> GetTotalCompletedCourses()
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized("Student ID not found.");

            var totalCompletedCoursesCount = await _dashboardService.GetTotalCompletedCoursesAsync(studentId);
            return Ok(totalCompletedCoursesCount);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("gpa-data")]
        public async Task<ActionResult<List<GpaData>>> GetGpaData()
        {
            var studentId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized("Student ID not found.");

            var gpaData = await _dashboardService.GetGpaDataAsync(studentId);
            return Ok(gpaData);
        }

        // Admin Dashboard Endpoints
        [Authorize(Roles = "Admin")]
        [HttpGet("registered-students-count")]
        public async Task<ActionResult<int>> GetRegisteredStudentsCount()
        {
            var count = await _dashboardService.GetRegisteredStudentsCountAsync();
            return Ok(count);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("active-courses-count")]
        public async Task<ActionResult<int>> GetActiveCoursesCount()
        {
            var count = await _dashboardService.GetActiveCoursesCountAsync();
            return Ok(count);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pending-approvals-count")]
        public async Task<ActionResult<int>> GetPendingApprovalsCount()
        {
            var count = await _dashboardService.GetPendingApprovalsCountAsync();
            return Ok(count);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pending-requests")]
        public async Task<ActionResult<List<PendingRequest>>> GetPendingRequests()
        {
            var pendingRequests = await _dashboardService.GetPendingRequestsAsync();
            return Ok(pendingRequests);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("enrollment-data")]
        public async Task<ActionResult<List<EnrollmentData>>> GetEnrollmentData()
        {
            var enrollmentData = await _dashboardService.GetEnrollmentDataAsync();
            return Ok(enrollmentData);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("completion-rate-data")]
        public async Task<ActionResult<List<CompletionRateData>>> GetCompletionRateData()
        {
            var completionRateData = await _dashboardService.GetCompletionRateDataAsync();
            return Ok(completionRateData);
        }
    }
}