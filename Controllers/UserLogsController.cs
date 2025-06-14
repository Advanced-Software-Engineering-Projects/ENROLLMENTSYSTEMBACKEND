using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ENROLLMENTSYSTEMBACKEND.Services;
using ENROLLMENTSYSTEMBACKEND.DTOs;
using System.Security.Claims;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // Ensure only admin users can access these endpoints
    public class UserLogsController : ControllerBase
    {
        private readonly IUserLogService _userLogService;

        public UserLogsController(IUserLogService userLogService)
        {
            _userLogService = userLogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLogDto>>> GetAllLogs()
        {
            try
            {
                var logs = await _userLogService.GetAllUserLogsAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving user logs", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserLogDto>> GetLogById(int id)
        {
            try
            {
                var log = await _userLogService.GetUserLogByIdAsync(id);
                if (log == null)
                    return NotFound(new { message = "User log not found" });

                return Ok(log);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving user log", error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserLogDto>>> GetLogsByUserId(string userId)
        {
            try
            {
                var logs = await _userLogService.GetUserLogsByUserIdAsync(userId);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving user logs", error = ex.Message });
            }
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<UserLogDto>>> GetLogsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var logs = await _userLogService.GetUserLogsByDateRangeAsync(startDate, endDate);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving user logs", error = ex.Message });
            }
        }

        // This endpoint can be used to manually create a log entry if needed
        [HttpPost]
        public async Task<ActionResult<UserLogDto>> CreateLog(string userId, string activity)
        {
            try
            {
                var log = await _userLogService.CreateUserLogAsync(userId, activity);
                return CreatedAtAction(nameof(GetLogById), new { id = log.UserLogId }, log);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating user log", error = ex.Message });
            }
        }
    }
}