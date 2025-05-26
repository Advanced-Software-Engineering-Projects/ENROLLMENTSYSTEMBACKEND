using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using StudentSystemBackend.Repositories;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class AuthService : IAuthService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IStudentRepository studentRepository, IAdminRepository adminRepository, IConfiguration configuration)
        {
            _studentRepository = studentRepository;
            _adminRepository = adminRepository;
            _configuration = configuration;
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var student = await _studentRepository.GetByEmailAsync(loginDto.Username);
            if (student != null && student.Password == loginDto.Password)
            {
                var refreshToken = GenerateRefreshToken();
                student.RefreshToken = refreshToken;
                student.RefreshTokenExpiry = DateTime.Now.AddDays(7);
                await _studentRepository.UpdateAsync(student);
                return GenerateUserDto(student.StudentId, student.FirstName, student.LastName, student.Email, "student", refreshToken);
            }

            var admin = await _adminRepository.GetByUsernameAsync(loginDto.Username);
            if (admin != null && admin.Password == loginDto.Password)
            {
                var refreshToken = GenerateRefreshToken();
                admin.RefreshToken = refreshToken;
                admin.RefreshTokenExpiry = DateTime.Now.AddDays(7);
                await _adminRepository.UpdateAsync(admin);
                return GenerateUserDto(admin.AdminId, admin.Username, "", admin.Email, "admin", refreshToken);
            }

            throw new UnauthorizedAccessException("Invalid credentials");
        }

        public async Task<UserDto> RefreshTokenAsync(string refreshToken)
        {
            var student = await _studentRepository.GetByRefreshTokenAsync(refreshToken);
            if (student != null && student.RefreshTokenExpiry > DateTime.Now)
            {
                var newRefreshToken = GenerateRefreshToken();
                student.RefreshToken = newRefreshToken;
                student.RefreshTokenExpiry = DateTime.Now.AddDays(7);
                await _studentRepository.UpdateAsync(student);
                return GenerateUserDto(student.StudentId, student.FirstName, student.LastName, student.Email, "student", newRefreshToken);
            }

            var admin = await _adminRepository.GetByRefreshTokenAsync(refreshToken);
            if (admin != null && admin.RefreshTokenExpiry > DateTime.Now)
            {
                var newRefreshToken = GenerateRefreshToken();
                admin.RefreshToken = newRefreshToken;
                admin.RefreshTokenExpiry = DateTime.Now.AddDays(7);
                await _adminRepository.UpdateAsync(admin);
                return GenerateUserDto(admin.AdminId, admin.Username, "", admin.Email, "admin", newRefreshToken);
            }

            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private UserDto GenerateUserDto(string id, string firstName, string lastName, string email, string role, string refreshToken)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Name, firstName),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new UserDto
            {
                Id = id,
                Name = $"{firstName} {lastName}".Trim(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Role = role,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            };
        }
    }
}