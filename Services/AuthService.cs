using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
            if (student != null && student.Password == loginDto.Password) // Simplified check; use proper hashing in production
            {
                return GenerateUserDto(student.StudentId, student.FirstName, student.LastName, student.Email, "student");
            }

            var admin = await _adminRepository.GetByUsernameAsync(loginDto.Username);
            if (admin != null && admin.Password == loginDto.Password)
            {
                return GenerateUserDto(admin.AdminId, admin.Username, "", admin.Email, "admin");
            }

            throw new UnauthorizedAccessException("Invalid credentials");
        }

        private UserDto GenerateUserDto(string id, string firstName, string lastName, string email, string role)
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
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}