using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface ICourseManagementService
    {
        Task<CourseRegistrationStatusDto> GetRegistrationStatusAsync();
        Task OpenCourseRegistrationAsync(CourseManagementDto request);
        Task CloseCourseRegistrationAsync(CloseCourseRegistrationDto request);
        Task<RegistrationMetricsDto> GetRegistrationMetricsAsync();
        Task<List<CourseDto>> GetAllCoursesAsync();
    }
}