using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface ICourseService
    {
        Task<CourseDto?> GetCourseByIdAsync(string courseId);
        Task<List<CourseDto>> GetCoursesByProgramAsync(string program);
        Task<List<CourseDto>> GetCoursesByProgramAndYearAsync(string program, int year);
        Task<bool> RegisterCourseAsync(CourseRegistrationDto registrationDto);
        Task<bool> UnregisterCourseAsync(CourseRegistrationDto registrationDto);
        Task<List<CourseDto>> GetAvailableCoursesAsync(string studentId);
        Task<List<CourseDto>> GetRegisteredCoursesAsync(string studentId);
        Task<List<CourseDto>> GetCourseHistoryAsync(string studentId);
        Task<List<CourseRequirementDto>> GetCoursePrerequisitesAsync(string courseId);
    }
}