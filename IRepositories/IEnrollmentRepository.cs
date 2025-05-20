using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.IRepositories
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment> GetEnrollmentByIdAsync(int enrollmentId);
        Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAsync(string studentId);
        Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAndSemesterAsync(string studentId, string semester);
        Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAndStatusAsync(string studentId, string status);
        Task AddEnrollmentAsync(Enrollment enrollment);
        Task UpdateEnrollmentAsync(Enrollment enrollment);
        Task DeleteEnrollmentAsync(int enrollmentId);
    }
}
