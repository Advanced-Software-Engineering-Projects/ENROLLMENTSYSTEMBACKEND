using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface ICourseService
    {
        Task<List<Course>> GetAvailableCoursesAsync(string studentId);
        Task<bool> RegisterCourseAsync(CourseRegistrationDto registrationDto);
        Task<List<Course>> GetRegisteredCoursesAsync(string studentId);
        Task<bool> UnregisterCourseAsync(CourseRegistrationDto registrationDto);
        Task<List<Enrollment>> GetCourseHistoryAsync(string studentId);
        Task<List<Course>> GetCoursePrerequisitesAsync(string courseId);
        Task<CourseDto> GetCourseDetailsAsync(string courseCode);
        Task<bool> RegisterCourseAsync(string studentId, string courseCode);
        Task<bool> DropCourseAsync(string studentId, string courseCode);
        Task<List<string>> GetPrerequisitesAsync(string courseCode);
        Task<bool> AddCourseAsync(CourseDto course);
        Task<bool> UpdateCourseAsync(string courseCode, CourseDto updatedCourse);
        Task<bool> DeleteCourseAsync(string courseCode);
    }
}
