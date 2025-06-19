using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.Repositories
{
    public interface ICourseManagementRepository
    {
        Task<List<CourseRegistrationPeriod>> GetOpenRegistrationsAsync();
        Task<List<ClosedRegistration>> GetClosedRegistrationsAsync();
        Task AddOpenRegistrationAsync(CourseRegistrationPeriod registration);
        Task CloseRegistrationAsync(List<string> courseCodes);
        Task<int> GetTotalRegistrationsCountAsync();
        Task<int> GetTotalDroppedCoursesCountAsync();
        Task<List<Course>> GetAllCoursesAsync();
    }
}