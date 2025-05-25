using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface IEnrollmentRepository
    {
        Task AddEnrollmentAsync(string studentId, int courseId, string semester);
        Task RemoveEnrollmentAsync(string studentId, int courseId, string semester);
        Task<List<Enrollment>> GetEnrollmentsByStudentAsync(string studentId);
        Task<List<Course>> GetCoursesBySemesterAsync(string semester);
    }
}