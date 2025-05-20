using ENROLLMENTSYSTEMBACKEND.DTOs;

namespace ENROLLMENTSYSTEMBACKEND.IServices
{
    public interface IUserActivityService
    {
        Task LogActivityAsync(string userId, string userType, string action, object details);
        Task<IEnumerable<UserActivityDto>> GetUserActivitiesAsync(string userId);
        Task<IEnumerable<UserActivityDto>> GetAuditLogsAsync(string? userId, DateTime? startDate, DateTime? endDate);
    }
}