using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.IServices;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Services;

namespace USPENROLLMENTSYSTEMBACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAdminService _adminService;
        private readonly ISystemConfigService _systemConfigService;
        private readonly IUserActivityService _userActivityService;

        public AdminController(
            IConfiguration configuration,
            IAdminService adminService,
            ISystemConfigService systemConfigService,
            IUserActivityService userActivityService)
        {
            _configuration = configuration;
            _adminService = adminService;
            _systemConfigService = systemConfigService;
            _userActivityService = userActivityService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var admin = await _adminService.LoginAsync(request.Username, request.Password);
                var token = GenerateJwtToken(admin);
                await _userActivityService.LogActivityAsync(admin.AdminId, "admin", "login", new { });
                return Ok(new { admin, token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("logout")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var adminId = User.FindFirst("adminId")?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized("Invalid token");
                }
                await _userActivityService.LogActivityAsync(adminId, "admin", "logout", new { });
                return Ok("Logged out successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("registration/toggle")]
        public async Task<IActionResult> ToggleRegistration([FromBody] bool isOpen)
        {
            try
            {
                var adminId = User.FindFirst("adminId")?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized("Invalid token");
                }

                var configDto = new SystemConfigDto
                {
                    Key = "Registration:IsOpen",
                    Value = isOpen.ToString()
                };

                var existingConfig = await _systemConfigService.GetConfigValueAsync(configDto.Key);
                if (existingConfig == null)
                {
                    await _systemConfigService.AddConfigAsync(configDto);
                }
                else
                {
                    var config = (await _systemConfigService.GetAllConfigsAsync())
                        .FirstOrDefault(c => c.Key == configDto.Key);
                    if (config != null)
                    {
                        config.Value = configDto.Value;
                        await _systemConfigService.UpdateConfigAsync(config.ConfigId, config);
                    }
                }

                await _userActivityService.LogActivityAsync(adminId, "admin", "toggle_registration", new { isOpen });
                return Ok(new { IsOpen = isOpen });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AdminDto adminDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var admin = await _adminService.RegisterAsync(adminDto);
                await _userActivityService.LogActivityAsync(admin.AdminId, "admin", "register", adminDto);
                return Ok(admin);
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

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("programs")]
        public async Task<IActionResult> AddProgram([FromBody] Programs program)
        {
            try
            {
                var adminId = User.FindFirst("adminId")?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized("Invalid token");
                }

                var addedProgram = await _adminService.AddProgramAsync(program);
                await _userActivityService.LogActivityAsync(adminId, "admin", "add_program", new { programId = addedProgram.ProgramId });
                return Ok(addedProgram);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("programs/{programId}")]
        public async Task<IActionResult> UpdateProgram(int programId, [FromBody] Programs program)
        {
            try
            {
                var adminId = User.FindFirst("adminId")?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized("Invalid token");
                }

                var updatedProgram = await _adminService.UpdateProgramAsync(programId, program);
                await _userActivityService.LogActivityAsync(adminId, "admin", "update_program", new { programId });
                return Ok(updatedProgram);
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

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("programs/{programId}")]
        public async Task<IActionResult> DeleteProgram(int programId)
        {
            try
            {
                var adminId = User.FindFirst("adminId")?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized("Invalid token");
                }

                var success = await _adminService.DeleteProgramAsync(programId);
                if (!success)
                {
                    return NotFound("Program not found");
                }

                await _userActivityService.LogActivityAsync(adminId, "admin", "delete_program", new { programId });
                return Ok("Program deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("enrollments/{enrollmentId}/grade")]
        public async Task<IActionResult> SubmitGrade(int enrollmentId, [FromBody] string grade)
        {
            try
            {
                var adminId = User.FindFirst("adminId")?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized("Invalid token");
                }

                var success = await _adminService.SubmitGradeAsync(enrollmentId, grade);
                if (!success)
                {
                    return NotFound("Enrollment not found");
                }

                await _userActivityService.LogActivityAsync(adminId, "admin", "submit_grade", new { enrollmentId, grade });
                return Ok("Grade submitted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("programs/{programId}/courses")]
        public async Task<IActionResult> AddCourseToProgram(int programId, [FromBody] Course course)
        {
            try
            {
                var adminId = User.FindFirst("adminId")?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized("Invalid token");
                }

                var addedCourse = await _adminService.AddCourseToProgramAsync(programId, course);
                await _userActivityService.LogActivityAsync(adminId, "admin", "add_course_to_program", new { programId, courseId = addedCourse.CourseId });
                return Ok(addedCourse);
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

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("audit-logs")]
        public async Task<IActionResult> GetAuditLogs([FromQuery] string? userId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var adminId = User.FindFirst("adminId")?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized("Invalid token");
                }

                var activities = await _userActivityService.GetAuditLogsAsync(userId, startDate, endDate);
                await _userActivityService.LogActivityAsync(adminId, "admin", "view_audit_logs", new { userId, startDate, endDate });
                return Ok(activities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("programs/{programId}/courses/{courseId}")]
        public async Task<IActionResult> RemoveCourseFromProgram(int programId, int courseId)
        {
            try
            {
                var adminId = User.FindFirst("adminId")?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized("Invalid token");
                }

                var success = await _adminService.RemoveCourseFromProgramAsync(programId, courseId);
                if (!success)
                {
                    return NotFound("Course or program not found");
                }

                await _userActivityService.LogActivityAsync(adminId, "admin", "remove_course_from_program", new { programId, courseId });
                return Ok("Course removed from program successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("configs")]
        public async Task<IActionResult> GetAllConfigs()
        {
            try
            {
                var adminId = User.FindFirst("adminId")?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized("Invalid token");
                }

                var configs = await _systemConfigService.GetAllConfigsAsync();
                await _userActivityService.LogActivityAsync(adminId, "admin", "view_configs", new { });
                return Ok(configs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("configs")]
        public async Task<IActionResult> AddConfig([FromBody] SystemConfigDto configDto)
        {
            try
            {
                var adminId = User.FindFirst("adminId")?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized("Invalid token");
                }

                var addedConfig = await _systemConfigService.AddConfigAsync(configDto);
                await _userActivityService.LogActivityAsync(adminId, "admin", "add_config", configDto);
                return Ok(addedConfig);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("configs/{configId}")]
        public async Task<IActionResult> UpdateConfig(int configId, [FromBody] SystemConfigDto configDto)
        {
            try
            {
                var adminId = User.FindFirst("adminId")?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized("Invalid token");
                }

                var updatedConfig = await _systemConfigService.UpdateConfigAsync(configId, configDto);
                await _userActivityService.LogActivityAsync(adminId, "admin", "update_config", configDto);
                return Ok(updatedConfig);
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

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("configs/{configId}")]
        public async Task<IActionResult> DeleteConfig(int configId)
        {
            try
            {
                var adminId = User.FindFirst("adminId")?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized("Invalid token");
                }

                var success = await _systemConfigService.DeleteConfigAsync(configId);
                if (!success)
                {
                    return NotFound("Configuration not found");
                }

                await _userActivityService.LogActivityAsync(adminId, "admin", "delete_config", new { configId });
                return Ok("Configuration deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("activities/{userId}")]
        public async Task<IActionResult> GetUserActivities(string userId)
        {
            try
            {
                var adminId = User.FindFirst("adminId")?.Value;
                if (string.IsNullOrEmpty(adminId))
                {
                    return Unauthorized("Invalid token");
                }

                var activities = await _userActivityService.GetUserActivitiesAsync(userId);
                await _userActivityService.LogActivityAsync(adminId, "admin", "view_user_activities", new { userId });
                return Ok(activities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string GenerateJwtToken(AdminDto admin)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: new[]
                {
                    new Claim("adminId", admin.AdminId),
                    new Claim(ClaimTypes.Role, admin.Role)
                },
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}