using ENROLLMENTSYSTEMBACKEND.Models;

namespace ENROLLMENTSYSTEMBACKEND.IRepositories
{
    public interface IUserActivityRepository
    {
        Task AddActivityAsync(UserActivity activity);
        Task<IEnumerable<UserActivity>> GetActivitiesByUserIdAsync(string userId);
        Task<IEnumerable<UserActivity>> GetActivitiesAsync(string? userId, DateTime? startDate, DateTime? endDate);
    }
}
