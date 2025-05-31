using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> GetStudentByIdAsync(string studentId);
        Task<List<Student>> GetStudentsAsync();
        Task<List<Student>> GetAllStudentsAsync();
        Task<int> GetRegisteredStudentsCountAsync();
        Task<List<Student>> GetAllStudentsAsync(int page, int pageSize);
        Task<int> GetTotalStudentsCountAsync();
        Task UpdateStudentAsync(Student student);
    }
}