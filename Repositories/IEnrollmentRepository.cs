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
        Task<List<(string Semester, int Count)>> GetEnrollmentCountsBySemesterAsync();
        Task<List<Enrollment>> GetEnrollmentsBySemesterAsync(string studentId, string semester);
        Task UpdateGradeAsync(string enrollmentId, string grade);
        Task<List<Enrollment>> GetCompletedEnrollmentsAsync(string studentId);
    }
}