using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ENROLLMENTSYSTEMBACKEND.Services;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http;
using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/hold-management")]
    public class AdminHoldManagementController : ControllerBase
    {
        private readonly HoldManagementServiceClient _holdClient;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5003/api";

        public AdminHoldManagementController(HoldManagementServiceClient holdClient, IHttpClientFactory httpClientFactory)
        {
            _holdClient = holdClient;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("service-access-rules")]
        public async Task<IActionResult> GetServiceAccessRules()
        {
            var response = await _holdClient.GetServiceAccessRulesAsync();
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        [HttpPut("service-access-rules")]
        public async Task<IActionResult> UpdateServiceAccessRules([FromBody] Dictionary<string, bool> allowedServices)
        {
            if (allowedServices == null)
            {
                return BadRequest("Allowed services cannot be null");
            }

            var response = await _holdClient.UpdateServiceAccessRulesAsync(allowedServices);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return Ok();
        }

        [HttpGet("services")]
        public async Task<IActionResult> GetAvailableServices()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/HoldManagement/services");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        [HttpGet("student-holds")]
        public async Task<IActionResult> GetAllHolds()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/HoldManagement/student-holds");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        [HttpPost("student-holds")]
        public async Task<IActionResult> AddHold([FromBody] HoldDto holdDto)
        {
            var response = await _holdClient.AddHoldAsync(holdDto.StudentId, "FeeNonPayment", holdDto.Reason);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return Ok();
        }

        [HttpDelete("student-holds/{holdId}")]
        public async Task<IActionResult> RemoveHold(string holdId)
        {
            var response = await _holdClient.RemoveHoldAsync(holdId);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            return Ok();
        }
    }
}