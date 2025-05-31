using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IStudentService
    {
        Task<List<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto> GetStudentByIdAsync(string id);
        Task<List<StudentDto>> GetAllStudentsAsync(int page, int pageSize);
        Task<int> GetTotalStudentsCountAsync();
        Task UpdateStudentAsync(StudentDto studentDto);
        Task<string> UploadAvatarAsync(string studentId, IFormFile file);
    }
}
