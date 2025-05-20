using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.IServices;
using ENROLLMENTSYSTEMBACKEND.Services;

namespace USPENROLLMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IConfiguration _configuration;
        private readonly IUserActivityService _userActivityService;

        public StudentController(
            IStudentService studentService,
            IConfiguration configuration,
            IUserActivityService userActivityService)
        {
            _studentService = studentService;
            _configuration = configuration;
            _userActivityService = userActivityService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var studentDto = await _studentService.AuthenticateStudentAsync(request.StudentId, request.Password);
                if (studentDto == null)
                    return Unauthorized(new { message = "Invalid credentials" });

                var token = GenerateJwtToken(studentDto);
                return Ok(new { Token = token, Student = studentDto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message} - StackTrace: {ex.StackTrace}");
            }
        }

        private string GenerateJwtToken(StudentDto student)
        {
            var keyString = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString) || keyString.Length < 16)
                throw new ArgumentException($"JWT key is invalid. Length: {keyString?.Length ?? 0}, Required: 16+ characters.");

            var keyBytes = Encoding.UTF8.GetBytes(keyString);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("studentId", student.StudentId), // Updated to "studentId"
                new Claim(ClaimTypes.Role, "STUDENT")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("logout")]
        [Authorize(Policy = "StudentOnly")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var studentId = User.FindFirst("studentId")?.Value;
                if (string.IsNullOrEmpty(studentId))
                {
                    return Unauthorized("Invalid token");
                }
                await _userActivityService.LogActivityAsync(studentId, "student", "logout", new { });
                return Ok("Logged out successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpPut("{studentId}/profile")]
        public async Task<IActionResult> UpdateProfile(string studentId, [FromBody] StudentDto studentDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only update your own profile");
                }
                var result = await _studentService.UpdateProfileAsync(studentId, studentDto);
                await _userActivityService.LogActivityAsync(studentId, "student", "update_profile", studentDto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/courses/{semester}")]
        public async Task<IActionResult> GetAvailableCourses(string studentId, string semester)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only view your own courses");
                }
                var courses = await _studentService.GetAvailableCoursesAsync(studentId, semester);
                await _userActivityService.LogActivityAsync(studentId, "student", "view_available_courses", new { semester });
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/courses/search")]
        public async Task<IActionResult> SearchCourses(
            string studentId,
            [FromQuery] string? keyword,
            [FromQuery] string? semester,
            [FromQuery] int? creditsMin,
            [FromQuery] int? creditsMax)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only search your own courses");
                }
                var courses = await _studentService.SearchCoursesAsync(studentId, keyword, semester, creditsMin, creditsMax);
                await _userActivityService.LogActivityAsync(studentId, "student", "search_courses", new { keyword, semester, creditsMin, creditsMax });
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpPost("{studentId}/courses/register")]
        public async Task<IActionResult> RegisterCourse(string studentId, [FromBody] CourseRegistrationRequest request)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only register courses for yourself");
                }
                var success = await _studentService.RegisterCourseAsync(studentId, request.CourseId, request.Semester);
                return success ? Ok("Course registered successfully") : BadRequest("Course registration failed due to unmet prerequisites, credit limits, or fee holds");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] StudentDto studentDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var student = await _studentService.RegisterAsync(studentDto);
                await _userActivityService.LogActivityAsync(student.StudentId, "student", "register", studentDto);
                return Ok(student);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpDelete("{studentId}/courses/{enrollmentId}")]
        public async Task<IActionResult> DropCourse(string studentId, int enrollmentId)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only drop your own courses");
                }
                var success = await _studentService.DropCourseAsync(studentId, enrollmentId);
                return success ? Ok("Course dropped successfully") : BadRequest("Course drop failed");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/timetable/{semester}")]
        public async Task<IActionResult> GetTimetable(string studentId, string semester)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only view your own timetable");
                }
                var timetable = await _studentService.GetTimetableAsync(studentId, semester);
                await _userActivityService.LogActivityAsync(studentId, "student", "view_timetable", new { semester });
                return Ok(timetable);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/grades")]
        public async Task<IActionResult> GetAcademicRecords(string studentId)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only view your own grades");
                }
                var records = await _studentService.GetAcademicRecordsAsync(studentId);
                await _userActivityService.LogActivityAsync(studentId, "student", "view_grades", new { });
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/fees")]
        public async Task<IActionResult> GetFees(string studentId)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only view your own fees");
                }
                var fees = await _studentService.GetFeeInformationAsync(studentId);
                await _userActivityService.LogActivityAsync(studentId, "student", "view_fees", new { });
                return Ok(fees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpPost("{studentId}/fees/{feeId}/pay")]
        public async Task<IActionResult> MarkFeeAsPaid(string studentId, int feeId)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only pay your own fees");
                }

                var success = await _studentService.MarkFeeAsPaidAsync(studentId, feeId);
                if (!success)
                {
                    return NotFound("Fee not found or already paid");
                }

                await _userActivityService.LogActivityAsync(studentId, "student", "pay_fee", new { feeId });
                return Ok("Fee marked as paid successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/fee-holds")]
        public async Task<IActionResult> CheckFeeHolds(string studentId)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only check your own fee holds");
                }
                var hasHolds = await _studentService.HasFeeHoldsAsync(studentId);
                await _userActivityService.LogActivityAsync(studentId, "student", "check_fee_holds", new { hasHolds });
                return Ok(new { HasFeeHolds = hasHolds });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/degree-progress")]
        public async Task<IActionResult> GetDegreeProgress(string studentId)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only view your own degree progress");
                }
                var progress = await _studentService.GetDegreeProgressAsync(studentId);
                await _userActivityService.LogActivityAsync(studentId, "student", "view_degree_progress", new { });
                return Ok(progress);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/dashboard")]
        public async Task<IActionResult> GetDashboardData(string studentId, [FromQuery] string semester)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only view your own dashboard");
                }
                var currentlyEnrolled = await _studentService.GetCurrentlyEnrolledCoursesAsync(studentId, semester);
                var completedThisYear = await _studentService.GetCoursesCompletedInCurrentYearAsync(studentId);
                var totalCompleted = await _studentService.GetTotalCoursesCompletedAsync(studentId);
                var progress = await _studentService.GetEnrollmentProgressAsync(studentId);
                var gpaBySemester = await _studentService.GetGpaBySemesterAsync(studentId);

                await _userActivityService.LogActivityAsync(studentId, "student", "view_dashboard", new { semester });

                return Ok(new
                {
                    CurrentlyEnrolledCourses = currentlyEnrolled,
                    CoursesCompletedInCurrentYear = completedThisYear,
                    TotalCoursesCompleted = totalCompleted,
                    EnrollmentProgress = progress,
                    GpaBySemester = gpaBySemester
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/dashboard/currently-enrolled/{semester}")]
        public async Task<IActionResult> GetCurrentlyEnrolledCourses(string studentId, string semester)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only view your own enrolled courses");
                }
                var count = await _studentService.GetCurrentlyEnrolledCoursesAsync(studentId, semester);
                await _userActivityService.LogActivityAsync(studentId, "student", "view_currently_enrolled", new { semester });
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/dashboard/completed-this-year")]
        public async Task<IActionResult> GetCoursesCompletedInCurrentYear(string studentId)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only view your own completed courses");
                }
                var count = await _studentService.GetCoursesCompletedInCurrentYearAsync(studentId);
                await _userActivityService.LogActivityAsync(studentId, "student", "view_completed_this_year", new { });
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/dashboard/total-completed")]
        public async Task<IActionResult> GetTotalCoursesCompleted(string studentId)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only view your own total completed courses");
                }
                var count = await _studentService.GetTotalCoursesCompletedAsync(studentId);
                await _userActivityService.LogActivityAsync(studentId, "student", "view_total_completed", new { });
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/dashboard/enrollment-progress")]
        public async Task<IActionResult> GetEnrollmentProgress(string studentId)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only view your own enrollment progress");
                }
                var progress = await _studentService.GetEnrollmentProgressAsync(studentId);
                await _userActivityService.LogActivityAsync(studentId, "student", "view_enrollment_progress", new { });
                return Ok(progress);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/dashboard/gpa-by-semester")]
        public async Task<IActionResult> GetGpaBySemester(string studentId)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only view your own GPA by semester");
                }
                var gpaBySemester = await _studentService.GetGpaBySemesterAsync(studentId);
                await _userActivityService.LogActivityAsync(studentId, "student", "view_gpa_by_semester", new { });
                return Ok(gpaBySemester);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/courses/{courseId}/prerequisites-graph")]
        public async Task<IActionResult> GetPrerequisiteGraph(string studentId, int courseId)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only view your own prerequisite graph");
                }
                var graph = await _studentService.GetPrerequisiteGraphAsync(studentId, courseId);
                await _userActivityService.LogActivityAsync(studentId, "student", "view_prerequisite_graph", new { courseId });
                return Ok(graph);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/program")]
        public async Task<IActionResult> GetStudentProgram(string studentId)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only view your own program");
                }
                var program = await _studentService.GetStudentProgramAsync(studentId);
                await _userActivityService.LogActivityAsync(studentId, "student", "view_program", new { });
                return Ok(program);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpGet("{studentId}/activities")]
        public async Task<IActionResult> GetUserActivities(string studentId)
        {
            try
            {
                if (studentId != User.FindFirst("studentId")?.Value)
                {
                    return Unauthorized("You can only view your own activities");
                }
                var activities = await _userActivityService.GetUserActivitiesAsync(studentId);
                await _userActivityService.LogActivityAsync(studentId, "student", "view_activities", new { });
                return Ok(activities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //private string GenerateJwtToken(StudentDto student)
        //{
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        claims: new[]
        //        {
        //            new Claim("studentId", student.StudentId),
        //            new Claim(ClaimTypes.Role, "STUDENT")
        //        },
        //        expires: DateTime.Now.AddHours(1),
        //        signingCredentials: creds);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}