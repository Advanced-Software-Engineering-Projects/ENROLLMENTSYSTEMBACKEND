using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.Services
{
    public interface IRegistrationPeriodService
    {
        Task OpenRegistrationAsync(DateTime startDate, DateTime endDate);
        Task CloseRegistrationAsync();
        Task<RegistrationStatusDto> GetRegistrationStatusAsync();
        Task<List<RegistrationPeriodDto>> GetAllRegistrationPeriodsAsync();
        Task<RegistrationPeriodDto> GetCurrentRegistrationPeriodAsync();
        Task OpenCourseRegistrationAsync(CourseRegistrationDto courseRegistrationDto);
        Task CloseCourseRegistrationAsync(List<string> courseCodes);
        Task<RegistrationMetricsDto> GetRegistrationMetricsAsync();
    }
}
