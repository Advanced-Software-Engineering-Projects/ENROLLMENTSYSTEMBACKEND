using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<List<Enrollment>> GetEnrollmentsByStudentIdAsync(string studentId);
        Task<List<Enrollment>> GetEnrollmentsAsync();
        Task<List<Enrollment>> GetEnrollmentsByCourseIdAsync(string courseId);
        Task<Enrollment> GetEnrollmentByIdAsync(string enrollmentId);
        Task AddEnrollmentAsync(Enrollment enrollment);
        Task UpdateEnrollmentAsync(Enrollment enrollment);
        Task DeleteEnrollmentAsync(string enrollmentId);
        Task<List<Enrollment>> GetAllEnrollmentsAsync();
    }
}