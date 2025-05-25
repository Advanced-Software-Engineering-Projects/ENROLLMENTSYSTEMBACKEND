using ENROLLMENTSYSTEMBACKEND.Models;

namespace StudentSystemBackend.Repositories
{
    public interface IStudentRepository
    {
        Task<Student> GetByIdAsync(string id);
        Task<Student> GetByEmailAsync(string email);
        Task<List<Enrollment>> GetEnrollmentsByStudentIdAsync(string studentId);
        Task<List<Student>> GetAllAsync();
        Task UpdateAsync(Student student);
    }
}