using ENROLLMENTSYSTEMBACKEND.Models;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using Microsoft.Extensions.Logging;
using BCrypt.Net;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            _logger.LogInformation("Authenticating user with email: {Email}", email);

            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} not found", email);
                return null;
            }

            if (!VerifyPassword(user.PasswordHash, password))
            {
                _logger.LogWarning("Password verification failed for email: {Email}", email);
                return null;
            }

            _logger.LogInformation("Authentication successful for email: {Email}", email);
            return user;
        }

        public async Task<string> ResetPasswordAsync(string token, string newPassword)
        {
            _logger.LogInformation("Attempting password reset with token: {Token}", token);

            if (string.IsNullOrEmpty(token) || token != "valid-token") // Simulated token validation
            {
                _logger.LogWarning("Invalid token for password reset: {Token}", token);
                return "Invalid token";
            }

            _logger.LogInformation("Password reset successful");
            return await Task.FromResult("Password reset successful");
        }

        private bool VerifyPassword(string storedHash, string providedPassword)
        {
            try
            {
                bool isMatch = BCrypt.Net.BCrypt.Verify(providedPassword, storedHash);
                _logger.LogInformation("Password verification result: {IsMatch}", isMatch);
                return isMatch;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying password: {ErrorMessage}", ex.Message);
                return false;
            }
        }
    }
}