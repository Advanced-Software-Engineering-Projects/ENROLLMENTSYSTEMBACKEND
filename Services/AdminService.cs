using Microsoft.AspNetCore.Identity;
using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.IRepositories;
using ENROLLMENTSYSTEMBACKEND.IServices;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IProgramRepository _programRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IProgramCoursesRepository _programCoursesRepository;
        private readonly IPasswordHasher<Admin> _passwordHasher;

        public AdminService(
            IAdminRepository adminRepository,
            IProgramRepository programRepository,
            ICourseRepository courseRepository,
            IEnrollmentRepository enrollmentRepository,
            IProgramCoursesRepository programCoursesRepository,
            IPasswordHasher<Admin> passwordHasher)
        {
            _adminRepository = adminRepository;
            _programRepository = programRepository;
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
            _programCoursesRepository = programCoursesRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<AdminDto> LoginAsync(string username, string password)
        {
            var admin = await _adminRepository.GetAdminByUsernameAsync(username);
            if (admin == null || _passwordHasher.VerifyHashedPassword(admin, admin.Password, password) != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException("Invalid credentials");
            return new AdminDto { AdminId = admin.AdminId, Username = admin.Username, Role = admin.Role, Email = admin.Email };
        }

        public async Task<AdminDto> RegisterAsync(AdminDto adminDto)
        {
            var admin = new Admin
            {
                AdminId = adminDto.AdminId,
                Username = adminDto.Username,
                Role = adminDto.Role,
                Email = adminDto.Email,
                Password = _passwordHasher.HashPassword(null, "defaultPassword")
            };
            await _adminRepository.AddAdminAsync(admin);
            return adminDto;
        }

        public async Task<Programs> AddProgramAsync(Programs program)
        {
            await _programRepository.AddProgramAsync(program);
            return program;
        }

        public async Task<Programs> UpdateProgramAsync(int programId, Programs program)
        {
            var existing = await _programRepository.GetProgramByIdAsync(programId);
            if (existing == null) throw new KeyNotFoundException("Program not found");
            existing.Name = program.Name;
            await _programRepository.UpdateProgramAsync(existing);
            return existing;
        }

        public async Task<bool> DeleteProgramAsync(int programId)
        {
            var program = await _programRepository.GetProgramByIdAsync(programId);
            if (program == null) return false;
            await _programRepository.DeleteProgramAsync(programId);
            return true;
        }

        public async Task<bool> SubmitGradeAsync(int enrollmentId, string grade)
        {
            var enrollment = await _enrollmentRepository.GetEnrollmentByIdAsync(enrollmentId);
            if (enrollment == null) return false;
            enrollment.Grade = grade;
            enrollment.Status = "Completed";
            await _enrollmentRepository.UpdateEnrollmentAsync(enrollment);
            return true;
        }

        public async Task<Course> AddCourseToProgramAsync(int programId, Course course)
        {
            var program = await _programRepository.GetProgramByIdAsync(programId);
            if (program == null) throw new KeyNotFoundException("Program not found");
            await _courseRepository.AddCourseAsync(course);
            // Note: In a real system, you'd add to ProgramVersion via ProgramCourses, simplified here
            return course;
        }

        public async Task<bool> RemoveCourseFromProgramAsync(int programId, int courseId)
        {
            var pc = (await _programCoursesRepository.GetProgramCoursesByProgramVersionAsync(programId)).FirstOrDefault(pc => pc.CourseId == courseId);
            if (pc == null) return false;
            await _programCoursesRepository.DeleteProgramCourseAsync(pc.ProgramCourseId);
            return true;
        }
    }
}