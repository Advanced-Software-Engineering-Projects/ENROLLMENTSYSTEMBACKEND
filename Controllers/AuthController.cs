using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, IConfiguration configuration)
        {
            _authService = authService;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);

            var user = await _authService.AuthenticateAsync(loginDto.Email, loginDto.Password);
            if (user == null)
            {
                _logger.LogWarning("Authentication failed for email: {Email}", loginDto.Email);
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            var token = GenerateJwtToken(user);
            // Optionally keep session for other purposes, but not required for JWT
            //HttpContext.Session.SetString("UserId", user.Id);
            _logger.LogInformation("Login successful for email: {Email}, UserId: {UserId}", loginDto.Email, user.Id);
            return Ok(new { Message = "Login successful", Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role), // Add role if applicable
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiry = DateTime.UtcNow.AddMinutes(30); // Token expiration
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            
            _logger.LogInformation("JWT Token generated for user {UserId} with role {Role}, expires at {Expiry}", 
                user.Id, user.Role, expiry);
            
            return tokenString;
        }

        [HttpPost("ResetUserPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid data for password reset: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest("Invalid data.");
            }

            var result = await _authService.ResetPasswordAsync(dto.Token, dto.NewPassword);

            if (result.StartsWith("Invalid") || result.StartsWith("expired"))
            {
                _logger.LogWarning("Password reset failed: {Result}", result);
                return BadRequest(result);
            }

            _logger.LogInformation("Password reset successful");
            return Ok(result);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            _logger.LogInformation("User logged out successfully");
            return Ok(new { Message = "Logout successful" });
        }

        [HttpPost("extend-session")]
        public IActionResult ExtendSession()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                _logger.LogWarning("Session expired during extend-session attempt");
                return Unauthorized(new { Message = "Session expired" });
            }

            // Extend the session by resetting the idle timeout
            HttpContext.Session.SetString("UserId", HttpContext.Session.GetString("UserId"));
            _logger.LogInformation("Session extended for UserId: {UserId}", HttpContext.Session.GetString("UserId"));
            return Ok(new { Message = "Session extended" });
        }

    }
}