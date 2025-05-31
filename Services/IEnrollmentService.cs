using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<CourseDto>> GetEnrolledCoursesAsync(string studentId);
        Task<IEnumerable<CourseDto>> GetDroppedCoursesAsync(string studentId);
        Task<IEnumerable<CourseDto>> GetAvailableCoursesAsync(string studentId, string program);
        Task<IEnumerable<string>> GetPrerequisiteGraphAsync(string courseCode);
        Task EnrollAsync(string studentId, string courseCode, string semester);
        Task DropAsync(string studentId, string courseCode);
    }
}
