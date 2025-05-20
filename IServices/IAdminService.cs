using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.IServices
{
    public interface IAdminService
    {
        Task<AdminDto> LoginAsync(string username, string password);
        Task<AdminDto> RegisterAsync(AdminDto adminDto);
        Task<Programs> AddProgramAsync(Programs program);
        Task<Programs> UpdateProgramAsync(int programId, Programs program);
        Task<bool> DeleteProgramAsync(int programId);
        Task<bool> SubmitGradeAsync(int enrollmentId, string grade);
        Task<Course> AddCourseToProgramAsync(int programId, Course course);
        Task<bool> RemoveCourseFromProgramAsync(int programId, int courseId);
    }
}