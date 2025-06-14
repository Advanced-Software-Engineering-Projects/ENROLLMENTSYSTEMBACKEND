using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IRegistrationPeriodService _registrationPeriodService;
        private readonly IAdminDashboardService _adminDashboardService;

        public AdminController(
            IRegistrationPeriodService registrationPeriodService,
            IAdminDashboardService adminDashboardService)
        {
            _registrationPeriodService = registrationPeriodService;
            _adminDashboardService = adminDashboardService;
        }

        
        //Opens a new registration period.
        [HttpPost("registration/open")]
        public async Task<IActionResult> OpenRegistration([FromBody] RegistrationPeriodDto periodDto)
        {
            if (periodDto == null || periodDto.StartDate == default || periodDto.EndDate == default)
            {
                return BadRequest("Invalid registration period data.");
            }

            try
            {
                await _registrationPeriodService.OpenRegistrationAsync(periodDto.StartDate, periodDto.EndDate);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Registration period already open"
            }
        }


        //Closes the current registration period.
        [HttpPost("registration/close")]
        public async Task<IActionResult> CloseRegistration()
        {
            try
            {
                await _registrationPeriodService.CloseRegistrationAsync();
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "No active registration period"
            }
        }

     
        //Gets the status of the registration period.
        [HttpGet("registration/status")]
        public async Task<IActionResult> GetRegistrationStatus()
        {
            try
            {
                var status = await _registrationPeriodService.GetRegistrationStatusAsync();
                return Ok(status);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No registration period found"
            }
        }

        //Gets all registration periods.
        [HttpGet("registration/periods")]
        public async Task<IActionResult> GetRegistrationPeriods()
        {
            try
            {
                var periods = await _registrationPeriodService.GetAllRegistrationPeriodsAsync();
                return Ok(periods);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No registration periods found"
            }
        }

     
        //Gets the current registration period.
        [HttpGet("registration/current")]
        public async Task<IActionResult> GetCurrentRegistrationPeriod()
        {
            try
            {
                var currentPeriod = await _registrationPeriodService.GetCurrentRegistrationPeriodAsync();
                if (currentPeriod == null)
                {
                    return NotFound("No current registration period found.");
                }
                return Ok(currentPeriod);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        
        //Gets key metrics for the admin dashboard (e.g., registered students, active courses, pending approvals).
        [HttpGet("dashboard/metrics")]
        public async Task<IActionResult> GetDashboardMetrics()
        {
            try
            {
                var metrics = await _adminDashboardService.GetDashboardMetricsAsync();
                return Ok(metrics);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No data available"
            }
        }

        //Gets pending approval requests for the admin dashboard.
        [HttpGet("dashboard/pending-requests")]
        public async Task<IActionResult> GetPendingRequests()
        {
            try
            {
                var requests = await _adminDashboardService.GetPendingRequestsAsync();
                return Ok(requests);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No pending requests found"
            }
        }

        //Gets enrollment data by semester for the admin dashboard.
        [HttpGet("dashboard/enrollment-data")]
        public async Task<IActionResult> GetEnrollmentData()
        {
            try
            {
                var enrollmentData = await _adminDashboardService.GetEnrollmentDataAsync();
                return Ok(enrollmentData);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No enrollment data available"
            }
        }

        //Gets course completion rate data by semester for the admin dashboard.
        [HttpGet("dashboard/completion-rate")]
        public async Task<IActionResult> GetCompletionRateData()
        {
            try
            {
                var completionRateData = await _adminDashboardService.GetCompletionRateDataAsync();
                return Ok(completionRateData);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No completion rate data available"
            }
        }

        //Opens a registration period for specific courses.
        [HttpPost("registration/open-courses")]
        public async Task<IActionResult> OpenCourseRegistration([FromBody] CourseRegistrationDto courseRegistrationDto)
        {
            if (courseRegistrationDto == null ||
                courseRegistrationDto.StartDate == default ||
                courseRegistrationDto.EndDate == default ||
                courseRegistrationDto.StartTime == TimeSpan.Zero ||
                courseRegistrationDto.EndTime == TimeSpan.Zero ||
                courseRegistrationDto.CourseCodes == null ||
                courseRegistrationDto.CourseCodes.Count == 0)
            {
                return BadRequest("Start date, end date, start time, end time, and at least one course code are required.");
            }

            try
            {
                await _registrationPeriodService.OpenCourseRegistrationAsync(courseRegistrationDto);
                return Ok(new { Message = "Registration period opened successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Registration period already open for these courses"
            }
        }

        //Closes a registration period for specific courses.
        [HttpPost("registration/close-courses")]
        public async Task<IActionResult> CloseCourseRegistration([FromBody] CourseCloseDto courseCloseDto)
        {
            if (courseCloseDto == null || courseCloseDto.CourseCodes == null || courseCloseDto.CourseCodes.Count == 0)
            {
                return BadRequest("At least one course code is required.");
            }

            try
            {
                await _registrationPeriodService.CloseCourseRegistrationAsync(courseCloseDto.CourseCodes);
                return Ok(new { Message = "Registration period closed successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "No active registration period for these courses"
            }
        }


        //Gets registration metrics (total registrations and dropped courses).
        [HttpGet("registration/metrics")]
        public async Task<IActionResult> GetRegistrationMetrics()
        {
            try
            {
                var metrics = await _registrationPeriodService.GetRegistrationMetricsAsync();
                return Ok(metrics);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // e.g., "No metrics available"
            }
        }
    }
}