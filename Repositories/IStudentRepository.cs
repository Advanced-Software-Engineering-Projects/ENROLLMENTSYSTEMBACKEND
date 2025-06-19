using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IStudentRepository
    {
        Task<Student?> GetStudentByIdAsync(string studentId);
        Task<List<Enrollment>> GetEnrollmentsAsync(string studentId);
        Task<List<Enrollment>> GetActiveEnrollmentsAsync(string studentId);
        Task<bool> IsEligibleForGraduationAsync(string studentId);
        Task<bool> UpdateStudentAsync(Student student);
        Task<List<Student>> GetAllStudentsAsync();
        Task<List<Student>> GetAllStudentsAsync(int page, int pageSize);
        Task<int> GetTotalStudentsCountAsync();
        Task<int> GetRegisteredStudentsCountAsync();
    }
}